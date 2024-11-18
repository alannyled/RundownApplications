using MongoDB.Driver;
using Mongo2Go;
using ControlRoomDbService.DAL.Repositories;
using ControlRoomDbService.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using ControlRoomDbService.DAL;

namespace ControlRoomDbService.Tests.DAL.Repositories
{
    public class HardwareRepositoryTests : IDisposable
    {
        private readonly MongoDbRunner _runner;
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly HardwareRepository _hardwareRepository;

        public HardwareRepositoryTests()
        {
            _runner = MongoDbRunner.Start();
            _client = new MongoClient(_runner.ConnectionString);
            _database = _client.GetDatabase("ControlRoomDb");

            var mongoDBSettings = Options.Create(new MongoDBSettings
            {
                ConnectionString = _runner.ConnectionString,
                DatabaseName = "ControlRoomDb"
            });

            _hardwareRepository = new HardwareRepository(mongoDBSettings);
        }

        public void Dispose()
        {
            _runner.Dispose();
        }

        [Fact]
        public async Task GetAsync_ShouldReturnAllHardware()
        {
            // Arrange
            var collection = _database.GetCollection<Hardware>("Hardware");
            var expectedHardware = new List<Hardware>
            {
                new() { UUID = Guid.NewGuid(), Name = "Hardware 1" },
                new() { UUID = Guid.NewGuid(), Name = "Hardware 2" }
            };
            await collection.InsertManyAsync(expectedHardware);

            // Act
            var hardwareList = await _hardwareRepository.GetAsync();

            // Assert
            Assert.NotNull(hardwareList);
            Assert.Equal(expectedHardware.Count, hardwareList.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnHardware_WhenHardwareExists()
        {
            // Arrange
            var uuid = Guid.NewGuid();
            var expectedHardware = new Hardware { UUID = uuid, Name = "Hardware 1" };
            var collection = _database.GetCollection<Hardware>("Hardware");
            await collection.InsertOneAsync(expectedHardware);

            // Act
            var hardware = await _hardwareRepository.GetByIdAsync(uuid.ToString());

            // Assert
            Assert.NotNull(hardware);
            Assert.Equal(uuid, hardware.UUID);
        }

        [Fact]
        public async Task CreateAsync_ShouldInsertHardware()
        {
            // Arrange
            var newHardware = new Hardware { UUID = Guid.NewGuid(), Name = "New Hardware" };

            // Act
            await _hardwareRepository.CreateAsync(newHardware);

            // Assert
            var collection = _database.GetCollection<Hardware>("Hardware");
            var insertedHardware = await collection.Find(r => r.UUID == newHardware.UUID).FirstOrDefaultAsync();
            Assert.NotNull(insertedHardware);
            Assert.Equal(newHardware.UUID, insertedHardware.UUID);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReplaceHardware_WhenHardwareExists()
        {
            // Arrange
            var uuid = Guid.NewGuid();
            var originalHardware = new Hardware { UUID = uuid, Name = "Original Hardware" };
            var updatedHardware = new Hardware { UUID = uuid, Name = "Updated Hardware" };
            var collection = _database.GetCollection<Hardware>("Hardware");
            await collection.InsertOneAsync(originalHardware);

            // Act
            await _hardwareRepository.UpdateAsync(uuid.ToString(), updatedHardware);

            // Assert
            var replacedHardware = await collection.Find(r => r.UUID == uuid).FirstOrDefaultAsync();
            Assert.NotNull(replacedHardware);
            Assert.Equal("Updated Hardware", replacedHardware.Name);
        }

        [Fact]
        public async Task RemoveAsync_ShouldDeleteHardware_WhenHardwareExists()
        {
            // Arrange
            var uuid = Guid.NewGuid();
            var hardwareToDelete = new Hardware { UUID = uuid, Name = "Hardware to Delete" };
            var collection = _database.GetCollection<Hardware>("Hardware");
            await collection.InsertOneAsync(hardwareToDelete);

            // Act
            await _hardwareRepository.RemoveAsync(uuid.ToString());

            // Assert
            var deletedHardware = await collection.Find(r => r.UUID == uuid).FirstOrDefaultAsync();
            Assert.Null(deletedHardware);
        }
    }
}
