using Microsoft.AspNetCore.Mvc;
using RundownDbService.BLL.Interfaces;
using CommonClassLibrary.DTO;
using RundownDbService.Models;
using Newtonsoft.Json;

namespace RundownDbService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RundownController(IRundownService rundownService, IStoryDetailService storyDetailService, IRundownStoryService rundownStoryService) : ControllerBase
    {
        private readonly IRundownService _rundownService = rundownService;
        private readonly IStoryDetailService _storyDetailService = storyDetailService;
        private readonly IRundownStoryService _rundownStoryService = rundownStoryService;

        [HttpGet]
        public async Task<ActionResult<List<Rundown>>> GetAll()
        {
            var rundowns = await _rundownService.GetAllRundownsAsync();
            return Ok(rundowns);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Rundown>> GetById(Guid id)
        {
            var rundown = await _rundownService.GetRundownByIdAsync(id);
            if (rundown == null)
            {
                return NotFound();
            }
            return Ok(rundown);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Rundown newRundown)
        {
            newRundown.UUID = Guid.NewGuid();
            await _rundownService.CreateRundownAsync(newRundown);
            return CreatedAtAction(nameof(GetById), new { id = newRundown.UUID }, newRundown);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] RundownDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    Console.WriteLine("Fejl: Modtaget RundownDTO er null.");
                    return BadRequest("Modtaget data er ugyldig.");
                }

                if (!ModelState.IsValid)
                {
                    Console.WriteLine("Fejl: ModelState er ugyldig.");
                    foreach (var key in ModelState.Keys)
                    {
                        var errors = ModelState[key]?.Errors;
                    }
                    return BadRequest(ModelState);
                }

                var rundown = await _rundownService.GetRundownByIdAsync(id);
                if (rundown == null)
                {
                    Console.WriteLine($"Rundown med ID: {id} blev ikke fundet.");
                    return NotFound();
                }

                // Opdater rundown
                rundown.ControlRoomId = Guid.Parse(dto.ControlRoomId);
                rundown.ArchivedDate = dto.ArchivedDate;
                

                foreach (var storyDto in dto.Stories)
                {
                    var existingStory = rundown.Stories.FirstOrDefault(i => i.UUID == storyDto.UUID);
                    if (existingStory != null)
                    {
                        existingStory.Order = storyDto.Order;
                    }
                }
                // Fjern Stories der er slettede i DTO
                var dtoUUIDs = dto.Stories.Select(story => story.UUID).ToHashSet();
                rundown.Stories.RemoveAll(story => !dtoUUIDs.Contains(story.UUID));

                await _rundownService.UpdateRundownAsync(id, rundown);
                Console.WriteLine($"Opdatering af Rundown med ID: {id} lykkedes.");

                return Ok(rundown);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl under opdatering af rundown: {ex.Message}");
                return BadRequest($"Fejl under opdatering: {ex.Message}");
            }
        }


        [HttpPut("add-story-to-rundown/{id:guid}")]
        public async Task<IActionResult> AddStoryToRundown(Guid id, [FromBody] RundownStoryDTO rundownDto)
        {
            if (rundownDto == null)
            {
                return BadRequest("RundownDTO modellen mangler?");
            }

            var existingRundown = await _rundownService.GetRundownByIdAsync(id);
            if (existingRundown == null)
            {
                return NotFound();
            }

            TimeSpan duration = !string.IsNullOrEmpty(rundownDto.Duration)
                ? TimeSpan.Parse(rundownDto.Duration)
                : TimeSpan.Zero;

            existingRundown.Stories.Add(new RundownStory
            {
                UUID = Guid.NewGuid(),
                RundownId = id,
                Name = rundownDto.Name,
                Duration = duration,
                Order = rundownDto.Order
            });
      
            var updatedRundown = await _rundownService.UpdateRundownAsync(id, existingRundown);
            return Ok(updatedRundown);
        }


        [HttpPut("add-story-detail-to-rundown/{rundownId:guid}")]
        public async Task<IActionResult> AddStoryDetailToRundown(Guid rundownId, [FromBody] DetailDTO storyDetailDto)
        {
            var existingRundown = await _rundownService.GetRundownByIdAsync(rundownId);
            if (existingRundown == null)
            {
                return NotFound("Rundown ikke fundet.");
            }

            var existingStory = existingRundown.Stories.FirstOrDefault(i => i.UUID == storyDetailDto.StoryId);
            if (existingStory == null)
            {
                return NotFound("Story ikke fundet.");
            }

            var storyDetail = _storyDetailService.GetModel(storyDetailDto.Type);
            if (storyDetail == null)
            {
                return BadRequest("Kunne ikke oprette story detail baseret på den angivne type.");
            }

            // Sæt værdierne på den nye story detail
            storyDetail.UUID = Guid.NewGuid();
            storyDetail.Title = storyDetailDto.Title;
            storyDetail.Duration = TimeSpan.Parse(storyDetailDto.Duration);
            storyDetail.StoryId = storyDetailDto.StoryId;
            storyDetail.Type = storyDetailDto.Type;
            storyDetail.Order = storyDetailDto.Order;

            // Tilpas værdier afhængigt af den specifikke type af story detail
            switch (storyDetail)
            {
                case StoryDetailVideo video when storyDetailDto.VideoPath != null:
                    video.VideoPath = storyDetailDto.VideoPath;
                    break;
                case StoryDetailTeleprompter teleprompter when storyDetailDto.PrompterText != null:
                    teleprompter.PrompterText = storyDetailDto.PrompterText;
                    break;
                case StoryDetailGraphic graphic when storyDetailDto.GraphicId != null:
                    graphic.GraphicId = storyDetailDto.GraphicId;
                    break;
                case StoryDetailComment comment when storyDetailDto.Comment != null:
                    comment.Comment = storyDetailDto.Comment;
                    break;
            }

            // Tilføj StoryDetail til existingStory's detaljer
            existingStory.Details.Add(storyDetail);

            // await _storyDetailService.CreateStoryDetailAsync(existingRundown.UUID, existingStory);
            await _storyDetailService.CreateStoryDetailAsync(existingRundown, existingStory);
            return Ok(existingRundown);
        }


        [HttpPut("edit-story-detail-in-rundown/{rundownId:guid}")]
        public async Task<IActionResult> EditStoryDetailInRundown(Guid rundownId, [FromBody] DetailDTO detailDto)
        {
    
            var existingRundown = await _rundownService.GetRundownByIdAsync(rundownId);
            if (existingRundown == null)
            {
                return NotFound("Rundown ikke fundet.");
            }

            var existingStory = existingRundown.Stories.FirstOrDefault(i => i.Details.Any(d => d.UUID == detailDto.UUID));
            if (existingStory == null)
            {
                return NotFound("story der indeholder detail ikke fundet.");
            }
   
            var existingDetail = existingStory.Details.FirstOrDefault(d => d.UUID == detailDto.UUID);
            if (existingDetail == null)
            {
                return NotFound($"StoryDetail med UUID {detailDto.UUID} blev ikke fundet.");
            }
      
            existingDetail.Title = detailDto.Title;
            existingDetail.Duration = TimeSpan.Parse(detailDto.Duration);
            existingDetail.Type = detailDto.Type;
            existingDetail.Order = detailDto.Order;

            switch (existingDetail)
            {
                case StoryDetailVideo video when detailDto.VideoPath != null:
                    video.VideoPath = detailDto.VideoPath;
                    break;
                case StoryDetailTeleprompter teleprompter when detailDto.PrompterText != null:
                    teleprompter.PrompterText = detailDto.PrompterText;
                    break;
                case StoryDetailGraphic graphic when detailDto.GraphicId != null:
                    graphic.GraphicId = detailDto.GraphicId;
                    break;
                case StoryDetailComment comment when detailDto.Comment != null:
                    comment.Comment = detailDto.Comment;
                    break;
            }

            // Gem ændringerne
            await _storyDetailService.CreateStoryDetailAsync(existingRundown, existingStory);

            return Ok(existingRundown);
        }



        [HttpDelete("delete-story-detail-from-rundown/{rundownId:guid}/{detailId:guid}")]
        public async Task<IActionResult> DeleteStoryDetailFromRundown(Guid rundownId, Guid detailId)
        {
        
            var existingRundown = await _rundownService.GetRundownByIdAsync(rundownId);
            if (existingRundown == null)
            {
                return NotFound("Rundown ikke fundet.");
            }
            var existingStory = existingRundown.Stories.FirstOrDefault(i => i.Details.Any(d => d.UUID == detailId));
            if (existingStory == null)
            {
                return NotFound("Story med det ønskede StoryDetail ikke fundet.");
            }

            var detailToRemove = existingStory.Details.FirstOrDefault(d => d.UUID == detailId);
            if (detailToRemove == null)
            {
                return NotFound("StoryDetail ikke fundet.");
            }

            existingStory.Details.Remove(detailToRemove);

            var updatedRundown = await _rundownService.UpdateRundownAsync(rundownId, existingRundown);

            return Ok(updatedRundown);
        }


        // delete rundown
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var rundown = await _rundownService.GetRundownByIdAsync(id);
            if (rundown == null)
            {
                return NotFound();
            }

            await _rundownService.DeleteRundownAsync(id);
            return NoContent();
        }
    }

    
}
