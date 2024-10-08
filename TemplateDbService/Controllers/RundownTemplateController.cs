using Microsoft.AspNetCore.Mvc;
using TemplateDbService.Models;
using TemplateDbService.BLL.Services;

namespace TemplateDbService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RundownTemplateController : ControllerBase
    {
        private readonly RundownTemplateService _service;

        public RundownTemplateController(RundownTemplateService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<RundownTemplate>>> GetAll()
        {
            var templates = await _service.GetAllAsync();
            return Ok(templates);
        }

        [HttpGet("{uuid}")]
        public async Task<ActionResult<RundownTemplate>> GetById(Guid uuid)
        {
            var template = await _service.GetByIdAsync(uuid);
            if (template == null)
            {
                return NotFound();
            }
            return Ok(template);
        }

        [HttpPost]
        public async Task<ActionResult> Create(RundownTemplate template)
        {
            await _service.CreateAsync(template);
            return CreatedAtAction(nameof(GetById), new { uuid = template.UUID }, template);
        }

        [HttpPut("{uuid}")]
        public async Task<ActionResult> Update(Guid uuid, RundownTemplate template)
        {
            var existingTemplate = await _service.GetByIdAsync(uuid);
            if (existingTemplate == null)
            {
                return NotFound();
            }

            template.UUID = uuid;
            await _service.UpdateAsync(uuid, template);
            return NoContent();
        }

        [HttpDelete("{uuid}")]
        public async Task<ActionResult> Delete(Guid uuid)
        {
            var template = await _service.GetByIdAsync(uuid);
            if (template == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(uuid);
            return NoContent();
        }
    }
}

