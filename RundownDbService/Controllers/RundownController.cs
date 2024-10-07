using Microsoft.AspNetCore.Mvc;
using RundownDbService.BLL.Interfaces;
using RundownDbService.DTO;
using RundownDbService.Models;
using System.Text.Json.Serialization;

namespace RundownDbService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RundownController : ControllerBase
    {
        private readonly IRundownService _rundownService;

        public RundownController(IRundownService rundownService)
        {
            _rundownService = rundownService;
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
            // Valider at controlRoomId er til stede og er en gyldig GUID
            if (dto == null || string.IsNullOrEmpty(dto.ControlRoomId) || !Guid.TryParse(dto.ControlRoomId, out var parsedControlRoomId))
            {
                return BadRequest("A valid controlRoomId is required.");
            }

            Console.WriteLine("Received PUT request for Rundown ID: " + id);
            Console.WriteLine("ControlRoomId from DTO: " + dto.ControlRoomId);

            // Hent den eksisterende rundown
            var rundown = await _rundownService.GetRundownByIdAsync(id);
            if (rundown == null)
            {
                return NotFound();
            }

            // Opdater controlRoomId i rundown objektet
            rundown.ControlRoomId = parsedControlRoomId;
            await _rundownService.UpdateRundownAsync(id, rundown);

            return NoContent();
        }


        [HttpPut("add-item-to-rundown/{id:guid}")]
        public async Task<ActionResult> AddItemToRundown(Guid id, [FromBody] RundownDTO rundownDto)
        {
            if (rundownDto == null)
            {
                return BadRequest("The RundownDTO field is required.");
            }

            // Hent eksisterende rundown baseret på ID
            var existingRundown = await _rundownService.GetRundownByIdAsync(id);
            if (existingRundown == null)
            {
                return NotFound();
            }

            // Tilføj alle nye items fra rundownDto til eksisterende rundown
            existingRundown.Items.AddRange(rundownDto.Items.Select(item => new RundownItem
            {
                UUID = Guid.NewGuid(),
                RundownId = id,
                Name = item.Name,
                Duration = TimeSpan.Parse(item.Duration),
                Order = item.Order
            }));

            // Opdater rundown i databasen
            await _rundownService.UpdateRundownAsync(id, existingRundown);

            return NoContent();
        }






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
    //public class ControlRoomUpdateRequest
    //{
    //    [JsonPropertyName("controlRoomId")]
    //    public string ControlRoomId { get; set; }
    //}

}
