using System;
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
    public class TeleprompterTestsApiController : Controller
    {
        private readonly RundownDbContext _context;

        public TeleprompterTestsApiController(RundownDbContext context)
        {
            _context = context;
        }

        // GET: api/teleprompter/TeleprompterTests
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.TeleprompterTests.ToListAsync());  // Use Ok for API responses
        }

        // GET: api/teleprompter/TeleprompterTests/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teleprompterTest = await _context.TeleprompterTests
                .FirstOrDefaultAsync(m => m.TeleprompterTestId == id);
            if (teleprompterTest == null)
            {
                return NotFound();
            }

            return Ok(teleprompterTest);  // Use Ok for API responses
        }

        // POST: api/teleprompter/TeleprompterTests/Create
        [HttpPost("create")]
        public async Task<IActionResult> Create([Bind("TeleprompterTestId,Text")] TeleprompterTest teleprompterTest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teleprompterTest);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);  // Return BadRequest if the model state is invalid
        }

        // PUT: api/teleprompter/TeleprompterTests/Edit/5
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("TeleprompterTestId,Text")] TeleprompterTest teleprompterTest)
        {
            if (id != teleprompterTest.TeleprompterTestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teleprompterTest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeleprompterTestExists(teleprompterTest.TeleprompterTestId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return BadRequest(ModelState);  // Return BadRequest if the model state is invalid
        }

        // DELETE: api/teleprompter/TeleprompterTests/Delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teleprompterTest = await _context.TeleprompterTests
                .FirstOrDefaultAsync(m => m.TeleprompterTestId == id);
            if (teleprompterTest == null)
            {
                return NotFound();
            }

            return Ok(teleprompterTest);
        }

        // POST: api/teleprompter/TeleprompterTests/DeleteConfirmed/5
        [HttpPost("delete-confirmed/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teleprompterTest = await _context.TeleprompterTests.FindAsync(id);
            if (teleprompterTest != null)
            {
                _context.TeleprompterTests.Remove(teleprompterTest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeleprompterTestExists(int id)
        {
            return _context.TeleprompterTests.Any(e => e.TeleprompterTestId == id);
        }
    }
}
