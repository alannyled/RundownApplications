using LogStoreService.DAL.Interfaces;
using LogStoreService.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LogStoreService.DAL.Repositories
{
    public class LogStoreRepository: ILogStoreRepository
    {
        private readonly IMongoCollection<Log> _logCollection;

        public LogStoreRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _logCollection = mongoDatabase.GetCollection<Log>("logs");
        }

        public async Task<List<Log>> GetAllLogsAsync()
        {
            return await _logCollection.Find(log => true).ToListAsync();
        }

        public async Task CreateAsync(Log newLog)
        {
            await _logCollection.InsertOneAsync(newLog);
        }
    }
}
