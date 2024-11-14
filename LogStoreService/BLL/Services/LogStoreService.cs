using Confluent.Kafka;
using LogStoreService.BLL.Interfaces;
using LogStoreService.DAL.Interfaces;
using LogStoreService.Models;

namespace LogStoreService.BLL.Services
{
    public class LogService(ILogStoreRepository logStoreRepository) : ILogStoreService
    {
        private readonly ILogStoreRepository _logStoreRepository = logStoreRepository;

        public async Task<List<Log>> GetLogsAsync()
        {
            var logs = await _logStoreRepository.GetAllLogsAsync();           
            return logs;
        }

        public async Task CreateLogAsync(Log newLog)
        {
            Console.WriteLine($"{newLog.TimeStamp} - {newLog.LogLevel} From {newLog.Host}: {newLog.Message}");
            await _logStoreRepository.CreateAsync(newLog);
        }

        public async Task DeleteAllLogsAsync()
        {
            await _logStoreRepository.DeleteAllLogsAsync();
        }
    }
}
