﻿using CommonClassLibrary.Enum;
using ControlRoomDbService.BLL.Interfaces;
using ControlRoomDbService.DAL.Interfaces;
using ControlRoomDbService.Models;
using Newtonsoft.Json;

namespace ControlRoomDbService.BLL.Services
{
    public class HardwareService(IHardwareRepository hardwareRepository, IKafkaService kafkaService) : IHardwareService
    {
        private readonly IHardwareRepository _hardwareRepository = hardwareRepository;
        private readonly IKafkaService _kafkaService = kafkaService;

        public async Task<List<Hardware>> GetAllHardwareAsync()
        {
            return await _hardwareRepository.GetAsync();
        }

        public async Task<Hardware?> GetHardwareByIdAsync(string id)
        {
            return await _hardwareRepository.GetByIdAsync(id);
        }

        public async Task CreateHardwareAsync(Hardware hardware)
        {
            await _hardwareRepository.CreateAsync(hardware);
            string message = JsonConvert.SerializeObject(
                new
                {
                    TimeStamp = DateTime.Now,
                    Action = MessageAction.Create.ToString()
                });
            string topic = MessageTopic.ControlRoom.ToKafkaTopic();
            _kafkaService.SendMessage(topic, message);
        }

        public async Task UpdateHardwareAsync(string id, Hardware updatedHardware)
        {
            await _hardwareRepository.UpdateAsync(id, updatedHardware);
            string message = JsonConvert.SerializeObject(
                new
                {
                    TimeStamp = DateTime.Now,
                    Action = MessageAction.Update.ToString()
                });
            string topic = MessageTopic.ControlRoom.ToKafkaTopic();
            _kafkaService.SendMessage(topic, message);
        }

        public async Task DeleteHardwareAsync(string id)
        {
            await _hardwareRepository.RemoveAsync(id);
            string message = JsonConvert.SerializeObject(
                new
                {
                    TimeStamp = DateTime.Now,
                    Action = MessageAction.Delete.ToString()
                });
            string topic = MessageTopic.ControlRoom.ToKafkaTopic();
            _kafkaService.SendMessage(topic, message);
        }
    }

}
