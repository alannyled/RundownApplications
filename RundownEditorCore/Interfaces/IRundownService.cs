using RundownEditorCore.DTO;

namespace RundownEditorCore.Interfaces
{
    public interface IRundownService
    {
        Task<List<RundownDTO>> GetActiveRundowsAsync();
        Task<RundownDTO> GetRundownAsync(string uuid);
        Task<RundownDTO> CreateRundownFromTemplate(string templateId, string controlroonId, DateTimeOffset date);
        Task<RundownDTO> UpdateRundownControlRoomAsync(string uuid, string controlRoomId);
        Task<RundownDTO> AddDetailToItemAsync(string rundownId, ItemDetailDTO.ItemDetail itemDetail);
        Task<RundownDTO> UpdateDetailAsync(string rundownId, DetailDTO itemDetail);
        Task<RundownDTO> AddItemToRundownAsync(string rundownId, RundownItemDTO item);
    }
}
