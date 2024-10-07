using RundownEditorCore.DTO;

namespace RundownEditorCore.Interfaces
{
    public interface IRundownService
    {
        Task<List<RundownDTO>> GetActiveRundowsAsync();
        Task<RundownDTO> GetRundownAsync(string uuid);
        //Task<RundownDTO> UpdateRundownControlRoomAsync(string uuid, string controlRoomId);
        Task UpdateRundownControlRoomAsync(string uuid, string controlRoomId);

        Task AddItemToRundownAsync(string rundownId, RundownItemDTO item);
    }
}
