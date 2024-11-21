using RundownDbService.Models;

namespace RundownDbService.BLL.Interfaces
{
    public interface IRundownStoryService
    {
        Task<List<RundownStory>> GetAllRundownStoriesAsync();
        //Task<List<RundownStory>> GetRundownStoryAsync(Guid rundownUuid);
        Task<RundownStory> GetRundownStoryByIdAsync(Guid uuid);
        Task CreateRundownStoryAsync(RundownStory newStory);
        Task UpdateRundownStoryAsync(Guid uuid, RundownStory updatedStory);
        Task DeleteRundownStoryAsync(Guid uuid);
    }
}
