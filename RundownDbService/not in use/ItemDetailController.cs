//using Microsoft.AspNetCore.Mvc;
//using RundownDbService.BLL.Interfaces;
//using RundownDbService.Models;

//namespace RundownDbService.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ItemDetailController : ControllerBase
//    {
//        private readonly IItemDetailService _itemDetailService;

//        public ItemDetailController(IItemDetailService itemDetailService)
//        {
//            _itemDetailService = itemDetailService;
//        }

//        [HttpGet]
//        public async Task<ActionResult<List<ItemDetail>>> GetAll()
//        {
//            var details = await _itemDetailService.GetAllItemDetailsAsync();
//            return Ok(details);
//        }

//        [HttpGet("{id:guid}")]
//        public async Task<ActionResult<ItemDetail>> GetById(Guid id)
//        {
//            var detail = await _itemDetailService.GetItemDetailByIdAsync(id);
//            if (detail == null)
//            {
//                return NotFound();
//            }
//            return Ok(detail);
//        }

//        [HttpPost]
//        public async Task<ActionResult> Create(ItemDetail newItemDetail)
//        {
//            newItemDetail.UUID = Guid.NewGuid();

//            await _itemDetailService.CreateItemDetailAsync(newItemDetail);
//            return CreatedAtAction(nameof(GetById), new { id = newItemDetail.UUID }, newItemDetail);
//        }

//        [HttpPut("{id:guid}")]
//        public async Task<ActionResult> Update(Guid id, ItemDetail updatedItemDetail)
//        {
//            var detail = await _itemDetailService.GetItemDetailByIdAsync(id);
//            if (detail == null)
//            {
//                return NotFound();
//            }

//            await _itemDetailService.UpdateItemDetailAsync(id, updatedItemDetail);
//            return NoContent();
//        }

//        [HttpDelete("{id:guid}")]
//        public async Task<ActionResult> Delete(Guid id)
//        {
//            var detail = await _itemDetailService.GetItemDetailByIdAsync(id);
//            if (detail == null)
//            {
//                return NotFound();
//            }

//            await _itemDetailService.DeleteItemDetailAsync(id);
//            return NoContent();
//        }
//    }
//}
