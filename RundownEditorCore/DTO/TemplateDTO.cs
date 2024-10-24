using System.Text.Json.Serialization;

namespace RundownEditorCore.DTO
{
    public class TemplateDTO
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
