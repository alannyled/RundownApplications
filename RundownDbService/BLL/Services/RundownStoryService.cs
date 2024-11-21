using Newtonsoft.Json;
using RundownDbService.BLL.Interfaces;
using RundownDbService.DAL.Interfaces;
using RundownDbService.Models;

namespace RundownDbService.BLL.Services
{
    public class RundownStoryService(IRundownStoryRepository rundownStoryRepository, IKafkaService kafkaService) : IRundownStoryService
    {
        private readonly IRundownStoryRepository _rundownStoryRepository = rundownStoryRepository;
        private readonly IKafkaService _kafkaService = kafkaService;

        public async Task<List<RundownStory>> GetAllRundownStoriesAsync()
        {
            return await _rundownStoryRepository.GetAllAsync();
        }

        public async Task<RundownStory> GetRundownStoryByIdAsync(Guid uuid)
        {
            return await _rundownStoryRepository.GetByIdAsync(uuid);
        }

        public async Task CreateRundownStoryAsync(RundownStory newStory)
        {
            await _rundownStoryRepository.CreateAsync(newStory);
        }

        public async Task UpdateRundownStoryAsync(Guid uuid, RundownStory updatedStory)
        {
            await _rundownStoryRepository.UpdateAsync(uuid, updatedStory);
        }

        public async Task DeleteRundownStoryAsync(Guid uuid)
        {
            await _rundownStoryRepository.DeleteAsync(uuid);
        }
    }
}
