using AggregatorService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AggregatorService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AggregatorController : ControllerBase
    {
        private readonly AggregatorManager _aggregatorManager;

        public AggregatorController(AggregatorManager aggregatorManager)
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
