using Microsoft.AspNetCore.Mvc;
using RundownDbService.BLL.Interfaces;
using RundownDbService.BLL.Services;
using RundownDbService.Models;

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
        public async Task<ActionResult> Update(Guid id, Rundown updatedRundown)
        {
            var rundown = await _rundownService.GetRundownByIdAsync(id);
            if (rundown == null)
            {
                return NotFound();
            }

            await _rundownService.UpdateRundownAsync(id, updatedRundown);
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
}
