using RundownDbService.BLL.Interfaces;
using RundownDbService.DAL.Interfaces;
using RundownDbService.Models;
using Newtonsoft.Json;

namespace RundownDbService.BLL.Services
{
    public class RundownService(IRundownRepository rundownRepository, IKafkaService kafkaService) : IRundownService
    {
        private readonly IRundownRepository _rundownRepository = rundownRepository;
        private readonly IKafkaService _kafkaService = kafkaService;

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
            var rundown = await _rundownRepository.CreateAsync(newRundown);
            var messageObject = new
            {
                Action = "create",
                Rundown = rundown
            };
            string message = JsonConvert.SerializeObject(messageObject);

            _kafkaService.SendMessage("rundown", message);
        }

        public async Task<Rundown> UpdateRundownAsync(Guid uuid, Rundown updatedRundown)
        {
            var rundown = await _rundownRepository.UpdateAsync(uuid, updatedRundown);
            var messageObject = new
            {
                Action = "update",
                Rundown = rundown
            };
            string message = JsonConvert.SerializeObject(messageObject);

            _kafkaService.SendMessage("rundown", message);
            return rundown;
        }

        public async Task DeleteRundownAsync(Guid uuid)
        {
            await _rundownRepository.DeleteAsync(uuid);
        }
    }
}
