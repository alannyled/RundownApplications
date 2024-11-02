using Xunit;
using Moq;
using RundownDbService.BLL.Interfaces;
using RundownDbService.Controllers;
using RundownDbService.Models;
using Microsoft.AspNetCore.Mvc;

namespace RundownDbService.Tests
{
    public class RundownControllerTests
    {
        private readonly Mock<IRundownService> _mockRundownService;
        private readonly Mock<IItemDetailService> _mockItemDetailService; // Tilføjet mock for IItemDetailService
        private readonly Mock<IRundownItemService> _mockRundownItemService; // Tilføjet mock for IRundownItemService
        private readonly RundownController _controller;

        public RundownControllerTests()
        {
            _mockRundownService = new Mock<IRundownService>();
            _mockItemDetailService = new Mock<IItemDetailService>(); // Initialiseret mock for IItemDetailService
            _mockRundownItemService = new Mock<IRundownItemService>(); // Initialiseret mock for IRundownItemService

            _controller = new RundownController(_mockRundownService.Object, _mockItemDetailService.Object, _mockRundownItemService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfRundowns()
        {
            // Arrange
            var mockRundowns = new List<Rundown>
            {
                new Rundown { UUID = Guid.NewGuid(), ControlRoomId = Guid.NewGuid() },
                new Rundown { UUID = Guid.NewGuid(), ControlRoomId = Guid.NewGuid() }
            };
            _mockRundownService.Setup(service => service.GetAllRundownsAsync())
                               .ReturnsAsync(mockRundowns);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Rundown>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetById_RundownExists_ReturnsOkResult()
        {
            // Arrange
            var rundownId = Guid.NewGuid();
            var mockRundown = new Rundown { UUID = rundownId, ControlRoomId = Guid.NewGuid() };
            _mockRundownService.Setup(service => service.GetRundownByIdAsync(rundownId))
                               .ReturnsAsync(mockRundown);

            // Act
            var result = await _controller.GetById(rundownId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Rundown>(okResult.Value);
            Assert.Equal(rundownId, returnValue.UUID);
        }

        [Fact]
        public async Task GetById_RundownDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var rundownId = Guid.NewGuid();
            _mockRundownService.Setup(service => service.GetRundownByIdAsync(rundownId))
                               .ReturnsAsync((Rundown?)null);

            // Act
            var result = await _controller.GetById(rundownId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ValidRundown_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var newRundown = new Rundown { ControlRoomId = Guid.NewGuid() };
            _mockRundownService.Setup(service => service.CreateRundownAsync(It.IsAny<Rundown>()))
                               .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(newRundown);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task Delete_RundownExists_ReturnsNoContentResult()
        {
            // Arrange
            var rundownId = Guid.NewGuid();
            var mockRundown = new Rundown { UUID = rundownId };
            _mockRundownService.Setup(service => service.GetRundownByIdAsync(rundownId))
                               .ReturnsAsync(mockRundown);
            _mockRundownService.Setup(service => service.DeleteRundownAsync(rundownId))
                               .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(rundownId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
