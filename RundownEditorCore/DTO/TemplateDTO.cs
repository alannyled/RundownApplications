using System.Text.Json.Serialization;

namespace RundownEditorCore.DTO
{
    public class TemplateDTO
    {
       // [JsonPropertyName("uuid")]
        public Guid Uuid { get; set; }

      //  [JsonPropertyName("name")]
        public string Name { get; set; }

      //  [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; }
    }
}
