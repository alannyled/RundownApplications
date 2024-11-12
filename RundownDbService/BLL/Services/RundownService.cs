using RundownDbService.BLL.Interfaces;
using RundownDbService.DAL.Interfaces;
using RundownDbService.Models;
using Newtonsoft.Json;
using CommonClassLibrary.Services;
using LogStoreService.Services;

namespace RundownDbService.BLL.Services
{
    public class RundownService : IRundownService
    {
        private readonly IRundownRepository _rundownRepository;
        private readonly IKafkaService _kafkaService;
        private readonly ResilienceService _resilienceService;
        private readonly ILogger _logger;

        public RundownService(IRundownRepository rundownRepository, IKafkaService kafkaService, ResilienceService resilienceService)
        {
            _rundownRepository = rundownRepository;
            _kafkaService = kafkaService;
            _resilienceService = resilienceService;
            _logger = CustomLoggerFactory.CreateLogger(nameof(RundownService), (message, topic) => { _kafkaService.SendMessage(topic, message); return true; });
        }

        public async Task<List<Rundown>> GetAllRundownsAsync()
        {
            _kafkaService.SendMessage("log", "Old school En opgave blev udført");
            _logger.LogInformation("En opgave blev udført");
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
