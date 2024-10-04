using AggregatorService.Managers;
using Microsoft.AspNetCore.Mvc;
using AggregatorService.Models; // Antag at ControlRoomDTO ligger her
using System.Threading.Tasks;

namespace AggregatorService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AggregatorController(ControlRoomManager aggregatorManager) : ControllerBase
    {
        private readonly ControlRoomManager _aggregatorManager = aggregatorManager;

        [HttpGet("fetch-controlroom-with-hardware")]
        public async Task<IActionResult> FetchControlRoomWithHardware()
        {
            var data = await _aggregatorManager.FetchControlRoomWithHardwareData();
            return Ok(data);
        }

        [HttpPost("create-controlroom")]
        public async Task<IActionResult> CreateControlRoom([FromBody] ControlRoom newControlRoom)
        {
            if (newControlRoom == null)
            {
                return BadRequest("Invalid control room data.");
            }

            var createdControlRoom = await _aggregatorManager.CreateControlRoomAsync(newControlRoom);
            return CreatedAtAction(nameof(FetchControlRoomWithHardware), new { id = createdControlRoom.Uuid }, createdControlRoom);
        }
    }
}
