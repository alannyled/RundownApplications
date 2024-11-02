using MongoDB.Driver;
using Mongo2Go;
using RundownDbService.DAL;
using RundownDbService.DAL.Repositories;
using RundownDbService.Models;
using Microsoft.Extensions.Options;

namespace RundownDbService.Tests.DAL.Repositories
{
    public class RundownRepositoryTests : IDisposable
    {
        private readonly MongoDbRunner _runner;
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly RundownRepository _rundownRepository;

        public RundownRepositoryTests()
        {
            _runner = MongoDbRunner.Start();
            _client = new MongoClient(_runner.ConnectionString);
            _database = _client.GetDatabase("RundownDb");

            var mongoDBSettings = Options.Create(new MongoDBSettings
            {
                ConnectionString = _runner.ConnectionString,
                DatabaseName = "RundownDb"
            });

            _rundownRepository = new RundownRepository(mongoDBSettings);
        }

        public void Dispose()
        {
            _runner.Dispose();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllRundowns()
        {
            // Arrange
            var collection = _database.GetCollection<Rundown>("Rundowns");
            var expectedRundowns = new List<Rundown> { new() { UUID = Guid.NewGuid() }, new() { UUID = Guid.NewGuid() } };
            await collection.InsertManyAsync(expectedRundowns);

            // Act
            var rundowns = await _rundownRepository.GetAllAsync();

            // Assert
            Assert.NotNull(rundowns);
            Assert.Equal(expectedRundowns.Count, rundowns.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnRundown_WhenRundownExists()
        {
            // Arrange
            var uuid = Guid.NewGuid();
            var expectedRundown = new Rundown { UUID = uuid };
            var collection = _database.GetCollection<Rundown>("Rundowns");
            await collection.InsertOneAsync(expectedRundown);

            // Act
            var rundown = await _rundownRepository.GetByIdAsync(uuid);

            // Assert
            Assert.NotNull(rundown);
            Assert.Equal(uuid, rundown.UUID);
        }

        [Fact]
        public async Task CreateAsync_ShouldInsertRundown()
        {
            // Arrange
            var newRundown = new Rundown { UUID = Guid.NewGuid() };

            // Act
            var result = await _rundownRepository.CreateAsync(newRundown);

            // Assert
            var collection = _database.GetCollection<Rundown>("Rundowns");
            var insertedRundown = await collection.Find(r => r.UUID == newRundown.UUID).FirstOrDefaultAsync();
            Assert.NotNull(insertedRundown);
            Assert.Equal(newRundown.UUID, insertedRundown.UUID);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReplaceRundown_WhenRundownExists()
        {
            // Arrange
            var uuid = Guid.NewGuid();
            var originalRundown = new Rundown { UUID = uuid, Name = "Original" };
            var updatedRundown = new Rundown { UUID = uuid, Name = "Updated" };
            var collection = _database.GetCollection<Rundown>("Rundowns");
            await collection.InsertOneAsync(originalRundown);

            // Act
            var result = await _rundownRepository.UpdateAsync(uuid, updatedRundown);

            // Assert
            var replacedRundown = await collection.Find(r => r.UUID == uuid).FirstOrDefaultAsync();
            Assert.NotNull(replacedRundown);
            Assert.Equal("Updated", replacedRundown.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteRundown_WhenRundownExists()
        {
            // Arrange
            var uuid = Guid.NewGuid();
            var rundownToDelete = new Rundown { UUID = uuid };
            var collection = _database.GetCollection<Rundown>("Rundowns");
            await collection.InsertOneAsync(rundownToDelete);

            // Act
            await _rundownRepository.DeleteAsync(uuid);

            // Assert
            var deletedRundown = await collection.Find(r => r.UUID == uuid).FirstOrDefaultAsync();
            Assert.Null(deletedRundown);
        }
    }
}
