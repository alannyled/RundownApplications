using Xunit;
using Moq;
using RundownDbService.Models;
using RundownDbService.BLL.Services;
using RundownDbService.DAL.Interfaces;

public class RundownServiceTests
{
    private readonly Mock<IRundownRepository> _mockRundownRepository;
    private readonly Mock<KafkaService> _mockKafkaService;
    private readonly RundownService _rundownService;

    public RundownServiceTests()
    {
        _mockRundownRepository = new Mock<IRundownRepository>();
        _mockKafkaService = new Mock<KafkaService>();
        _rundownService = new RundownService(_mockRundownRepository.Object, _mockKafkaService.Object);
    }

    [Fact]
    public async Task GetAllRundownsAsync_ReturnsListOfRundowns()
    {
        // Arrange
        var mockRundowns = new List<Rundown>
        {
            new Rundown { UUID = Guid.NewGuid(), Name = "Rundown 1" },
            new Rundown { UUID = Guid.NewGuid(), Name = "Rundown 2" }
        };
        _mockRundownRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(mockRundowns);

        // Act
        var result = await _rundownService.GetAllRundownsAsync();

        // Assert
        Assert.Equal(2, result.Count);
        _mockRundownRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateRundownAsync_CallsRepositoryAndSendsMessage()
    {
        // Arrange
        var newRundown = new Rundown { UUID = Guid.NewGuid(), Name = "New Rundown" };
        _mockRundownRepository.Setup(repo => repo.CreateAsync(newRundown))
                              .ReturnsAsync(newRundown);

        var _rundownService = new RundownService(_mockRundownRepository.Object, _mockKafkaService.Object);

        // Act
        await _rundownService.CreateRundownAsync(newRundown);

        // Assert
        _mockRundownRepository.Verify(repo => repo.CreateAsync(newRundown), Times.Once);
        _mockKafkaService.Verify(kafka => kafka.SendMessage("rundown", It.IsAny<string>()), Times.Once);
    }


    [Fact]
    public async Task UpdateRundownAsync_CallsRepositoryAndSendsMessage()
    {
        // Arrange
        var rundownId = Guid.NewGuid();
        var updatedRundown = new Rundown { UUID = rundownId, Name = "Updated Rundown" };
        _mockRundownRepository.Setup(repo => repo.UpdateAsync(rundownId, updatedRundown)).ReturnsAsync(updatedRundown);

        // Act
        var result = await _rundownService.UpdateRundownAsync(rundownId, updatedRundown);

        // Assert
        Assert.Equal("Updated Rundown", result.Name);
        _mockRundownRepository.Verify(repo => repo.UpdateAsync(rundownId, updatedRundown), Times.Once);
        _mockKafkaService.Verify(kafka => kafka.SendMessage("rundown", It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DeleteRundownAsync_CallsRepository()
    {
        // Arrange
        var rundownId = Guid.NewGuid();

        _mockRundownRepository.Setup(repo => repo.DeleteAsync(rundownId))
                              .Returns(Task.CompletedTask);

        var service = new RundownService(_mockRundownRepository.Object, _mockKafkaService.Object);

        // Act
        await service.DeleteRundownAsync(rundownId);

        // Assert
        _mockRundownRepository.Verify(repo => repo.DeleteAsync(rundownId), Times.Once);
        // Bemærk: Ingen verifikation af _mockKafkaService her.
    }

}
