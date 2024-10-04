using ControlRoomService.DAL;
using ControlRoomService.DTO;
using ControlRoomService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControlRoomService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HardwareController : ControllerBase
    {
        private readonly HardwareRepository _hardwareService;

        public HardwareController(HardwareRepository hardwareService)
        {
            _hardwareService = hardwareService;
        }

        [HttpGet]
        public async Task<List<Hardware>> Get() => await _hardwareService.GetAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Hardware>> Get(string id)
        {
            var hardware = await _hardwareService.GetByIdAsync(id);

            if (hardware == null)
            {
                return NotFound();
            }

            return hardware;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateHardwareDto newHardwareDto)
        {
            var newHardware = new Hardware
            {
                ControlRoomId = newHardwareDto.ControlRoomId,
                Name = newHardwareDto.Name,
                Model = newHardwareDto.Model,
                MacAddress = newHardwareDto.MacAddress,
                IpAddress = newHardwareDto.IpAddress,
                Port = newHardwareDto.Port,
                CreatedDate = DateTime.Now
            };

            await _hardwareService.CreateAsync(newHardware);
            return CreatedAtAction(nameof(Get), new { id = newHardware.UUID }, newHardware);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Hardware updatedHardware)
        {
            var existingHardware = await _hardwareService.GetByIdAsync(id);

            if (existingHardware == null)
            {
                return NotFound();
            }

            updatedHardware.UUID = existingHardware.UUID;
            await _hardwareService.UpdateAsync(id, updatedHardware);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var hardware = await _hardwareService.GetByIdAsync(id);

            if (hardware == null)
            {
                return NotFound();
            }

            await _hardwareService.RemoveAsync(id);
            return NoContent();
        }
    }
}

