//using Xunit;
//using Moq;
//using MongoDB.Driver;
//using RundownDbService.DAL.Repositories;
//using RundownDbService.Models;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;

//namespace RundownDbService.DAL
//{
//    public class MongoDBSettings
//    {
//        public string ConnectionString { get; set; } = string.Empty;
//        public string DatabaseName { get; set; } = string.Empty;
//    }
//}

//public class RundownRepositoryTests
//{
//    private readonly Mock<IMongoCollection<Rundown>> _mockRundownCollection;
//    private readonly Mock<IMongoDatabase> _mockMongoDatabase;
//    private readonly Mock<IOptions<RundownDbService.DAL.MongoDBSettings>> _mockMongoDBSettings;
//    private readonly RundownRepository _rundownRepository;

//    public RundownRepositoryTests()
//    {
//        _mockRundownCollection = new Mock<IMongoCollection<Rundown>>();
//        _mockMongoDatabase = new Mock<IMongoDatabase>();
//        _mockMongoDBSettings = new Mock<IOptions<RundownDbService.DAL.MongoDBSettings>>();

//        _mockMongoDatabase.Setup(db => db.GetCollection<Rundown>("Rundowns", null))
//                          .Returns(_mockRundownCollection.Object);

//        var mongoSettings = new RundownDbService.DAL.MongoDBSettings
//        {
//            ConnectionString = "mongodb://localhost:27017",
//            DatabaseName = "RundownDb"
//        };
//        _mockMongoDBSettings.Setup(s => s.Value).Returns(mongoSettings);

//        _rundownRepository = new RundownRepository(_mockMongoDBSettings.Object);
//    }

//    [Fact]
//    public async Task GetAllAsync_ReturnsListOfRundowns()
//    {
//        // Arrange
//        var mockRundowns = new List<Rundown> { new Rundown(), new Rundown() };
//        var cursor = new Mock<IAsyncCursor<Rundown>>();
//        cursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
//              .Returns(true)
//              .Returns(false);
//        cursor.Setup(_ => _.Current).Returns(mockRundowns);

//        _mockRundownCollection
//            .Setup(x => x.FindAsync(It.IsAny<FilterDefinition<Rundown>>(),
//                                    It.IsAny<FindOptions<Rundown>>(),
//                                    It.IsAny<CancellationToken>()))
//            .ReturnsAsync(cursor.Object);

//        // Act
//        var result = await _rundownRepository.GetAllAsync();

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(mockRundowns.Count, result.Count);
//    }

//    [Fact]
//    public async Task GetByIdAsync_ReturnsRundown()
//    {
//        // Arrange
//        var mockRundown = new Rundown { UUID = Guid.NewGuid() };
//        var cursor = new Mock<IAsyncCursor<Rundown>>();
//        cursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
//              .Returns(true)
//              .Returns(false);
//        cursor.Setup(_ => _.Current).Returns(new List<Rundown> { mockRundown });

//        _mockRundownCollection
//            .Setup(x => x.FindSync(It.IsAny<FilterDefinition<Rundown>>(),
//                                    It.IsAny<FindOptions<Rundown>>(),
//                                    It.IsAny<CancellationToken>()))
//            .Returns(cursor.Object);

//        // Act
//        var result = await _rundownRepository.GetByIdAsync(mockRundown.UUID);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(mockRundown.UUID, result.UUID);
//    }

//    [Fact]
//    public async Task CreateAsync_AddsNewRundown()
//    {
//        // Arrange
//        var newRundown = new Rundown { UUID = Guid.NewGuid() };

//        // Act
//        await _rundownRepository.CreateAsync(newRundown);

//        // Assert
//        _mockRundownCollection.Verify(x => x.InsertOneAsync(It.Is<Rundown>(r => r.UUID == newRundown.UUID),
//                                                            It.IsAny<InsertOneOptions>(),
//                                                            It.IsAny<CancellationToken>()),
//                                      Times.Once);
//    }

//    [Fact]
//    public async Task UpdateAsync_UpdatesRundown()
//    {
//        // Arrange
//        var rundownId = Guid.NewGuid();
//        var updatedRundown = new Rundown { UUID = rundownId, ControlRoomId = Guid.NewGuid() };

//        // Act
//        await _rundownRepository.UpdateAsync(rundownId, updatedRundown);

//        // Assert
//        _mockRundownCollection.Verify(x => x.ReplaceOneAsync(
//            It.Is<FilterDefinition<Rundown>>(f => f != null),
//            updatedRundown,
//            It.IsAny<ReplaceOptions>(),
//            It.IsAny<CancellationToken>()),
//            Times.Once);
//    }
//}
