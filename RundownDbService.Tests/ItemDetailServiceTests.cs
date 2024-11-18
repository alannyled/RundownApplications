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
    public class ItemDetailServiceTests
    {
        private readonly Mock<IRundownRepository> _mockRundownRepository;
        private readonly Mock<IKafkaService> _mockKafkaService;
        private readonly ItemDetailService _itemDetailService;

        public ItemDetailServiceTests()
        {
            _mockRundownRepository = new Mock<IRundownRepository>();
            _mockKafkaService = new Mock<IKafkaService>();
            _itemDetailService = new ItemDetailService(_mockRundownRepository.Object, _mockKafkaService.Object);
        }

        [Theory]
        [InlineData("Video", typeof(ItemDetailVideo))]
        [InlineData("Teleprompter", typeof(ItemDetailTeleprompter))]
        [InlineData("Grafik", typeof(ItemDetailGraphic))]
        [InlineData("Kommentar", typeof(ItemDetailComment))]
        [InlineData("Voiceover", typeof(ItemDetailTeleprompter))]
        [InlineData("Unknown", typeof(ItemDetail))]
        public void GetModel_ShouldReturnCorrectItemDetailModel(string type, Type expectedType)
        {
            // Act
            var result = _itemDetailService.GetModel(type);

            // Assert
            Assert.NotNull(result);
            Assert.IsType(expectedType, result);
        }

        [Fact]
        public async Task CreateItemDetailAsync_WithValidRundownAndItem_UpdatesRundownAndSendsMessage()
        {
            // Arrange
            var rundown = new Rundown { UUID = Guid.NewGuid(), Name = "Test Rundown" };
            var item = new RundownItem { UUID = Guid.NewGuid(), Name = "Test Item" };

            _mockRundownRepository.Setup(repo => repo.UpdateItemAsync(rundown.UUID, item)).Returns(Task.CompletedTask);

            // Act
            await _itemDetailService.CreateItemDetailAsync(rundown, item);

            // Assert
            _mockRundownRepository.Verify(repo => repo.UpdateItemAsync(rundown.UUID, item), Times.Once);
            _mockKafkaService.Verify(kafka => kafka.SendMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
