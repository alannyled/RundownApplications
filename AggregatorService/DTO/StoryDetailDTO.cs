using System.Text.Json.Serialization;

namespace AggregatorService.DTO
{
    public class StoryDetailDTO
    {
        [JsonPropertyName("uuid")]
        public Guid UUID { get; set; } = Guid.NewGuid();

        [JsonPropertyName("storyId")]
        public Guid StoryId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("duration")]
        public string Duration { get; set; } = string.Empty;

        [JsonPropertyName("prompterText")]
        public string? PrompterText { get; set; }

        [JsonPropertyName("videoPath")]
        public string? VideoPath { get; set; }

        [JsonPropertyName("graphicId")]
        public string? GraphicId { get; set; }

        [JsonPropertyName("comment")]
        public string? Comment { get; set; }
    }
}
