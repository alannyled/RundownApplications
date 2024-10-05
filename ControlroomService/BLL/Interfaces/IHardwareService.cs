using ControlroomService.Models;

namespace ControlroomService.BLL.Interfaces
{
    public interface IHardwareService
    {
        Task<List<Hardware>> GetAllHardwareAsync();
        Task<Hardware> GetHardwareByIdAsync(string id);
        Task CreateHardwareAsync(Hardware hardware);
        Task UpdateHardwareAsync(string id, Hardware updatedHardware);
        Task DeleteHardwareAsync(string id);
    }
}
