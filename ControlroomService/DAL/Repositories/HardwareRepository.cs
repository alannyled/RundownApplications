using ControlroomService.DAL.Interfaces;
using ControlroomService.DAL;
using ControlroomService.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;

namespace ControlroomService.DAL.Repositories
{
    public class HardwareRepository : IHardwareRepository
    {
        private readonly IMongoCollection<Hardware> _hardwareCollection;

        public HardwareRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _hardwareCollection = mongoDatabase.GetCollection<Hardware>("Hardware");
        }

        public async Task<List<Hardware>> GetAsync() => await _hardwareCollection.Find(_ => true).ToListAsync();

        public async Task<Hardware?> GetByIdAsync(string id) =>
            await _hardwareCollection.Find(x => x.UUID == Guid.Parse(id)).FirstOrDefaultAsync();

        public async Task CreateAsync(Hardware newHardware) =>
            await _hardwareCollection.InsertOneAsync(newHardware);

        public async Task UpdateAsync(string id, Hardware updatedHardware) =>
            await _hardwareCollection.ReplaceOneAsync(x => x.UUID == Guid.Parse(id), updatedHardware);

        public async Task RemoveAsync(string id) =>
            await _hardwareCollection.DeleteOneAsync(x => x.UUID == Guid.Parse(id));
    }
}

