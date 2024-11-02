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
            string message = JsonConvert.SerializeObject(new { TimeStamp = DateTime.Now, Action = "create" });
            _kafkaService.SendMessage("controlroom", message);
        }

        public async Task UpdateControlRoomAsync(string id, ControlRoom updatedControlRoom)
        {
            await _controlRoomRepository.UpdateAsync(id, updatedControlRoom);
            string message = JsonConvert.SerializeObject(new { TimeStamp = DateTime.Now, Action = "update" });
            _kafkaService.SendMessage("controlroom", message);
        }

        public async Task DeleteControlRoomAsync(string id)
        {
            await _controlRoomRepository.RemoveAsync(id);
            string message = JsonConvert.SerializeObject(new { TimeStamp = DateTime.Now, Action = "delete" });
            _kafkaService.SendMessage("controlroom", message);
        }

        public async Task DeleteAllControlRoomsAsync()
        {
            await _controlRoomRepository.RemoveAllAsync();
        }
    }

}
