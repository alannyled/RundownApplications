using ControlRoomDbService.BLL.Interfaces;
using ControlRoomDbService.DTO;
using ControlRoomDbService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControlRoomDbService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControlRoomController : ControllerBase
    {
        private readonly IControlRoomService _controlRoomService;

        public ControlRoomController(IControlRoomService controlRoomService)
        {
            _controlRoomService = controlRoomService;
        }

        [HttpGet]
        public async Task<List<ControlRoom>> Get() => await _controlRoomService.GetControlRoomsAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<ControlRoom>> Get(string id)
        {
            var controlRoom = await _controlRoomService.GetControlRoomByIdAsync(id);

            if (controlRoom == null)
            {
                return NotFound();
            }

            return controlRoom;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateControlRoomDto newControlRoomDto)
        {
            var newControlRoom = new ControlRoom
            {
                Name = newControlRoomDto.Name,
                Location = newControlRoomDto.Location,
                CreatedDate = DateTime.Now
            };

            await _controlRoomService.CreateControlRoomAsync(newControlRoom);
            return CreatedAtAction(nameof(Get), new { id = newControlRoom.UUID }, newControlRoom);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, ControlRoom updatedControlRoom)
        {
            var existingControlRoom = await _controlRoomService.GetControlRoomByIdAsync(id);

            if (existingControlRoom == null)
            {
                return NotFound();
            }

            updatedControlRoom.UUID = existingControlRoom.UUID;
            await _controlRoomService.UpdateControlRoomAsync(id, updatedControlRoom);

            return Ok(updatedControlRoom);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var controlRoom = await _controlRoomService.GetControlRoomByIdAsync(id);

            if (controlRoom == null)
            {
                return NotFound();
            }

            await _controlRoomService.DeleteControlRoomAsync(id);
            return Ok(new { Message = $"Control room with ID {id} has been deleted successfully." });
        }

        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAll()
        {
            await _controlRoomService.DeleteAllControlRoomsAsync();
            return NoContent();
        }
    }
}
