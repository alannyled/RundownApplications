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
    }
