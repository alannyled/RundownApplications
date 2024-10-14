using RundownDbService.BLL.Interfaces;
using RundownDbService.DAL.Interfaces;
using RundownDbService.Models;
using RundownDbService.BLL.Services;
using Confluent.Kafka;
using System.Text.Json;
using System;

namespace RundownDbService.BLL.Services
{
    public class RundownService(IRundownRepository rundownRepository, KafkaService kafkaService) : IRundownService
    {
        private readonly IRundownRepository _rundownRepository = rundownRepository;
        private readonly KafkaService _kafkaService = kafkaService;

        public async Task<List<Rundown>> GetAllRundownsAsync()
        {
            return await _rundownRepository.GetAllAsync();
        }

        public async Task<Rundown> GetRundownByIdAsync(Guid uuid)
        {
            return await _rundownRepository.GetByIdAsync(uuid);
        }

        public async Task CreateRundownAsync(Rundown newRundown)
        {
            
            await _rundownRepository.CreateAsync(newRundown);
            var messageObject = new
            {
                Action = "create",
                uuid = newRundown.UUID.ToString(),
                rundown = newRundown.Name
            };
            string message = JsonSerializer.Serialize(messageObject);
            _kafkaService.SendMessage("rundown", message);
        }

        public async Task<Rundown> UpdateRundownAsync(Guid uuid, Rundown updatedRundown)
        {
            var rundown = await _rundownRepository.UpdateAsync(uuid, updatedRundown);
            var messageObject = new
            {
                Action = "update",
                uuid = uuid.ToString(),
                rundown = updatedRundown.Name 
            };

            string message = JsonSerializer.Serialize(messageObject);
            _kafkaService.SendMessage("rundown", message);
            return rundown;
        }

        public async Task DeleteRundownAsync(Guid uuid)
        {
            await _rundownRepository.DeleteAsync(uuid);
            var messageObject = new
            {
                Action = "delete",
                uuid = uuid.ToString(),
                rundown = string.Empty
            };
            string message = JsonSerializer.Serialize(messageObject);
            _kafkaService.SendMessage("rundown", message);
        }
    }
}
