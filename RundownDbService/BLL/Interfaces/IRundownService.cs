using RundownDbService.Models;

namespace RundownDbService.BLL.Interfaces
{
    public interface IRundownService
    {
        Task<List<Rundown>> GetAllRundownsAsync();
        Task<Rundown> GetRundownByIdAsync(Guid uuid);
        Task CreateRundownAsync(Rundown newRundown);
        Task UpdateRundownAsync(Guid uuid, Rundown updatedRundown);
        Task DeleteRundownAsync(Guid uuid);
    }
}
