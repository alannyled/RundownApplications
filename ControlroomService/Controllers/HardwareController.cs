using ControlroomService.BLL.Interfaces;
using ControlroomService.DTO;
using ControlroomService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControlroomService.Controllers
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
        public async Task<List<Hardware>> Get() => await _hardwareService.GetAllHardwareAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Hardware>> Get(string id)
        {
            var hardware = await _hardwareService.GetHardwareByIdAsync(id);

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
                Vendor = newHardwareDto.Vendor,  
                Model = newHardwareDto.Model,
                MacAddress = newHardwareDto.MacAddress,
                IpAddress = newHardwareDto.IpAddress,
                Port = newHardwareDto.Port,
                CreatedDate = DateTime.Now
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

            return NoContent();
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

