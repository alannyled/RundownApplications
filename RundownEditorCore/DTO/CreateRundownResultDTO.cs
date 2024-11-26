using CommonClassLibrary.DTO;

namespace RundownEditorCore.DTO
{
    public class CreateRundownResultDTO
    {
        public List<RundownDTO> ActiveRundowns { get; set; } = [];
        public RundownDTO SelectedRundown { get; set; } = new();
    }
}
