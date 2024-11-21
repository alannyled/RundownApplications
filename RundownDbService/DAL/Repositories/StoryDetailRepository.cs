using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RundownDbService.DAL.Interfaces;
using RundownDbService.Models;

namespace RundownDbService.DAL.Repositories
{
    public class StoryDetailRepository : IStoryDetailRepository
    {
        private readonly IMongoCollection<StoryDetail> _storyDetails;

        public StoryDetailRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _storyDetails = mongoDatabase.GetCollection<StoryDetail>("StoryDetails");
        }

        public async Task<List<StoryDetail>> GetAllAsync()
        {
            return await _storyDetails.Find(detail => true).ToListAsync();
        }

        public async Task<StoryDetail> GetByIdAsync(Guid uuid)
        {
            return await _storyDetails.Find(detail => detail.UUID == uuid).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(StoryDetail detail)
        {
            await _storyDetails.InsertOneAsync(detail);
        }

        public async Task UpdateAsync(Guid uuid, StoryDetail detail)
        {
            await _storyDetails.ReplaceOneAsync(d => d.UUID == uuid, detail);
        }

        public async Task DeleteAsync(Guid uuid)
        {
            await _storyDetails.DeleteOneAsync(d => d.UUID == uuid);
        }
    }
}
