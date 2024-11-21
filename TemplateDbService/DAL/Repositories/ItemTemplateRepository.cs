using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TemplateDbService.DAL.Interfaces;
using TemplateDbService.Models;

namespace TemplateDbService.DAL.Repositories
{
    public class ItemTemplateRepository : IItemTemplateRepository
    {
        private readonly IMongoCollection<StoryTemplate> _itemTemplates;

        public ItemTemplateRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _itemTemplates = mongoDatabase.GetCollection<StoryTemplate>("ItemTemplates");
        }

        public async Task<List<StoryTemplate>> GetAllAsync() =>
            await _itemTemplates.Find(_ => true).ToListAsync();

        public async Task<StoryTemplate> GetByIdAsync(Guid uuid) =>
            await _itemTemplates.Find(x => x.UUID == uuid).FirstOrDefaultAsync();

        public async Task CreateAsync(StoryTemplate itemTemplate) =>
            await _itemTemplates.InsertOneAsync(itemTemplate);

        public async Task UpdateAsync(Guid uuid, StoryTemplate itemTemplate) =>
            await _itemTemplates.ReplaceOneAsync(x => x.UUID == uuid, itemTemplate);

        public async Task DeleteAsync(Guid uuid) =>
            await _itemTemplates.DeleteOneAsync(x => x.UUID == uuid);
    }
}

