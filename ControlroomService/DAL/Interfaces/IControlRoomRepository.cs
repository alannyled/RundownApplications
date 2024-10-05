using ControlroomService.Models;

namespace ControlroomService.DAL.Interfaces
{
    public interface IControlRoomRepository
    {
        Task<List<ControlRoom>> GetAsync();
        Task<ControlRoom?> GetByIdAsync(string id);
        Task CreateAsync(ControlRoom newControlRoom);
        Task UpdateAsync(string id, ControlRoom updatedControlRoom);
        Task RemoveAsync(string id);
        Task RemoveAllAsync();
    }
}
