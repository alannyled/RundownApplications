using AggregatorService.Managers;
using Microsoft.AspNetCore.Mvc;

namespace AggregatorService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AggregatorController : ControllerBase
    {
        private readonly ControlRoomManager _aggregatorManager;

        public AggregatorController(ControlRoomManager aggregatorManager)
        {
            _aggregatorManager = aggregatorManager;
        }

        [HttpGet("fetch-controlroom-with-hardware")]
        public async Task<IActionResult> FetchControlRoomWithHardware()
        {
            var data = await _aggregatorManager.FetchControlRoomWithHardwareData();
            return Ok(data);
        }
    }



}
