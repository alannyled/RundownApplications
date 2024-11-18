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
    public class ControlRoomServiceTests
    {
        private readonly Mock<IControlRoomRepository> _mockControlRoomRepository;
        private readonly Mock<IKafkaService> _mockKafkaService;
        private readonly ControlRoomService _controlRoomService;

        public ControlRoomServiceTests()
        {
            _mockControlRoomRepository = new Mock<IControlRoomRepository>();
            _mockKafkaService = new Mock<IKafkaService>();
            _controlRoomService = new ControlRoomService(_mockControlRoomRepository.Object, _mockKafkaService.Object);
        }

        [Fact]
        public async Task GetControlRoomsAsync_ReturnsListOfControlRooms()
        {
            // Arrange
            var mockControlRooms = new List<ControlRoom>
            {
                new ControlRoom { UUID = Guid.NewGuid(), Name = "Control Room 1" },
                new ControlRoom { UUID = Guid.NewGuid(), Name = "Control Room 2" }
            };
            _mockControlRoomRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(mockControlRooms);

            // Act
            var result = await _controlRoomService.GetControlRoomsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _mockControlRoomRepository.Verify(repo => repo.GetAsync(), Times.Once);
        }

        [Fact]
        public async Task GetControlRoomByIdAsync_WithValidId_ReturnsControlRoom()
        {
            // Arrange
            var controlRoomId = Guid.NewGuid().ToString();
            var mockControlRoom = new ControlRoom { UUID = Guid.Parse(controlRoomId), Name = "Control Room 1" };
            _mockControlRoomRepository.Setup(repo => repo.GetByIdAsync(controlRoomId)).ReturnsAsync(mockControlRoom);

            // Act
            var result = await _controlRoomService.GetControlRoomByIdAsync(controlRoomId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(mockControlRoom.UUID, result.UUID);
            _mockControlRoomRepository.Verify(repo => repo.GetByIdAsync(controlRoomId), Times.Once);
        }

        [Fact]
        public async Task GetControlRoomByIdAsync_WithInvalidId_ReturnsEmptyControlRoom()
        {
            // Arrange
            var controlRoomId = Guid.NewGuid().ToString();
            _mockControlRoomRepository.Setup(repo => repo.GetByIdAsync(controlRoomId)).ReturnsAsync((ControlRoom)null);

            // Act
            var result = await _controlRoomService.GetControlRoomByIdAsync(controlRoomId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(Guid.Empty, result.UUID);
            _mockControlRoomRepository.Verify(repo => repo.GetByIdAsync(controlRoomId), Times.Once);
        }

        [Fact]
        public async Task CreateControlRoomAsync_CreatesControlRoomAndSendsMessage()
        {
            // Arrange
            var newControlRoom = new ControlRoom { UUID = Guid.NewGuid(), Name = "New Control Room" };

            // Act
            await _controlRoomService.CreateControlRoomAsync(newControlRoom);

            // Assert
            _mockControlRoomRepository.Verify(repo => repo.CreateAsync(newControlRoom), Times.Once);
            _mockKafkaService.Verify(kafka => kafka.SendMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdateControlRoomAsync_WithValidId_UpdatesControlRoomAndSendsMessage()
        {
            // Arrange
            var controlRoomId = Guid.NewGuid().ToString();
            var updatedControlRoom = new ControlRoom { UUID = Guid.Parse(controlRoomId), Name = "Updated Control Room" };

            _mockControlRoomRepository.Setup(repo => repo.UpdateAsync(controlRoomId, updatedControlRoom)).ReturnsAsync(new List<ControlRoom> { updatedControlRoom });

            // Act
            await _controlRoomService.UpdateControlRoomAsync(controlRoomId, updatedControlRoom);

            // Assert
            _mockControlRoomRepository.Verify(repo => repo.UpdateAsync(controlRoomId, updatedControlRoom), Times.Once);
            _mockKafkaService.Verify(kafka => kafka.SendMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeleteControlRoomAsync_WithValidId_DeletesControlRoomAndSendsMessage()
        {
            // Arrange
            var controlRoomId = Guid.NewGuid().ToString();

            // Act
            await _controlRoomService.DeleteControlRoomAsync(controlRoomId);

            // Assert
            _mockControlRoomRepository.Verify(repo => repo.RemoveAsync(controlRoomId), Times.Once);
            _mockKafkaService.Verify(kafka => kafka.SendMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAllControlRoomsAsync_DeletesAllControlRooms()
        {
            // Act
            await _controlRoomService.DeleteAllControlRoomsAsync();

            // Assert
            _mockControlRoomRepository.Verify(repo => repo.RemoveAllAsync(), Times.Once);
        }
    }
}

