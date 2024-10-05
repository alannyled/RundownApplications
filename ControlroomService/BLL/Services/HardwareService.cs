using ControlroomService.BLL.Interfaces;
using ControlroomService.DAL.Interfaces;
using ControlroomService.Models;

namespace ControlroomService.BLL.Services
{
    public class HardwareService : IHardwareService
    {
        private readonly IHardwareRepository _hardwareRepository;

        public HardwareService(IHardwareRepository hardwareRepository)
        {
            _hardwareRepository = hardwareRepository;
        }

        public async Task<List<Hardware>> GetAllHardwareAsync()
        {
            return await _hardwareRepository.GetAsync();
        }

        public async Task<Hardware> GetHardwareByIdAsync(string id)
        {
            return await _hardwareRepository.GetByIdAsync(id);
        }

        public async Task CreateHardwareAsync(Hardware hardware)
        {
            await _hardwareRepository.CreateAsync(hardware);
        }

        public async Task UpdateHardwareAsync(string id, Hardware updatedHardware)
        {
            await _hardwareRepository.UpdateAsync(id, updatedHardware);
        }

        public async Task DeleteHardwareAsync(string id)
        {
            await _hardwareRepository.RemoveAsync(id);
        }
    }

}
