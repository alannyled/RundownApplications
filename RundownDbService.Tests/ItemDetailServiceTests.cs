using Xunit;
using Moq;
using RundownDbService.BLL.Interfaces;
using RundownDbService.DAL.Interfaces;
using RundownDbService.BLL.Services;
using RundownDbService.Models;
using CommonClassLibrary.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RundownDbService.Tests.Services
{
    public class StoryDetailServiceTests
    {
        private readonly Mock<IRundownRepository> _mockRundownRepository;
        private readonly Mock<IKafkaService> _mockKafkaService;
        private readonly StoryDetailService _storyDetailService;

        public StoryDetailServiceTests()
        {
            _mockRundownRepository = new Mock<IRundownRepository>();
            _mockKafkaService = new Mock<IKafkaService>();
            _storyDetailService = new StoryDetailService(_mockRundownRepository.Object, _mockKafkaService.Object);
        }

        [Theory]
        [InlineData("Video", typeof(StoryDetailVideo))]
        [InlineData("Teleprompter", typeof(StoryDetailTeleprompter))]
        [InlineData("Grafik", typeof(StoryDetailGraphic))]
        [InlineData("Kommentar", typeof(StoryDetailComment))]
        [InlineData("Voiceover", typeof(StoryDetailTeleprompter))]
        [InlineData("Unknown", typeof(StoryDetail))]
        public void GetModel_ShouldReturnCorrectStoryDetailModel(string type, Type expectedType)
        {
            // Act
            var result = _storyDetailService.GetModel(type);

            // Assert
            Assert.NotNull(result);
            Assert.IsType(expectedType, result);
        }

        [Fact]
        public async Task CreateStoryDetailAsync_WithValidRundownAndStory_UpdatesRundownAndSendsMessage()
        {
            // Arrange
            var rundown = new Rundown { UUID = Guid.NewGuid(), Name = "Test Rundown" };
            var story = new RundownStory { UUID = Guid.NewGuid(), Name = "Test Story" };

            _mockRundownRepository.Setup(repo => repo.UpdateStoryAsync(rundown.UUID, story)).Returns(Task.CompletedTask);

            // Act
            await _storyDetailService.CreateStoryDetailAsync(rundown, story);

            // Assert
            _mockRundownRepository.Verify(repo => repo.UpdateStoryAsync(rundown.UUID, story), Times.Once);
            _mockKafkaService.Verify(kafka => kafka.SendMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
