using ControlRoomDbService.Models;

namespace ControlRoomDbService.DAL.Interfaces
{
    public interface IControlRoomRepository
    {
        Task<List<ControlRoom>> GetAsync();
        Task<ControlRoom?> GetByIdAsync(string id);
        Task CreateAsync(ControlRoom newControlRoom);
        Task<List<ControlRoom>> UpdateAsync(string id, ControlRoom updatedControlRoom);
        Task RemoveAsync(string id);
        Task RemoveAllAsync();
    }
}
