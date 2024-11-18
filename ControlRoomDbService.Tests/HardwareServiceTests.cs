using Xunit;
using Moq;
using ControlRoomDbService.BLL.Interfaces;
using ControlRoomDbService.DAL.Interfaces;
using ControlRoomDbService.Models;
using ControlRoomDbService.BLL.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonClassLibrary.Enum;

namespace ControlRoomDbService.Tests.Services
{
    public class HardwareServiceTests
    {
        private readonly Mock<IHardwareRepository> _mockHardwareRepository;
        private readonly Mock<IKafkaService> _mockKafkaService;
        private readonly HardwareService _hardwareService;

        public HardwareServiceTests()
        {
            _mockHardwareRepository = new Mock<IHardwareRepository>();
            _mockKafkaService = new Mock<IKafkaService>();
            _hardwareService = new HardwareService(_mockHardwareRepository.Object, _mockKafkaService.Object);
        }

        [Fact]
        public async Task GetAllHardwareAsync_ReturnsListOfHardware()
        {
            // Arrange
            var mockHardwareList = new List<Hardware>
            {
                new Hardware { UUID = Guid.NewGuid(), Name = "Hardware 1" },
                new Hardware { UUID = Guid.NewGuid(), Name = "Hardware 2" }
            };
            _mockHardwareRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(mockHardwareList);

            // Act
            var result = await _hardwareService.GetAllHardwareAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _mockHardwareRepository.Verify(repo => repo.GetAsync(), Times.Once);
        }

        [Fact]
        public async Task GetHardwareByIdAsync_WithValidId_ReturnsHardware()
        {
            // Arrange
            var hardwareId = Guid.NewGuid().ToString();
            var mockHardware = new Hardware { UUID = Guid.Parse(hardwareId), Name = "Hardware 1" };
            _mockHardwareRepository.Setup(repo => repo.GetByIdAsync(hardwareId)).ReturnsAsync(mockHardware);

            // Act
            var result = await _hardwareService.GetHardwareByIdAsync(hardwareId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(mockHardware.UUID, result.UUID);
            _mockHardwareRepository.Verify(repo => repo.GetByIdAsync(hardwareId), Times.Once);
        }

        [Fact]
        public async Task GetHardwareByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var hardwareId = Guid.NewGuid().ToString();
            _mockHardwareRepository.Setup(repo => repo.GetByIdAsync(hardwareId)).ReturnsAsync((Hardware)null);

            // Act
            var result = await _hardwareService.GetHardwareByIdAsync(hardwareId);

            // Assert
            Assert.Null(result);
            _mockHardwareRepository.Verify(repo => repo.GetByIdAsync(hardwareId), Times.Once);
        }

        [Fact]
        public async Task CreateHardwareAsync_CreatesHardwareAndSendsMessage()
        {
            // Arrange
            var newHardware = new Hardware { UUID = Guid.NewGuid(), Name = "New Hardware" };

            // Act
            await _hardwareService.CreateHardwareAsync(newHardware);

            // Assert
            _mockHardwareRepository.Verify(repo => repo.CreateAsync(newHardware), Times.Once);
            _mockKafkaService.Verify(kafka => kafka.SendMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdateHardwareAsync_WithValidId_UpdatesHardwareAndSendsMessage()
        {
            // Arrange
            var hardwareId = Guid.NewGuid().ToString();
            var updatedHardware = new Hardware { UUID = Guid.Parse(hardwareId), Name = "Updated Hardware" };

            _mockHardwareRepository.Setup(repo => repo.UpdateAsync(hardwareId, updatedHardware)).Returns(Task.FromResult(updatedHardware));

            // Act
            await _hardwareService.UpdateHardwareAsync(hardwareId, updatedHardware);

            // Assert
            _mockHardwareRepository.Verify(repo => repo.UpdateAsync(hardwareId, updatedHardware), Times.Once);
            _mockKafkaService.Verify(kafka => kafka.SendMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeleteHardwareAsync_WithValidId_DeletesHardwareAndSendsMessage()
        {
            // Arrange
            var hardwareId = Guid.NewGuid().ToString();

            // Act
            await _hardwareService.DeleteHardwareAsync(hardwareId);

            // Assert
            _mockHardwareRepository.Verify(repo => repo.RemoveAsync(hardwareId), Times.Once);
            _mockKafkaService.Verify(kafka => kafka.SendMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}

