using Xunit;
using MongoDB.Driver;
using Mongo2Go;
using ControlRoomDbService.DAL;
using ControlRoomDbService.DAL.Repositories;
using ControlRoomDbService.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Text;

namespace ControlRoomDbService.Tests.DAL.Repositories
{
    public class ControlRoomRepositoryTests : IDisposable
    {
        private readonly MongoDbRunner _runner;
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly ControlRoomRepository _controlRoomRepository;

        public ControlRoomRepositoryTests()
        {
            _runner = MongoDbRunner.Start();
            _client = new MongoClient(_runner.ConnectionString);
            _database = _client.GetDatabase("ControlRoomDb");

            var mongoDBSettings = Options.Create(new MongoDBSettings
            {
                ConnectionString = _runner.ConnectionString,
                DatabaseName = "ControlRoomDb"
            });

            _controlRoomRepository = new ControlRoomRepository(mongoDBSettings);
        }

        public void Dispose()
        {
            _runner.Dispose();
        }

        [Fact]
        public async Task GetAsync_ShouldReturnAllControlRooms()
        {
            // Arrange
            var collection = _database.GetCollection<ControlRoom>("ControlRoom");
            var expectedControlRooms = new List<ControlRoom>
            {
                new ControlRoom { UUID = Guid.NewGuid(), Name = "Control Room 1" },
                new ControlRoom { UUID = Guid.NewGuid(), Name = "Control Room 2" }
            };
            await collection.InsertManyAsync(expectedControlRooms);

            // Act
            var controlRooms = await _controlRoomRepository.GetAsync();

            // Assert
            Assert.NotNull(controlRooms);
            Assert.Equal(expectedControlRooms.Count, controlRooms.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnControlRoom_WhenControlRoomExists()
        {
            // Arrange
            var uuid = Guid.NewGuid();
            var expectedControlRoom = new ControlRoom { UUID = uuid, Name = "Control Room 1" };
            var collection = _database.GetCollection<ControlRoom>("ControlRoom");
            await collection.InsertOneAsync(expectedControlRoom);

            // Act
            var controlRoom = await _controlRoomRepository.GetByIdAsync(uuid.ToString());

            // Assert
            Assert.NotNull(controlRoom);
            Assert.Equal(uuid, controlRoom.UUID);
        }

        [Fact]
        public async Task CreateAsync_ShouldInsertControlRoom()
        {
            // Arrange
            var newControlRoom = new ControlRoom { UUID = Guid.NewGuid(), Name = "New Control Room" };

            // Act
            await _controlRoomRepository.CreateAsync(newControlRoom);

            // Assert
            var collection = _database.GetCollection<ControlRoom>("ControlRoom");
            var insertedControlRoom = await collection.Find(c => c.UUID == newControlRoom.UUID).FirstOrDefaultAsync();
            Assert.NotNull(insertedControlRoom);
            Assert.Equal(newControlRoom.UUID, insertedControlRoom.UUID);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReplaceControlRoom_WhenControlRoomExists()
        {
            // Arrange
            var uuid = Guid.NewGuid();
            var originalControlRoom = new ControlRoom { UUID = uuid, Name = "Original Control Room" };
            var updatedControlRoom = new ControlRoom { UUID = uuid, Name = "Updated Control Room" };
            var collection = _database.GetCollection<ControlRoom>("ControlRoom");
            await collection.InsertOneAsync(originalControlRoom);

            // Act
            var result = await _controlRoomRepository.UpdateAsync(uuid.ToString(), updatedControlRoom);

            // Assert
            var replacedControlRoom = await collection.Find(c => c.UUID == uuid).FirstOrDefaultAsync();
            Assert.NotNull(replacedControlRoom);
            Assert.Equal("Updated Control Room", replacedControlRoom.Name);
        }

        [Fact]
        public async Task RemoveAsync_ShouldDeleteControlRoom_WhenControlRoomExists()
        {
            // Arrange
            var uuid = Guid.NewGuid();
            var controlRoomToDelete = new ControlRoom { UUID = uuid, Name = "Control Room to Delete" };
            var collection = _database.GetCollection<ControlRoom>("ControlRoom");
            await collection.InsertOneAsync(controlRoomToDelete);

            // Act
            await _controlRoomRepository.RemoveAsync(uuid.ToString());

            // Assert
            var deletedControlRoom = await collection.Find(c => c.UUID == uuid).FirstOrDefaultAsync();
            Assert.Null(deletedControlRoom);
        }

        [Fact]
        public async Task RemoveAllAsync_ShouldDeleteAllControlRooms()
        {
            // Arrange
            var collection = _database.GetCollection<ControlRoom>("ControlRoom");
            var controlRooms = new List<ControlRoom>
            {
                new ControlRoom { UUID = Guid.NewGuid(), Name = "Control Room 1" },
                new ControlRoom { UUID = Guid.NewGuid(), Name = "Control Room 2" }
            };
            await collection.InsertManyAsync(controlRooms);

            // Act
            await _controlRoomRepository.RemoveAllAsync();

            // Assert
            var remainingControlRooms = await collection.Find(_ => true).ToListAsync();
            Assert.Empty(remainingControlRooms);
        }
    }
}
