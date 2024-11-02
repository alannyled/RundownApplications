using Microsoft.AspNetCore.Mvc;
using RundownDbService.BLL.Interfaces;
using CommonClassLibrary.DTO;
using RundownDbService.Models;

namespace RundownDbService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RundownController : ControllerBase
    {
        private readonly IRundownService _rundownService;
        private readonly IItemDetailService _itemDetailService;
        private readonly IRundownItemService _rundownItemService;

        public RundownController(IRundownService rundownService, IItemDetailService itemDetailService, IRundownItemService rundownItemService)
        {
            _rundownService = rundownService;
            _itemDetailService = itemDetailService;
            _rundownItemService = rundownItemService;
        }

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
            var rundown = await _rundownService.GetRundownByIdAsync(id);
            if (rundown == null)
            {
                return NotFound();
            }
            // Opdater felter i rundown objektet
            rundown.ControlRoomId = Guid.Parse(dto.ControlRoomId);
            rundown.ArchivedDate = dto.ArchivedDate;
            await _rundownService.UpdateRundownAsync(id, rundown);

            return Ok(rundown);
        }


        [HttpPut("add-item-to-rundown/{id:guid}")]
        public async Task<IActionResult> AddItemToRundown(Guid id, [FromBody] RundownItemDTO rundownDto)
        {
            if (rundownDto == null)
            {
                return BadRequest("RundownDTO modellen mangler?");
            }

            // Hent eksisterende rundown baseret på ID
            var existingRundown = await _rundownService.GetRundownByIdAsync(id);
            if (existingRundown == null)
            {
                return NotFound();
            }
            existingRundown.Items.Add(new RundownItem
            {
                UUID = Guid.NewGuid(),
                RundownId = id,
                Name = rundownDto.Name,
                Duration = TimeSpan.Parse(rundownDto.Duration),
                Order = rundownDto.Order
            });
      
            var updatedRundown = await _rundownService.UpdateRundownAsync(id, existingRundown);

            return Ok(updatedRundown);
        }


        [HttpPut("add-item-detail-to-rundown/{rundownId:guid}")]
        public async Task<IActionResult> AddItemDetailToRundown(Guid rundownId, [FromBody] DetailDTO itemDetailDto)
        {
            var existingRundown = await _rundownService.GetRundownByIdAsync(rundownId);
            if (existingRundown == null)
            {
                return NotFound("Rundown ikke fundet.");
            }

            var existingItem = existingRundown.Items.FirstOrDefault(i => i.UUID == itemDetailDto.ItemId);
            if (existingItem == null)
            {
                return NotFound("Item ikke fundet.");
            }

            // Brug GetModel() til at oprette den korrekte itemDetail instans baseret på typen
            var itemDetail = _itemDetailService.GetModel(itemDetailDto.Type);
            if (itemDetail == null)
            {
                return BadRequest("Kunne ikke oprette item detail baseret på den angivne type.");
            }

            // Sæt værdierne på den nye item detail
            itemDetail.UUID = Guid.NewGuid();
            itemDetail.Title = itemDetailDto.Title;
            itemDetail.Duration = TimeSpan.Parse(itemDetailDto.Duration);
            itemDetail.ItemId = itemDetailDto.ItemId;
            itemDetail.Type = itemDetailDto.Type;
            itemDetail.Order = itemDetailDto.Order;

            // Tilpas værdier afhængigt af den specifikke type af item detail
            switch (itemDetail)
            {
                case ItemDetailVideo video when itemDetailDto.VideoPath != null:
                    video.VideoPath = itemDetailDto.VideoPath;
                    break;
                case ItemDetailTeleprompter teleprompter when itemDetailDto.PrompterText != null:
                    teleprompter.PrompterText = itemDetailDto.PrompterText;
                    break;
                case ItemDetailGraphic graphic when itemDetailDto.GraphicId != null:
                    graphic.GraphicId = itemDetailDto.GraphicId;
                    break;
                case ItemDetailComment comment when itemDetailDto.Comment != null:
                    comment.Comment = itemDetailDto.Comment;
                    break;
            }

            // Tilføj itemDetail til existingItem's detaljer
            existingItem.Details.Add(itemDetail);

            await _itemDetailService.CreateItemDetailAsync(existingRundown.UUID, existingItem);
            return Ok(existingRundown);
        }


        [HttpPut("edit-item-detail-in-rundown/{rundownId:guid}")]
        public async Task<IActionResult> EditItemDetailInRundown(Guid rundownId, [FromBody] DetailDTO detailDto)
        {
            // Hent det eksisterende rundown baseret på ID
            var existingRundown = await _rundownService.GetRundownByIdAsync(rundownId);
            if (existingRundown == null)
            {
                return NotFound("Rundown ikke fundet.");
            }
            // Find det item, som har den detail, der skal opdateres
            var existingItem = existingRundown.Items.FirstOrDefault(i => i.Details.Any(d => d.UUID == detailDto.UUID));
            if (existingItem == null)
            {
                return NotFound("Item der indeholder detail ikke fundet.");
            }
            // Find den specifikke detail i itemet og opdater den
            var existingDetail = existingItem.Details.FirstOrDefault(d => d.UUID == detailDto.UUID);
            if (existingDetail == null)
            {
                return NotFound($"ItemDetail med UUID {detailDto.UUID} blev ikke fundet.");
            }
            // Opdater de generelle felter for det specifikke detail
            existingDetail.Title = detailDto.Title;
            existingDetail.Duration = TimeSpan.Parse(detailDto.Duration);
            existingDetail.Type = detailDto.Type;
            existingDetail.Order = detailDto.Order;

            // Switch for at håndtere opdateringen af felter afhængig af typen af detail
            switch (existingDetail)
            {
                case ItemDetailVideo video when detailDto.VideoPath != null:
                    video.VideoPath = detailDto.VideoPath;
                    break;
                case ItemDetailTeleprompter teleprompter when detailDto.PrompterText != null:
                    teleprompter.PrompterText = detailDto.PrompterText;
                    break;
                case ItemDetailGraphic graphic when detailDto.GraphicId != null:
                    graphic.GraphicId = detailDto.GraphicId;
                    break;
                case ItemDetailComment comment when detailDto.Comment != null:
                    comment.Comment = detailDto.Comment;
                    break;
            }

            // Gem ændringerne
            await _itemDetailService.CreateItemDetailAsync(existingRundown.UUID, existingItem);

            return Ok(existingRundown);
        }



        [HttpDelete("delete-item-detail-from-rundown/{rundownId:guid}/{detailId:guid}")]
        public async Task<IActionResult> DeleteItemDetailFromRundown(Guid rundownId, Guid detailId)
        {
            // Hent eksisterende rundown baseret på ID
            var existingRundown = await _rundownService.GetRundownByIdAsync(rundownId);
            if (existingRundown == null)
            {
                return NotFound("Rundown ikke fundet.");
            }

            // Gennemgå items i rundown
            var existingItem = existingRundown.Items.FirstOrDefault(i => i.Details.Any(d => d.UUID == detailId));
            if (existingItem == null)
            {
                return NotFound("Item med det ønskede ItemDetail ikke fundet.");
            }

            // Find og fjern detail baseret på detailId (UUID)
            var detailToRemove = existingItem.Details.FirstOrDefault(d => d.UUID == detailId);
            if (detailToRemove == null)
            {
                return NotFound("ItemDetail ikke fundet.");
            }

            // Fjern det fundne detail
            existingItem.Details.Remove(detailToRemove);

            // Opdater rundown i databasen
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
