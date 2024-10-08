using MongoDB.Driver;
using TemplateDbService.Models;

namespace TemplateDbService.BLL.Services
{
    public class ItemTemplateService
    {
        private readonly IMongoCollection<ItemTemplate> _itemTemplates;

        public ItemTemplateService(IMongoDatabase mongoDatabase)
        {
            _itemTemplates = mongoDatabase.GetCollection<ItemTemplate>("ItemTemplates");
        }

        public async Task<List<ItemTemplate>> GetAllAsync() =>
            await _itemTemplates.Find(_ => true).ToListAsync();

        public async Task<ItemTemplate> GetByIdAsync(Guid uuid) =>
            await _itemTemplates.Find(x => x.UUID == uuid).FirstOrDefaultAsync();

        public async Task CreateAsync(ItemTemplate itemTemplate) =>
            await _itemTemplates.InsertOneAsync(itemTemplate);

        public async Task UpdateAsync(Guid uuid, ItemTemplate itemTemplate) =>
            await _itemTemplates.ReplaceOneAsync(x => x.UUID == uuid, itemTemplate);

        public async Task DeleteAsync(Guid uuid) =>
            await _itemTemplates.DeleteOneAsync(x => x.UUID == uuid);
    }
}

