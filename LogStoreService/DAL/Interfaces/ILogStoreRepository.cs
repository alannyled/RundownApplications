using LogStoreService.Models;

namespace LogStoreService.DAL.Interfaces
{
    public interface ILogStoreRepository
    {
        Task<List<Log>> GetAllLogsAsync();
        Task CreateAsync(Log newLog);
    }
}
