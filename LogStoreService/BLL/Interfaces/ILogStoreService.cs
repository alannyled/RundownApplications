using LogStoreService.Models;

namespace LogStoreService.BLL.Interfaces
{
    public interface ILogStoreService
    {
        Task<List<Log>> GetLogsAsync();
        Task CreateLogAsync(Log newLog);
        Task DeleteAllLogsAsync();
    }
}
