//using Xunit;
//using Moq;
//using MongoDB.Driver;
//using Microsoft.Extensions.Options;
//using RundownDbService.DAL.Repositories;
//using RundownDbService.Models;
//using RundownDbService.DAL;
//using System.Threading.Tasks;
//using System;
//using System.Collections.Generic;
//using System.Threading;

//public class RundownRepositoryTests
//{
//    private readonly Mock<IMongoCollection<Rundown>> _mockRundownCollection;
//    private readonly Mock<IOptions<MongoDBSettings>> _mockMongoSettings;
//    private readonly Mock<IMongoClient> _mockMongoClient;
//    private readonly Mock<IMongoDatabase> _mockMongoDatabase;
//    private readonly RundownRepository _rundownRepository;

//    public RundownRepositoryTests()
//    {
//        _mockRundownCollection = new Mock<IMongoCollection<Rundown>>();
//        _mockMongoSettings = new Mock<IOptions<MongoDBSettings>>();
//        _mockMongoClient = new Mock<IMongoClient>();
//        _mockMongoDatabase = new Mock<IMongoDatabase>();

//        _mockMongoSettings.Setup(s => s.Value).Returns(new MongoDBSettings
//        {
//            ConnectionString = "mongodb://localhost:27017",
//            DatabaseName = "TestDatabase"
//        });

//        _mockMongoClient.Setup(c => c.GetDatabase("TestDatabase", null))
//                        .Returns(_mockMongoDatabase.Object);
//        _mockMongoDatabase.Setup(db => db.GetCollection<Rundown>("Rundowns", null))
//                          .Returns(_mockRundownCollection.Object);

//        _rundownRepository = new RundownRepository(_mockMongoSettings.Object);
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
//        Assert.Equal(2, result.Count);
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
//            .Setup(x => x.FindAsync(It.IsAny<FilterDefinition<Rundown>>(),
//                                    It.IsAny<FindOptions<Rundown>>(),
//                                    It.IsAny<CancellationToken>()))
//            .ReturnsAsync(cursor.Object);

//        // Act
//        var result = await _rundownRepository.GetByIdAsync(mockRundown.UUID);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(mockRundown.UUID, result.UUID);
//    }

//    [Fact]
//    public async Task DeleteAsync_DeletesRundown()
//    {
//        // Arrange
//        var rundownId = Guid.NewGuid();

//        _mockRundownCollection
//            .Setup(x => x.DeleteOneAsync(It.IsAny<FilterDefinition<Rundown>>(),
//                                         It.IsAny<CancellationToken>()))
//            .ReturnsAsync(DeleteResult.acknowledged(1));

//        // Act
//        await _rundownRepository.DeleteAsync(rundownId);

//        // Assert
//        _mockRundownCollection.Verify(x => x.DeleteOneAsync(It.Is<FilterDefinition<Rundown>>(f => f.ToString().Contains(rundownId.ToString())),
//                                                            It.IsAny<CancellationToken>()),
//                                      Times.Once);
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
//}

