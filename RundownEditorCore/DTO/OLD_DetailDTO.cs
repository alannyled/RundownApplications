using System.Text.Json.Serialization;

namespace RundownEditorCore.DTO
{
    public class OLD_DetailDTO
    {
        //[JsonPropertyName("uuid")]
        public Guid UUID { get; set; } = Guid.NewGuid();

      //  [JsonPropertyName("itemId")]
        public Guid ItemId { get; set; }

       // [JsonPropertyName("title")]
        public string Title { get; set; }

      //  [JsonPropertyName("type")]
        public string Type { get; set; }

      //  [JsonPropertyName("order")]
        public int Order { get; set; }

      //  [JsonPropertyName("duration")]
        public string Duration { get; set; }

     //   [JsonPropertyName("prompterText")]
        public string? PrompterText { get; set; } = null;

      //  [JsonPropertyName("videoPath")]
        public string? VideoPath { get; set; } = null;

      //  [JsonPropertyName("graphicId")]
        public string? GraphicId { get; set; } = null;

      //  [JsonPropertyName("comment")]
        public string? Comment { get; set; } = null;
    }
}
