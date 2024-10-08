using Microsoft.AspNetCore.Mvc;
using TemplateDbService.BLL.Services;
using TemplateDbService.DAL.Interfaces;
using TemplateDbService.DAL.Repositories;
using TemplateDbService.Models;

namespace TemplateDbService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemTemplateController : ControllerBase
    {
        private readonly IItemTemplateRepository _service;

        public ItemTemplateController(IItemTemplateRepository service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ItemTemplate>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{uuid}")]
        public async Task<ActionResult<ItemTemplate>> GetById(Guid uuid)
        {
            var item = await _service.GetByIdAsync(uuid);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ItemTemplate itemTemplate)
        {
            await _service.CreateAsync(itemTemplate);
            return CreatedAtAction(nameof(GetById), new { uuid = itemTemplate.UUID }, itemTemplate);
        }

        [HttpPut("{uuid}")]
        public async Task<ActionResult> Update(Guid uuid, ItemTemplate itemTemplate)
        {
            var existingItem = await _service.GetByIdAsync(uuid);
            if (existingItem == null)
            {
                return NotFound();
            }

            itemTemplate.UUID = uuid;
            await _service.UpdateAsync(uuid, itemTemplate);
            return NoContent();
        }

        [HttpDelete("{uuid}")]
        public async Task<ActionResult> Delete(Guid uuid)
        {
            var item = await _service.GetByIdAsync(uuid);
            if (item == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(uuid);
            return NoContent();
        }
    }
}
