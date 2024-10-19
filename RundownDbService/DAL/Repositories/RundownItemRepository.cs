using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RundownDbService.DAL.Interfaces;
using RundownDbService.Models;

namespace RundownDbService.DAL.Repositories
{
    public class RundownItemRepository : IRundownItemRepository
    {
        private readonly IMongoCollection<RundownItem> _rundownItemCollection;

        public RundownItemRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _rundownItemCollection = mongoDatabase.GetCollection<RundownItem>("RundownItems");
        }

        public async Task<List<RundownItem>> GetAllAsync()
        {
            return await _rundownItemCollection.Find(item => true).ToListAsync();
        }

        public async Task<List<RundownItem>> GetByRundownUuidAsync(Guid uuid)
        {
            return await _rundownItemCollection.Find(item => item.UUID == uuid).ToListAsync();
        }

        public async Task<RundownItem> GetByIdAsync(Guid uuid)
        {
            return await _rundownItemCollection.Find(item => item.UUID == uuid).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(RundownItem item)
        {
            await _rundownItemCollection.InsertOneAsync(item);
        }

        public async Task UpdateAsync(Guid uuid, RundownItem item)
        {
            await _rundownItemCollection.ReplaceOneAsync(i => i.UUID == uuid, item);
           
        }

        public async Task DeleteAsync(Guid uuid)
        {
            await _rundownItemCollection.DeleteOneAsync(i => i.UUID == uuid);
        }
    }
}
