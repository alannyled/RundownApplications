using RundownEditorCore.DTO;

namespace RundownEditorCore.Interfaces
{
    public interface IRundownService
    {
        Task<List<RundownDTO>> GetActiveRundowsAsync();
        Task<RundownDTO> GetRundownAsync(string uuid);
        Task<RundownDTO> CreateRundownFromTemplate(string templateId, string controlroonId, DateTime date);
        Task UpdateRundownControlRoomAsync(string uuid, string controlRoomId);

        Task AddItemToRundownAsync(string rundownId, RundownItemDTO item);
    }
}
