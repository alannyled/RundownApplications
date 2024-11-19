using ControlRoomDbService.BLL.Interfaces;
using ControlRoomDbService.DTO;
using ControlRoomDbService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControlRoomDbService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HardwareController : ControllerBase
    {
        private readonly IHardwareService _hardwareService;

        public HardwareController(IHardwareService hardwareService)
        {
            _hardwareService = hardwareService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Hardware>>> Get()
        {
           var hardwares =  await _hardwareService.GetAllHardwareAsync();
            return Ok(hardwares);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Hardware>> Get(string id)
        {
            var hardware = await _hardwareService.GetHardwareByIdAsync(id);

            if (hardware == null)
            {
                return NotFound();
            }

            return Ok(hardware);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateHardwareDto newHardwareDto)
        {
            if (newHardwareDto == null)
            {
                return BadRequest("ControlRoomId cannot be null.");
            }

            var newHardware = new Hardware
            {
                ControlRoomId = newHardwareDto.ControlRoomId ?? string.Empty,
                Name = newHardwareDto.Name ?? string.Empty,
                Vendor = newHardwareDto.Vendor ?? string.Empty,
                Model = newHardwareDto.Model ?? string.Empty,
                MacAddress = newHardwareDto.MacAddress ?? string.Empty,
                IpAddress = newHardwareDto.IpAddress ?? string.Empty,
                Port = newHardwareDto.Port,
                CreatedDate = DateTime.Now,
                ArchivedDate = null,
                ArchivedBy = null
            };

            await _hardwareService.CreateHardwareAsync(newHardware);
            return CreatedAtAction(nameof(Get), new { id = newHardware.UUID }, newHardware);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Hardware updatedHardware)
        {
            var existingHardware = await _hardwareService.GetHardwareByIdAsync(id);

            if (existingHardware == null)
            {
                return NotFound();
            }

            updatedHardware.UUID = existingHardware.UUID;
            await _hardwareService.UpdateHardwareAsync(id, updatedHardware);

            return Ok(updatedHardware);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var hardware = await _hardwareService.GetHardwareByIdAsync(id);

            if (hardware == null)
            {
                return NotFound();
            }

            await _hardwareService.DeleteHardwareAsync(id);
            return NoContent();
        }
    }

}

