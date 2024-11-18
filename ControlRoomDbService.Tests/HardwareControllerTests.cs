using Xunit;
using Moq;
using ControlRoomDbService.Controllers;
using ControlRoomDbService.BLL.Interfaces;
using ControlRoomDbService.Models;
using ControlRoomDbService.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControlRoomDbService.Tests.Controllers
{
    public class HardwareControllerTests
    {
        private readonly Mock<IHardwareService> _mockHardwareService;
        private readonly HardwareController _hardwareController;

        public HardwareControllerTests()
        {
            _mockHardwareService = new Mock<IHardwareService>();
            _hardwareController = new HardwareController(_mockHardwareService.Object);
        }

        [Fact]
        public async Task Get_ReturnsListOfHardware()
        {
            // Arrange
            var mockHardwares = new List<Hardware>
            {
                new Hardware { UUID = Guid.NewGuid(), Name = "Hardware 1" },
                new Hardware { UUID = Guid.NewGuid(), Name = "Hardware 2" }
            };

            _mockHardwareService.Setup(service => service.GetAllHardwareAsync()).ReturnsAsync(mockHardwares);

            // Act
            var result = await _hardwareController.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Hardware>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            _mockHardwareService.Verify(service => service.GetAllHardwareAsync(), Times.Once);
        }

        [Fact]
        public async Task Get_WithValidId_ReturnsHardware()
        {
            // Arrange
            var hardwareId = Guid.NewGuid();
            var mockHardware = new Hardware { UUID = hardwareId, Name = "Hardware 1" };

            _mockHardwareService.Setup(service => service.GetHardwareByIdAsync(hardwareId.ToString())).ReturnsAsync(mockHardware);

            // Act
            var result = await _hardwareController.Get(hardwareId.ToString());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var hardware = Assert.IsType<Hardware>(okResult.Value);
            Assert.Equal(mockHardware.UUID, hardware.UUID);
            _mockHardwareService.Verify(service => service.GetHardwareByIdAsync(hardwareId.ToString()), Times.Once);
        }

        [Fact]
        public async Task Get_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var hardwareId = Guid.NewGuid().ToString();

            _mockHardwareService.Setup(service => service.GetHardwareByIdAsync(hardwareId)).ReturnsAsync((Hardware)null);

            // Act
            var result = await _hardwareController.Get(hardwareId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
            _mockHardwareService.Verify(service => service.GetHardwareByIdAsync(hardwareId), Times.Once);
        }

        [Fact]
        public async Task Create_ValidHardware_ReturnsCreatedAtAction()
        {
            // Arrange
            var newHardwareDto = new CreateHardwareDto { Name = "New Hardware", ControlRoomId = Guid.NewGuid().ToString() };
            var newHardware = new Hardware { UUID = Guid.NewGuid(), Name = newHardwareDto.Name, ControlRoomId = newHardwareDto.ControlRoomId, CreatedDate = DateTime.Now };

            _mockHardwareService.Setup(service => service.CreateHardwareAsync(It.IsAny<Hardware>())).Returns(Task.CompletedTask);

            // Act
            var result = await _hardwareController.Create(newHardwareDto);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_hardwareController.Get), actionResult.ActionName);
            _mockHardwareService.Verify(service => service.CreateHardwareAsync(It.IsAny<Hardware>()), Times.Once);
        }

        [Fact]
        public async Task Update_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var hardwareId = Guid.NewGuid();
            var updatedHardware = new Hardware { UUID = hardwareId, Name = "Updated Hardware", ControlRoomId = Guid.NewGuid().ToString() };

            _mockHardwareService.Setup(service => service.GetHardwareByIdAsync(hardwareId.ToString())).ReturnsAsync(updatedHardware);
            _mockHardwareService.Setup(service => service.UpdateHardwareAsync(hardwareId.ToString(), updatedHardware)).Returns(Task.CompletedTask);

            // Act
            var result = await _hardwareController.Update(hardwareId.ToString(), updatedHardware);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            _mockHardwareService.Verify(service => service.GetHardwareByIdAsync(hardwareId.ToString()), Times.Once);
            _mockHardwareService.Verify(service => service.UpdateHardwareAsync(hardwareId.ToString(), updatedHardware), Times.Once);
        }

        [Fact]
        public async Task Update_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var hardwareId = Guid.NewGuid();
            var updatedHardware = new Hardware { UUID = hardwareId, Name = "Updated Hardware", ControlRoomId = Guid.NewGuid().ToString() };

            _mockHardwareService.Setup(service => service.GetHardwareByIdAsync(hardwareId.ToString())).ReturnsAsync((Hardware)null);

            // Act
            var result = await _hardwareController.Update(hardwareId.ToString(), updatedHardware);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _mockHardwareService.Verify(service => service.GetHardwareByIdAsync(hardwareId.ToString()), Times.Once);
            _mockHardwareService.Verify(service => service.UpdateHardwareAsync(It.IsAny<string>(), It.IsAny<Hardware>()), Times.Never);
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsNoContentResult()
        {
            // Arrange
            var hardwareId = Guid.NewGuid();
            var mockHardware = new Hardware { UUID = hardwareId, Name = "Hardware to Delete" };

            _mockHardwareService.Setup(service => service.GetHardwareByIdAsync(hardwareId.ToString())).ReturnsAsync(mockHardware);
            _mockHardwareService.Setup(service => service.DeleteHardwareAsync(hardwareId.ToString())).Returns(Task.CompletedTask);

            // Act
            var result = await _hardwareController.Delete(hardwareId.ToString());

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockHardwareService.Verify(service => service.GetHardwareByIdAsync(hardwareId.ToString()), Times.Once);
            _mockHardwareService.Verify(service => service.DeleteHardwareAsync(hardwareId.ToString()), Times.Once);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var hardwareId = Guid.NewGuid().ToString();

            _mockHardwareService.Setup(service => service.GetHardwareByIdAsync(hardwareId)).ReturnsAsync((Hardware)null);

            // Act
            var result = await _hardwareController.Delete(hardwareId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _mockHardwareService.Verify(service => service.GetHardwareByIdAsync(hardwareId), Times.Once);
            _mockHardwareService.Verify(service => service.DeleteHardwareAsync(It.IsAny<string>()), Times.Never);
        }
    }
}
