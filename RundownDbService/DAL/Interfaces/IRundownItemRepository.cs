using RundownDbService.Models;

namespace RundownDbService.DAL.Interfaces
{
    public interface IRundownItemRepository
    {
        Task<List<RundownItem>> GetAllAsync();
        Task<RundownItem> GetByIdAsync(Guid uuid);
        Task CreateAsync(RundownItem item);
        Task UpdateAsync(Guid uuid, RundownItem item);
        Task DeleteAsync(Guid uuid);
    }
}
