using RundownEditorCore.DTO;

namespace RundownEditorCore.Interfaces
{
    public interface IControlRoomService
    {
        Task<List<ControlRoomDTO>> GetControlRoomsAsync();
        Task<ControlRoomDTO> CreateControlRoomAsync(ControlRoomDTO newControlRoom);
        Task<ControlRoomDTO> UpdateControlRoomAsync(string controlRoomId, ControlRoomDTO updatedControlRoom);
        Task<ControlRoomDTO> DeleteControlRoomAsync(string controlRoomId);
    }
}
