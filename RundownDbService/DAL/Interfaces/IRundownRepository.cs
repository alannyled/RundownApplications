using RundownDbService.Models;

namespace RundownDbService.DAL.Interfaces
{
    public interface IRundownRepository
    {
        Task<List<Rundown>> GetAllAsync();
        Task<Rundown> GetByIdAsync(Guid uuid);
        Task<Rundown> CreateAsync(Rundown rundown);
        Task<Rundown> UpdateAsync(Guid uuid, Rundown rundown);
        Task UpdateStoryAsync(Guid rundownId, RundownStory updatedStory);
        Task DeleteAsync(Guid uuid);
    }
}
