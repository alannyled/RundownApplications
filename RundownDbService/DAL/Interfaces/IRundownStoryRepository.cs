using RundownDbService.Models;

namespace RundownDbService.DAL.Interfaces
{
    public interface IRundownStoryRepository
    {
        Task<List<RundownStory>> GetAllAsync();
        Task<List<RundownStory>> GetByRundownUuidAsync(Guid uuid);
        Task<RundownStory> GetByIdAsync(Guid uuid);
        Task CreateAsync(RundownStory story);
        Task UpdateAsync(Guid uuid, RundownStory story);
        Task DeleteAsync(Guid uuid);
    }
}
