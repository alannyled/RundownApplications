using RundownEditorCore.DTO;
using CommonClassLibrary.DTO;

namespace RundownEditorCore.Interfaces
{
    public interface IRundownService
    {
        Task<List<RundownDTO>> GetRundownsAsync();
        Task<RundownDTO?> GetRundownAsync(string uuid);
        Task<RundownDTO?> CreateRundownFromTemplate(string templateId, string controlroonId, DateTimeOffset date);
        Task<RundownDTO?> UpdateRundownControlRoomAsync(string uuid, string controlRoomId);
        Task<RundownDTO?> AddDetailToStoryAsync(string rundownId, StoryDetailDTO.StoryDetail StoryDetail);
        Task<RundownDTO?> UpdateDetailAsync(string rundownId, DetailDTO StoryDetail);
        Task<RundownDTO?> AddStoryToRundownAsync(string rundownId, RundownStoryDTO Story);
        Task<RundownDTO?> UpdateRundownAsync(string rundownId, RundownDTO rundown);
    }
}
