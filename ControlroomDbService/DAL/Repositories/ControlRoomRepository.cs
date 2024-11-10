using ControlRoomDbService.DAL.Interfaces;
using ControlRoomDbService.DAL;
using ControlRoomDbService.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;

namespace ControlRoomDbService.DAL.Repositories
{
    public class ControlRoomRepository : IControlRoomRepository
    {
        private readonly IMongoCollection<ControlRoom> _controlRoomCollection;

        public ControlRoomRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _controlRoomCollection = mongoDatabase.GetCollection<ControlRoom>("ControlRoom");
        }

        public async Task<List<ControlRoom>> GetAsync() => await _controlRoomCollection.Find(_ => true).ToListAsync();

        public async Task<ControlRoom?> GetByIdAsync(string id) =>
            await _controlRoomCollection.Find(x => x.UUID == Guid.Parse(id)).FirstOrDefaultAsync();

        public async Task CreateAsync(ControlRoom newControlRoom) =>
            await _controlRoomCollection.InsertOneAsync(newControlRoom);

        public async Task<List<ControlRoom>> UpdateAsync(string id, ControlRoom updatedControlRoom)
        {
            await _controlRoomCollection.ReplaceOneAsync(x => x.UUID == Guid.Parse(id), updatedControlRoom);

            // Hent alle controlrooms og returnér listen
            return await _controlRoomCollection.Find(_ => true).ToListAsync();
        }


        public async Task RemoveAsync(string id) =>
            await _controlRoomCollection.DeleteOneAsync(x => x.UUID == Guid.Parse(id));

        public async Task RemoveAllAsync()
        {
            await _controlRoomCollection.DeleteManyAsync(FilterDefinition<ControlRoom>.Empty);
        }

    }
}
