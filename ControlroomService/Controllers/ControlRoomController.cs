using ControlRoomService.DAL;
using ControlRoomService.DTO;
using ControlRoomService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControlRoomService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControlRoomController : ControllerBase
    {
        private readonly ControlRoomRepository _controlRoomService;

        public ControlRoomController(ControlRoomRepository controlRoomService)
        {
            _controlRoomService = controlRoomService;
        }

        [HttpGet]
        public async Task<List<ControlRoom>> Get() => await _controlRoomService.GetAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<ControlRoom>> Get(string id)
        {
            var controlRoom = await _controlRoomService.GetByIdAsync(id);

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

            await _controlRoomService.CreateAsync(newControlRoom);
            return CreatedAtAction(nameof(Get), new { id = newControlRoom.UUID }, newControlRoom);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, ControlRoom updatedControlRoom)
        {
            var existingControlRoom = await _controlRoomService.GetByIdAsync(id);

            if (existingControlRoom == null)
            {
                return NotFound();
            }

            updatedControlRoom.UUID = existingControlRoom.UUID;
            await _controlRoomService.UpdateAsync(id, updatedControlRoom);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var controlRoom = await _controlRoomService.GetByIdAsync(id);

            if (controlRoom == null)
            {
                return NotFound();
            }

            await _controlRoomService.RemoveAsync(id);
            return NoContent();
        }

        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAll()
        {
            await _controlRoomService.RemoveAllAsync();
            return NoContent();
        }
    }
}
