using RundownEditorCore.DTO;

namespace RundownEditorCore.Interfaces
{
    public interface ITemplateService
    {
        Task<List<RundownDTO>> GetAllTemplatesAsync();
    }
}
