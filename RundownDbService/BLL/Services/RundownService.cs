using RundownDbService.BLL.Interfaces;
using RundownDbService.DAL.Interfaces;
using RundownDbService.Models;
using Newtonsoft.Json;
using CommonClassLibrary.Services;
using CommonClassLibrary.Enum;

namespace RundownDbService.BLL.Services
{
    public class RundownService(IRundownRepository rundownRepository, IKafkaService kafkaService, ResilienceService resilienceService, ILogger<RundownService> logger) : IRundownService
    {
        private readonly IRundownRepository _rundownRepository = rundownRepository;
        private readonly IKafkaService _kafkaService = kafkaService;
        private readonly ResilienceService _resilienceService = resilienceService;
        private readonly ILogger<RundownService> _logger = logger;

        public async Task<List<Rundown>> GetAllRundownsAsync()
        {  
            var rundown = await _resilienceService.ExecuteWithResilienceAsync(() => _rundownRepository.GetAllAsync());
            
            if(rundown == null)
            {
                _logger.LogWarning("Der blev ikke fundet nogen rundowns i databasen");
                return [];
            }
            _logger.LogInformation("Alle rundowns er hentet fra databasen");
            return rundown;            
        }

        public async Task<Rundown?> GetRundownByIdAsync(Guid uuid)
        {
            var rundown = await _resilienceService.ExecuteWithResilienceAsync(() => _rundownRepository.GetByIdAsync(uuid));
            if (rundown == null)
            {
                _logger.LogWarning($"Rundown med UUID = {uuid} blev ikke fundet i databasen");
            }
            _logger.LogInformation($"{rundown?.Name} {rundown?.BroadcastDate.ToShortDateString()} er hentet i databasen");
            return rundown;
        }

        public async Task CreateRundownAsync(Rundown newRundown)
        {
            await _resilienceService.ExecuteWithResilienceAsync(async () =>
            {
                var rundown = await _rundownRepository.CreateAsync(newRundown);
                var messageObject = new
                {
                    Action = MessageAction.Create.ToString(),
                    Rundown = rundown
                };
                string message = JsonConvert.SerializeObject(messageObject);
                string topic = MessageTopic.Rundown.ToKafkaTopic();
                _kafkaService.SendMessage(topic, message);
                _logger.LogInformation($"{rundown.Name} {rundown.BroadcastDate.ToShortDateString()} er oprettet");
            });
        }

        public async Task<Rundown> UpdateRundownAsync(Guid uuid, Rundown updatedRundown)
        {  
            return await _resilienceService.ExecuteWithResilienceAsync(async () =>
            {
                var rundown = await _rundownRepository.UpdateAsync(uuid, updatedRundown);
                if (rundown == null)
                {
                    _logger.LogWarning($"Rundown med UUID = {uuid} blev ikke fundet i databasen");
                    return new Rundown(); 
                }
                var messageObject = new
                {
                    Action = MessageAction.Update.ToString(),
                    Rundown = rundown
                };
                string message = JsonConvert.SerializeObject(messageObject);
                string topic = MessageTopic.Rundown.ToKafkaTopic();
                _kafkaService.SendMessage(topic, message);
                _logger.LogInformation($"{rundown.Name} {rundown.BroadcastDate.ToShortDateString()} er opdateret");
                return rundown;
            }) ?? new Rundown(); 
        }

        public async Task DeleteRundownAsync(Guid uuid)
        {
            _logger.LogWarning($"Rundown med UUID = {uuid} slettes fra databasen");
            await _resilienceService.ExecuteWithResilienceAsync(() => _rundownRepository.DeleteAsync(uuid));
        }
    }
}
