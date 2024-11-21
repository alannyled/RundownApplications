using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RundownDbService.DAL.Interfaces;
using RundownDbService.Models;

namespace RundownDbService.DAL.Repositories
{
    public class RundownStoryRepository : IRundownStoryRepository
    {
        private readonly IMongoCollection<RundownStory> _rundownStoryCollection;

        public RundownStoryRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _rundownStoryCollection = mongoDatabase.GetCollection<RundownStory>("RundownStorys");
        }

        public async Task<List<RundownStory>> GetAllAsync()
        {
            return await _rundownStoryCollection.Find(story => true).ToListAsync();
        }

        public async Task<List<RundownStory>> GetByRundownUuidAsync(Guid uuid)
        {
            return await _rundownStoryCollection.Find(story => story.UUID == uuid).ToListAsync();
        }

        public async Task<RundownStory> GetByIdAsync(Guid uuid)
        {
            return await _rundownStoryCollection.Find(story => story.UUID == uuid).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(RundownStory story)
        {
            await _rundownStoryCollection.InsertOneAsync(story);
        }

        public async Task UpdateAsync(Guid uuid, RundownStory story)
        {
            await _rundownStoryCollection.ReplaceOneAsync(i => i.UUID == uuid, story);
           
        }

        public async Task DeleteAsync(Guid uuid)
        {
            await _rundownStoryCollection.DeleteOneAsync(i => i.UUID == uuid);
        }
    }
}
