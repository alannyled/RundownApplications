using ControlroomService.Models;

namespace ControlroomService.DAL.Interfaces
{
    public interface IHardwareRepository
    {
        Task<List<Hardware>> GetAsync();
        Task<Hardware?> GetByIdAsync(string id);
        Task CreateAsync(Hardware newHardware);
        Task UpdateAsync(string id, Hardware updatedHardware);
        Task RemoveAsync(string id);

    }
}
