using ControlroomService.Models;

namespace ControlroomService.BLL.Interfaces
{
    public interface IControlRoomService
    {
        Task<List<ControlRoom>> GetControlRoomsAsync();
        Task<ControlRoom> GetControlRoomByIdAsync(string id);
        Task CreateControlRoomAsync(ControlRoom controlRoom);
        Task UpdateControlRoomAsync(string id, ControlRoom updatedControlRoom);
        Task DeleteControlRoomAsync(string id);
        Task DeleteAllControlRoomsAsync();
    }
}
