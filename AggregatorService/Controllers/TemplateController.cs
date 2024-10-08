using AggregatorService.Managers;
using Microsoft.AspNetCore.Mvc;

namespace AggregatorService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemplateController(TemplateManager templateManager) : ControllerBase
    {
        private readonly TemplateManager _templateManager = templateManager;

        [HttpGet("fetch-all-rundown-templates")]
        public async Task<IActionResult> FetchRundownsWithControlRooms()
        {
            var data = await _templateManager.FetchRundownTemplate();
            return Ok(data);
        }
    }
}
