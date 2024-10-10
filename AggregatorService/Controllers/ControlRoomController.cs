using AggregatorService.Managers;
using Microsoft.AspNetCore.Mvc;
using AggregatorService.Models;
using System.Threading.Tasks;
using AggregatorService.DTO;

namespace AggregatorService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControlRoomController(ControlRoomManager controlroomManager) : ControllerBase
    {
        private readonly ControlRoomManager _controlroomManager = controlroomManager;

        [HttpGet("fetch-controlroom-with-hardware")]
        public async Task<IActionResult> FetchControlRoomWithHardware()
        {
            var data = await _controlroomManager.FetchControlRoomWithHardwareData();
            return Ok(data);
        }

        [HttpPost("create-controlroom")]
        public async Task<IActionResult> CreateControlRoom([FromBody] ControlRoomDTO newControlRoom)
        {
            if (newControlRoom == null)
            {
                return BadRequest("Invalid control room data.");
            }

            var createdControlRoom = await _controlroomManager.CreateControlRoomAsync(newControlRoom);
            return CreatedAtAction(nameof(FetchControlRoomWithHardware), new { id = createdControlRoom.Uuid }, createdControlRoom);
        }

        [HttpPut("update-controlroom/{controlRoomId}")]
        public async Task<IActionResult> UpdateControlRoom(string controlRoomId, [FromBody] ControlRoom updatedControlRoom)
        {
            if (updatedControlRoom == null)
            {
                return BadRequest("Control room data is missing.");
            }

            var updatedControlRoomResponse = await _controlroomManager.UpdateControlRoomAsync(controlRoomId, updatedControlRoom);
            return Ok(updatedControlRoomResponse);
        }

        [HttpDelete("delete-controlroom/{controlRoomId}")]
        public async Task<IActionResult> DeleteControlRoom(string controlRoomId)
        {
            var deletedControlRoom = await _controlroomManager.DeleteControlRoomAsync(controlRoomId);
            return Ok(deletedControlRoom);
        }
    }
}