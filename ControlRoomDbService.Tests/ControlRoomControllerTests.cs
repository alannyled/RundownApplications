using Xunit;
using Moq;
using RundownDbService.Models;
using RundownDbService.BLL.Services;
using RundownDbService.BLL.Interfaces;
using CommonClassLibrary.Services;
using Microsoft.Extensions.Logging.Abstractions; // Til NullLogger
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RundownDbService.DAL.Interfaces;

public class RundownServiceTests
{
    private readonly Mock<IRundownRepository> _mockRundownRepository;
    private readonly Mock<IKafkaService> _mockKafkaService;
    private readonly Mock<IResilienceService> _mockResilienceService;
    private readonly RundownService _rundownService;

    public RundownServiceTests()
    {
        _mockRundownRepository = new Mock<IRundownRepository>();
        _mockKafkaService = new Mock<IKafkaService>();
        _mockResilienceService = new Mock<IResilienceService>();

        // Brug en NullLogger for at undgå logging
        var nullLogger = NullLogger<RundownService>.Instance;

        _rundownService = new RundownService(
            _mockRundownRepository.Object,
            _mockKafkaService.Object,
            _mockResilienceService.Object,
            nullLogger);
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

        _mockResilienceService
            .Setup(r => r.ExecuteWithResilienceAsync(It.IsAny<Func<Task<List<Rundown>>>>()))
            .ReturnsAsync(mockRundowns);

        // Act
        var result = await _rundownService.GetAllRundownsAsync();

        // Assert
        Assert.Equal(2, result.Count);
        _mockResilienceService.Verify(r => r.ExecuteWithResilienceAsync(It.IsAny<Func<Task<List<Rundown>>>>()), Times.Once);
    }

    [Fact]
    public async Task CreateRundownAsync_CallsRepositoryAndSendsMessage()
    {
        // Arrange
        var newRundown = new Rundown { UUID = Guid.NewGuid(), Name = "New Rundown" };

        _mockResilienceService
            .Setup(r => r.ExecuteWithResilienceAsync(It.IsAny<Func<Task>>()))
            .Returns(async (Func<Task> action) => await action());

        _mockRundownRepository
            .Setup(repo => repo.CreateAsync(newRundown))
            .ReturnsAsync(newRundown);

        // Act
        await _rundownService.CreateRundownAsync(newRundown);

        // Assert
        _mockRundownRepository.Verify(repo => repo.CreateAsync(newRundown), Times.Once);
        _mockKafkaService.Verify(kafka => kafka.SendMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateRundownAsync_CallsRepositoryAndSendsMessage()
    {
        // Arrange
        var rundownId = Guid.NewGuid();
        var updatedRundown = new Rundown { UUID = rundownId, Name = "Updated Rundown" };

        _mockResilienceService
            .Setup(r => r.ExecuteWithResilienceAsync(It.IsAny<Func<Task<Rundown>>>()))
            .Returns(async (Func<Task<Rundown>> action) => await action());

        _mockRundownRepository
            .Setup(repo => repo.UpdateAsync(rundownId, updatedRundown))
            .ReturnsAsync(updatedRundown);

        // Act
        var result = await _rundownService.UpdateRundownAsync(rundownId, updatedRundown);

        // Assert
        Assert.Equal("Updated Rundown", result.Name);
        _mockRundownRepository.Verify(repo => repo.UpdateAsync(rundownId, updatedRundown), Times.Once);
        _mockKafkaService.Verify(kafka => kafka.SendMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DeleteRundownAsync_CallsRepository()
    {
        // Arrange
        var rundownId = Guid.NewGuid();

        _mockResilienceService
            .Setup(r => r.ExecuteWithResilienceAsync(It.IsAny<Func<Task>>()))
            .Returns(async (Func<Task> action) => await action());

        _mockRundownRepository
            .Setup(repo => repo.DeleteAsync(rundownId))
            .Returns(Task.CompletedTask);

        // Act
        await _rundownService.DeleteRundownAsync(rundownId);

        // Assert
        _mockRundownRepository.Verify(repo => repo.DeleteAsync(rundownId), Times.Once);
    }
}
