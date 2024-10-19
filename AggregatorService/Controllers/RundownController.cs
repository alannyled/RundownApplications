using AggregatorService.DTO;
using AggregatorService.Managers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AggregatorService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RundownController(RundownManager rundownManager) : ControllerBase
    {
        private readonly RundownManager _rundownManager = rundownManager;

        [HttpPost("create-rundown-from-template/{templateId}")]
        public async Task<IActionResult> CreateRundownFromTemplate(string templateId, [FromBody] CreateRundownRequest request)
        {

            var rundown = await _rundownManager.CreateRundownFromTemplate(templateId, request.ControlRoomId, request.BroadcastDate);
            return Ok(rundown);
        }
        
        
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
        public async Task<IActionResult> UpdateRundownControlRoom(string rundownId, [FromBody] RundownDTO request)
        {
            if (request == null || string.IsNullOrEmpty(request.ControlRoomId))
            {
                return BadRequest("Invalid request.");
            }
            var updatedRundown = await _rundownManager.UpdateControlRoomAsync(rundownId, request);

            return Ok(updatedRundown);
        }

        [HttpPut("add-item-to-rundown/{rundownId}")]
        public async Task<IActionResult> AddItemToRundown(string rundownId, [FromBody] RundownItemDTO itemDto)
        {
            var rundown = await _rundownManager.AddItemToRundownAsync(Guid.Parse(rundownId), itemDto);
            return Ok(rundown);
        }

        [HttpPut("add-detail-to-item/{rundownId}")]
        public async Task<IActionResult> AddDetailToItem(string rundownId, [FromBody] RundownItemDTO itemDto)
        {
            var rundown = await _rundownManager.AddDetailToItemAsync(Guid.Parse(rundownId), itemDto);
            return Ok(rundown);
        }




    }
}
