using CommonClassLibrary.Enum;
using ControlRoomDbService.BLL.Interfaces;
using ControlRoomDbService.DAL.Interfaces;
using ControlRoomDbService.Models;
using Newtonsoft.Json;

namespace ControlRoomDbService.BLL.Services
{
    public class ControlRoomService(IControlRoomRepository controlRoomRepository, IKafkaService kafkaService) : IControlRoomService
    {
        private readonly IControlRoomRepository _controlRoomRepository = controlRoomRepository;
        private readonly IKafkaService _kafkaService = kafkaService;

        public async Task<List<ControlRoom>> GetControlRoomsAsync()
        {
            return await _controlRoomRepository.GetAsync();
        }

        public async Task<ControlRoom> GetControlRoomByIdAsync(string id)
        {
            return await _controlRoomRepository.GetByIdAsync(id) ?? new ControlRoom();
        }

        public async Task CreateControlRoomAsync(ControlRoom controlRoom)
        {
            await _controlRoomRepository.CreateAsync(controlRoom);
            string message = JsonConvert.SerializeObject(new 
            { 
                TimeStamp = DateTime.Now, 
                Action = MessageAction.Create.ToString() 
            });
            string topic = MessageTopic.ControlRoom.ToKafkaTopic();
            _kafkaService.SendMessage(topic, message);
        }

        public async Task UpdateControlRoomAsync(string id, ControlRoom updatedControlRoom)
        {
            var controlRooms = await _controlRoomRepository.UpdateAsync(id, updatedControlRoom);
            string message = JsonConvert.SerializeObject(new
            {
                TimeStamp = DateTime.Now,
                Action = MessageAction.Update.ToString(),
                ControlRooms = controlRooms
            });
            string topic = MessageTopic.ControlRoom.ToKafkaTopic();
            _kafkaService.SendMessage(topic, message);
        }


        public async Task DeleteControlRoomAsync(string id)
        {
            await _controlRoomRepository.RemoveAsync(id);
            string message = JsonConvert.SerializeObject(new 
            { 
                TimeStamp = DateTime.Now, 
                Action = MessageAction.Delete.ToString() 
            });
            string topic = MessageTopic.ControlRoom.ToKafkaTopic();
            _kafkaService.SendMessage(topic, message);
        }

        public async Task DeleteAllControlRoomsAsync()
        {
            await _controlRoomRepository.RemoveAllAsync();
        }
    }

}
