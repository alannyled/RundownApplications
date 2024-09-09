using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RundownDbService.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RundownDbService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemplatesController : ControllerBase // Use ControllerBase for API controllers without views
    {
        private readonly RundownDbContext _context;

        public TemplatesController(RundownDbContext context)
        {
            _context = context;
        }

        // GET: api/Templates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Template>>> Index()
        {
            return Ok(await _context.Templates.ToListAsync()); // Return Ok() with data for API
        }

        // GET: api/Templates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Template>> Details(int id)
        {
            var template = await _context.Templates.FirstOrDefaultAsync(m => m.Id == id);

            if (template == null)
            {
                return NotFound(); // Return NotFound() for API
            }

            return Ok(template); // Return Ok() with data
        }

        // POST: api/Templates
        [HttpPost]
        public async Task<ActionResult<Template>> Create([Bind("Id,Name")] Template template)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return BadRequest if model state is invalid
            }

            _context.Add(template);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Details), new { id = template.Id }, template); // Return CreatedAtAction for the new resource
        }

        // PUT: api/Templates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Template template)
        {
            if (id != template.Id)
            {
                return BadRequest();
            }

            _context.Entry(template).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TemplateExists(template.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Return NoContent for successful updates
        }

        // DELETE: api/Templates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var template = await _context.Templates.FindAsync(id);
            if (template == null)
            {
                return NotFound();
            }

            _context.Templates.Remove(template);
            await _context.SaveChangesAsync();

            return NoContent(); // Return NoContent for successful deletion
        }

        // Helper method to check if a template exists
        private bool TemplateExists(int id)
        {
            return _context.Templates.Any(e => e.Id == id);
        }
    }
}
