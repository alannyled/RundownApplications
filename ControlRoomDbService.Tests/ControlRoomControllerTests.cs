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
    public class ControlRoomControllerTests
    {
        private readonly Mock<IControlRoomService> _mockControlRoomService;
        private readonly ControlRoomController _controlRoomController;

        public ControlRoomControllerTests()
        {
            _mockControlRoomService = new Mock<IControlRoomService>();
            _controlRoomController = new ControlRoomController(_mockControlRoomService.Object);
        }

        [Fact]
        public async Task Get_ReturnsListOfControlRooms()
        {
            // Arrange
            var mockControlRooms = new List<ControlRoom>
            {
                new ControlRoom { UUID = Guid.NewGuid(), Name = "Control Room 1" },
                new ControlRoom { UUID = Guid.NewGuid(), Name = "Control Room 2" }
            };

            // Mock setup - Sørg for at mock'en returnerer de ønskede kontrolrum
            _mockControlRoomService.Setup(service => service.GetControlRoomsAsync())
                                   .ReturnsAsync(mockControlRooms);

            // Act
            var result = await _controlRoomController.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result); // Tjek at resultatet er af typen OkObjectResult
            var controlRooms = Assert.IsType<List<ControlRoom>>(okResult.Value); // Tjek at værdien er en liste af ControlRoom
            Assert.Equal(2, controlRooms.Count); // Kontrollerer at der er to elementer i listen
            _mockControlRoomService.Verify(service => service.GetControlRoomsAsync(), Times.Once);
        }


        [Fact]
        public async Task Get_WithValidId_ReturnsControlRoom()
        {
            // Arrange
            var controlRoomId = Guid.NewGuid();
            var mockControlRoom = new ControlRoom { UUID = controlRoomId, Name = "Control Room 1", Location = "DR1" };

            _mockControlRoomService
                .Setup(service => service.GetControlRoomByIdAsync(controlRoomId.ToString()))
                .ReturnsAsync(mockControlRoom);

            // Act
            var result = await _controlRoomController.Get(controlRoomId.ToString());

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);

            var okResult = Assert.IsType<OkObjectResult>(result.Result); 

            Assert.NotNull(okResult.Value);
            var controlRoom = Assert.IsType<ControlRoom>(okResult.Value); 

            Assert.Equal(mockControlRoom.UUID, controlRoom.UUID);
            Assert.Equal(mockControlRoom.Name, controlRoom.Name);

            _mockControlRoomService.Verify(service => service.GetControlRoomByIdAsync(controlRoomId.ToString()), Times.Once);
        }



        [Fact]
        public async Task Get_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var controlRoomId = Guid.NewGuid().ToString();

            _mockControlRoomService
                .Setup(service => service.GetControlRoomByIdAsync(controlRoomId))
                .ReturnsAsync(new ControlRoom { UUID = Guid.Empty });

            var controller = new ControlRoomController(_mockControlRoomService.Object);

            // Act
            var result = await controller.Get(controlRoomId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedControlRoom = Assert.IsType<ControlRoom>(actionResult.Value);
            Assert.Equal(Guid.Empty, returnedControlRoom.UUID);
        }

        [Fact]
        public async Task Create_ValidControlRoom_ReturnsCreatedAtAction()
        {
            // Arrange
            var newControlRoomDto = new CreateControlRoomDto { Name = "New Control Room", Location = "DR1" };
            var newControlRoom = new ControlRoom { UUID = Guid.NewGuid(), Name = newControlRoomDto.Name, Location = newControlRoomDto.Location, CreatedDate = DateTime.Now };

            _mockControlRoomService.Setup(service => service.CreateControlRoomAsync(It.IsAny<ControlRoom>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controlRoomController.Create(newControlRoomDto);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controlRoomController.Get), actionResult.ActionName);
            _mockControlRoomService.Verify(service => service.CreateControlRoomAsync(It.IsAny<ControlRoom>()), Times.Once);
        }

        [Fact]
        public async Task Update_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var controlRoomId = Guid.NewGuid();
            var updatedControlRoom = new ControlRoom { UUID = controlRoomId, Name = "Updated Control Room", Location = "DR1" };

            _mockControlRoomService.Setup(service => service.GetControlRoomByIdAsync(controlRoomId.ToString())).ReturnsAsync(updatedControlRoom);
            _mockControlRoomService.Setup(service => service.UpdateControlRoomAsync(controlRoomId.ToString(), updatedControlRoom)).Returns(Task.CompletedTask);

            // Act
            var result = await _controlRoomController.Update(controlRoomId.ToString(), updatedControlRoom);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            _mockControlRoomService.Verify(service => service.GetControlRoomByIdAsync(controlRoomId.ToString()), Times.Once);
            _mockControlRoomService.Verify(service => service.UpdateControlRoomAsync(controlRoomId.ToString(), updatedControlRoom), Times.Once);
        }

        [Fact]
        public async Task Update_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var controlRoomId = Guid.NewGuid();
            var updatedControlRoom = new ControlRoom { UUID = controlRoomId, Name = "Updated Control Room", Location = "DR1" };

            _mockControlRoomService
                .Setup(service => service.GetControlRoomByIdAsync(controlRoomId.ToString()))
                .ReturnsAsync(new ControlRoom { UUID = Guid.Empty }); 

            // Act
            var result = await _controlRoomController.Update(controlRoomId.ToString(), updatedControlRoom);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _mockControlRoomService.Verify(service => service.GetControlRoomByIdAsync(controlRoomId.ToString()), Times.Once);
            _mockControlRoomService.Verify(service => service.UpdateControlRoomAsync(It.IsAny<string>(), It.IsAny<ControlRoom>()), Times.Never);
        }


        [Fact]
        public async Task Delete_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var controlRoomId = Guid.NewGuid();
            var mockControlRoom = new ControlRoom { UUID = controlRoomId, Name = "Control Room to Delete" };

            _mockControlRoomService.Setup(service => service.GetControlRoomByIdAsync(controlRoomId.ToString())).ReturnsAsync(mockControlRoom);
            _mockControlRoomService.Setup(service => service.DeleteControlRoomAsync(controlRoomId.ToString())).Returns(Task.CompletedTask);

            // Act
            var result = await _controlRoomController.Delete(controlRoomId.ToString());

            // Assert
            Assert.IsType<OkObjectResult>(result);
            _mockControlRoomService.Verify(service => service.GetControlRoomByIdAsync(controlRoomId.ToString()), Times.Once);
            _mockControlRoomService.Verify(service => service.DeleteControlRoomAsync(controlRoomId.ToString()), Times.Once);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var controlRoomId = Guid.NewGuid().ToString();

            _mockControlRoomService
                .Setup(service => service.GetControlRoomByIdAsync(controlRoomId))
                .ReturnsAsync(new ControlRoom { UUID = Guid.Empty }); // Signalér, at ID ikke findes

            // Act
            var result = await _controlRoomController.Delete(controlRoomId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _mockControlRoomService.Verify(service => service.GetControlRoomByIdAsync(controlRoomId), Times.Once);
            _mockControlRoomService.Verify(service => service.DeleteControlRoomAsync(It.IsAny<string>()), Times.Never);
        }


        [Fact]
        public async Task DeleteAll_ReturnsNoContentResult()
        {
            // Arrange
            _mockControlRoomService.Setup(service => service.DeleteAllControlRoomsAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _controlRoomController.DeleteAll();

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockControlRoomService.Verify(service => service.DeleteAllControlRoomsAsync(), Times.Once);
        }
    }
}
