using RundownDbService.Models;

namespace RundownDbService.BLL.Interfaces
{
    public interface IRundownItemService
    {
        Task<List<RundownItem>> GetAllRundownItemsAsync();
        Task<RundownItem> GetRundownItemByIdAsync(Guid uuid);
        Task CreateRundownItemAsync(RundownItem newItem);
        Task UpdateRundownItemAsync(Guid uuid, RundownItem updatedItem);
        Task DeleteRundownItemAsync(Guid uuid);
    }
}
