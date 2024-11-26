using AggregatorService.DTO;
using AggregatorService.Models;
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
            var controlRoomId = request.ControlRoomId ?? string.Empty;
            var rundown = await _rundownManager.CreateRundownFromTemplate(templateId, controlRoomId, request.BroadcastDate);
            return Ok(rundown);
        }
        
        
        [HttpGet("fetch-rundowns-with-controlrooms")]
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

        [HttpPut("add-story-to-rundown/{rundownId}")]
        public async Task<IActionResult> AddStoryToRundown(string rundownId, [FromBody] RundownStoryDTO storyDto)
        {
            var rundown = await _rundownManager.AddStoryToRundownAsync(Guid.Parse(rundownId), storyDto);
            return Ok(rundown);
        }

        [HttpPut("add-detail-to-story/{rundownId}")]
        public async Task<IActionResult> AddDetailToStory(string rundownId, [FromBody] StoryDetailDTO storyDetailDto)
        {
            var rundown = await _rundownManager.AddDetailToStoryAsync(Guid.Parse(rundownId), storyDetailDto);
            return Ok(rundown);
        }

        [HttpPut("update-detail-in-story/{rundownId}")]
        public async Task<IActionResult> UpdateDetailInStory(string rundownId, [FromBody] StoryDetailDTO storyDetailDto)
        {
            Console.WriteLine("Updating detail in story: " + JsonSerializer.Serialize(storyDetailDto));
            var rundown = await _rundownManager.UpdateStoryDetailAsync(Guid.Parse(rundownId), storyDetailDto);
            return Ok(rundown);
        }

        [HttpPut("update-rundown/{rundownId}")]
        public async Task<IActionResult> UpdateRundown(Guid rundownId, [FromBody] Rundown request)
        {
            var updatedRundown = await _rundownManager.UpdateRundownAsync(rundownId, request);
            return Ok(updatedRundown);
        }



    }
}
