using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RundownDbService.DAL.Interfaces;
using RundownDbService.Models;

namespace RundownDbService.DAL.Repositories
{
    public class RundownRepository : IRundownRepository
    {
        private readonly IMongoCollection<Rundown> _rundownCollection;

        public RundownRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _rundownCollection = mongoDatabase.GetCollection<Rundown>("Rundowns");
        }

        public async Task<List<Rundown>> GetAllAsync()
        {
            return await _rundownCollection.Find(rundown => true).ToListAsync();
        }

        public async Task<Rundown> GetByIdAsync(Guid uuid)
        {
            return await _rundownCollection.Find(rundown => rundown.UUID == uuid).FirstOrDefaultAsync();
        }

        public async Task<Rundown> CreateAsync(Rundown newRundown)
        {
            await _rundownCollection.InsertOneAsync(newRundown);
            return newRundown;
        }

        public async Task<Rundown> UpdateAsync(Guid uuid, Rundown updatedRundown)
        {
            await _rundownCollection.ReplaceOneAsync(rundown => rundown.UUID == uuid, updatedRundown);
            var rundown = await GetByIdAsync(uuid);
            return rundown;
        }

        public async Task UpdateStoryAsync(Guid rundownId, RundownStory updatedStory)
        {
            var filter = Builders<Rundown>.Filter.And(
                Builders<Rundown>.Filter.Eq(r => r.UUID, rundownId),
                Builders<Rundown>.Filter.ElemMatch(r => r.Stories, i => i.UUID == updatedStory.UUID)
            );

            var update = Builders<Rundown>.Update.Set("Stories.$", updatedStory);  // Opdaterer hele story

            await _rundownCollection.UpdateOneAsync(filter, update);
        }
        public async Task DeleteAsync(Guid uuid)
        {
            await _rundownCollection.DeleteOneAsync(rundown => rundown.UUID == uuid);
        }
    }
}
