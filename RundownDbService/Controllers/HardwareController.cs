using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RundownDbService.Data;

namespace RundownDbService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HardwareApiController : ControllerBase
    {
        private readonly RundownDbContext _context;

        public HardwareApiController(RundownDbContext context)
        {
            _context = context;
        }

        // GET: api/Hardware
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hardware>>> GetHardware()
        {
            return await _context.Hardwares.ToListAsync();
        }

        // GET: api/Hardware/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hardware>> GetHardware(int id)
        {
            var hardware = await _context.Hardwares.FindAsync(id);

            if (hardware == null)
            {
                return NotFound();
            }

            return hardware;
        }

        // PUT: api/Hardware/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHardware(int id, Hardware hardware)
        {
            if (id != hardware.HardwareId)
            {
                return BadRequest();
            }

            _context.Entry(hardware).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HardwareExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Hardware
        [HttpPost]
        public async Task<ActionResult<Hardware>> PostHardware(Hardware hardware)
        {
            _context.Hardwares.Add(hardware);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHardware), new { id = hardware.HardwareId }, hardware);
        }

        // DELETE: api/Hardware/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHardware(int id)
        {
            var hardware = await _context.Hardwares.FindAsync(id);
            if (hardware == null)
            {
                return NotFound();
            }

            _context.Hardwares.Remove(hardware);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HardwareExists(int id)
        {
            return _context.Hardwares.Any(e => e.HardwareId == id);
        }
    }
}
