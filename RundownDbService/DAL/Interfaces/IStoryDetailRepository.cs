using RundownDbService.Models;

namespace RundownDbService.DAL.Interfaces
{
    public interface IStoryDetailRepository
    {
        Task<List<StoryDetail>> GetAllAsync();
        Task<StoryDetail> GetByIdAsync(Guid uuid);
        Task CreateAsync(StoryDetail detail);
        Task UpdateAsync(Guid uuid, StoryDetail detail);
        Task DeleteAsync(Guid uuid);
    }
}
