using Microsoft.AspNetCore.Mvc;
using RundownDbService.BLL.Interfaces;
using RundownDbService.Models;

namespace RundownDbService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RundownItemController : ControllerBase
    {
        private readonly IRundownItemService _rundownItemService;

        public RundownItemController(IRundownItemService rundownItemService)
        {
            _rundownItemService = rundownItemService;
        }

        [HttpGet]
        public async Task<ActionResult<List<RundownItem>>> GetAll()
        {
            var items = await _rundownItemService.GetAllRundownItemsAsync();
            return Ok(items);
        }

        //[HttpGet("/rundown/{id:guid}")]
        //public async Task<ActionResult<List<RundownItem>>> GetRundownItems(Guid id)
        //{
        //    var items = await _rundownItemService.GetRundownItemsAsync(id);
        //    return Ok(items);
        //}

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RundownItem>> GetById(Guid id)
        {
            var item = await _rundownItemService.GetRundownItemByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult> Create(RundownItem newItem)
        {
            newItem.UUID = Guid.NewGuid();
            await _rundownItemService.CreateRundownItemAsync(newItem);
            return CreatedAtAction(nameof(GetById), new { id = newItem.UUID }, newItem);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, RundownItem updatedItem)
        {
            var item = await _rundownItemService.GetRundownItemByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            await _rundownItemService.UpdateRundownItemAsync(id, updatedItem);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var item = await _rundownItemService.GetRundownItemByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            await _rundownItemService.DeleteRundownItemAsync(id);
            return NoContent();
        }
    }
}
