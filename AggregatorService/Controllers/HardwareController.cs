using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using AggregatorService.Managers;
using AggregatorService.Models;

namespace AggregatorService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HardwareController(HardwareManager hardwareManager) : ControllerBase
    {
        private readonly HardwareManager _hardwareManager = hardwareManager;

        [HttpPost("create-hardware")]
        public async Task<IActionResult> Create([FromBody] Hardware hardware)
        {
            if (hardware == null)
                return BadRequest("Hardware is null.");

            hardware.Uuid = Guid.NewGuid();
            await _hardwareManager.CreateHardwareAsync(hardware);
            return Ok(hardware);
        }

        //[HttpGet("{uuid}")]
        //public async Task<IActionResult> Get(Guid uuid)
        //{
        //    var hardware = await _hardwareManager.Find(h => h.Uuid == uuid).FirstOrDefaultAsync();
        //    if (hardware == null)
        //        return NotFound();

        //    return Ok(hardware);
        //}

        [HttpPut("update-hardware/{uuid}")]
        public async Task<IActionResult> Update(string uuid, [FromBody] Hardware updatedHardware)
        {
            if (updatedHardware == null)
                return BadRequest("Updated hardware is null.");

            var hardware = await _hardwareManager.UpdateHardwareAsync(uuid, updatedHardware);
            if (hardware == null)
                return NotFound();
            return Ok(updatedHardware);
        }

        [HttpDelete("delete-hardware/{uuid}")]
        public async Task<IActionResult> Delete(string uuid)
        {
            await _hardwareManager.DeleteHardwareAsync(uuid);
            return NoContent();
        }
    }
}