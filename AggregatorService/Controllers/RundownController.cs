using AggregatorService.Managers;
using Microsoft.AspNetCore.Mvc;

namespace AggregatorService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RundownController(RundownManager rundownManager) : ControllerBase
    {
        private readonly RundownManager _rundownManager = rundownManager;

        [HttpGet("fetch-active-rundowns-with-controlrooms")]
        public async Task<IActionResult> FetchRundownsWithControlRooms()
        {
            var data = await _rundownManager.FetchRundownsWithControlRoomData();
            return Ok(data);
        }

        [HttpGet("fetch-rundown/{rundownId}")]
        public async Task<IActionResult> FetchSelectedRundown(string rundownId)
        {
            var data = await _rundownManager.FetchSelectedRundown(rundownId);
            return Ok(data);
        }

        [HttpPut("update-rundown-controlroom/{rundownId}")]
        public async Task<IActionResult> UpdateRundownControlRoom(string rundownId, string controlRoomId)
        {
            var data = await _rundownManager.UpdateControlRoomAsync(rundownId, controlRoomId);
            return Ok(data);
        }
    }
}
