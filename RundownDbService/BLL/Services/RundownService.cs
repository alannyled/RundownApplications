using RundownDbService.BLL.Interfaces;
using RundownDbService.DAL.Interfaces;
using RundownDbService.Models;
using Newtonsoft.Json;
using CommonClassLibrary.Services;

namespace RundownDbService.BLL.Services
{
    public class RundownService(IRundownRepository rundownRepository, IKafkaService kafkaService, ResilienceService resilienceService) : IRundownService
    {
        private readonly IRundownRepository _rundownRepository = rundownRepository;
        private readonly IKafkaService _kafkaService = kafkaService;
        private readonly ResilienceService _resilienceService = resilienceService;

        public async Task<List<Rundown>> GetAllRundownsAsync()
        {
            return await _resilienceService.ExecuteWithResilienceAsync(() => _rundownRepository.GetAllAsync());
        }

        public async Task<Rundown?> GetRundownByIdAsync(Guid uuid)
        {
            return await _resilienceService.ExecuteWithResilienceAsync(() => _rundownRepository.GetByIdAsync(uuid));
        }

        public async Task CreateRundownAsync(Rundown newRundown)
        {
            await _resilienceService.ExecuteWithResilienceAsync(async () =>
            {
                var rundown = await _rundownRepository.CreateAsync(newRundown);
                var messageObject = new
                {
                    Action = "create",
                    Rundown = rundown
                };
                string message = JsonConvert.SerializeObject(messageObject);
                _kafkaService.SendMessage("rundown", message);
            });
        }

        public async Task<Rundown> UpdateRundownAsync(Guid uuid, Rundown updatedRundown)
        {
            return await _resilienceService.ExecuteWithResilienceAsync(async () =>
            {
                var rundown = await _rundownRepository.UpdateAsync(uuid, updatedRundown);
                var messageObject = new
                {
                    Action = "update",
                    Rundown = rundown
                };
                string message = JsonConvert.SerializeObject(messageObject);
                _kafkaService.SendMessage("rundown", message);
                Console.WriteLine($"MESSAGE: Rundown opdateret UUID = {rundown.UUID}");
                return rundown;
            });
        }

        public async Task DeleteRundownAsync(Guid uuid)
        {
            await _resilienceService.ExecuteWithResilienceAsync(() => _rundownRepository.DeleteAsync(uuid));
        }
    }
}
