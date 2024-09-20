using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RundownDbService.Data;

namespace RundownDbService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControlRoomsApiController : ControllerBase
    {
        private readonly RundownDbContext _context;

        public ControlRoomsApiController(RundownDbContext context)
        {
            _context = context;
        }

        // GET: api/ControlRooms
        [HttpGet]
        public async Task<IActionResult> GetControlRooms()
        {
            return Ok(await _context.ControlRooms.ToListAsync());
        }

        // GET: api/ControlRooms/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetControlRoom(int id)
        {
            var controlRoom = await _context.ControlRooms.FindAsync(id);

            if (controlRoom == null)
            {
                return NotFound();
            }

            return Ok(controlRoom);
        }

        // POST: api/ControlRooms
        [HttpPost]
        public async Task<IActionResult> CreateControlRoom([FromBody] ControlRoom controlRoom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ControlRooms.Add(controlRoom);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetControlRoom), new { id = controlRoom.Id }, controlRoom);
        }

        // DELETE: api/ControlRooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteControlRoom(int id)
        {
            var controlRoom = await _context.ControlRooms.FindAsync(id);
            if (controlRoom == null)
            {
                return NotFound();
            }

            _context.ControlRooms.Remove(controlRoom);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
