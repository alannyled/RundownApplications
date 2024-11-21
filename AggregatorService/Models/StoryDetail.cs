using System.Text.Json.Serialization;

namespace AggregatorService.Models
{

    public class StoryDetail
    {
        [JsonPropertyName("uuid")] 
        public Guid UUID { get; set; }

        [JsonPropertyName("storyId")]
        public Guid StoryId { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("title")]
        public string title { get; set; }

        [JsonPropertyName("duration")]
        public TimeSpan Duration { get; set; }
    }

  
    public class ItemDetailTeleprompter : StoryDetail
    {
        [JsonPropertyName("prompterText")]
        public string? PrompterText { get; set; }

        [JsonPropertyName("detailType")]
        public string DetailType { get; set; } = "ItemDetailTeleprompter";
    }


    public class ItemDetailVideo : StoryDetail
    {

        [JsonPropertyName("videoPath")]
        public string? VideoPath { get; set; }

        [JsonPropertyName("detailType")]
        public string DetailType { get; set; } = "ItemDetailVideo";
    }


    public class ItemDetailGraphic : StoryDetail
    {

        [JsonPropertyName("graphicId")]
        public string? GraphicId { get; set; }

        [JsonPropertyName("detailType")]
        public string DetailType { get; set; } = "ItemDetailGraphic";
    }


    public class ItemDetailComment : StoryDetail
    {
        [JsonPropertyName("comment")]
        public string? Comment { get; set; }

        [JsonPropertyName("detailType")]
        public string DetailType { get; set; } = "ItemDetailComment";

    }
}
