using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RundownDbService.DAL.Interfaces;
using RundownDbService.Models;

namespace RundownDbService.DAL.Repositories
{
    public class ItemDetailRepository : IItemDetailRepository
    {
        private readonly IMongoCollection<ItemDetail> _itemDetails;

        public ItemDetailRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _itemDetails = mongoDatabase.GetCollection<ItemDetail>("ItemDetails");
        }

        public async Task<List<ItemDetail>> GetAllAsync()
        {
            return await _itemDetails.Find(detail => true).ToListAsync();
        }

        public async Task<ItemDetail> GetByIdAsync(Guid uuid)
        {
            return await _itemDetails.Find(detail => detail.UUID == uuid).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(ItemDetail detail)
        {
            await _itemDetails.InsertOneAsync(detail);
        }

        public async Task UpdateAsync(Guid uuid, ItemDetail detail)
        {
            await _itemDetails.ReplaceOneAsync(d => d.UUID == uuid, detail);
        }

        public async Task DeleteAsync(Guid uuid)
        {
            await _itemDetails.DeleteOneAsync(d => d.UUID == uuid);
        }
    }
}
