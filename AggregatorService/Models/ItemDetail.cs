using System.Text.Json.Serialization;

namespace AggregatorService.Models
{

    public class ItemDetail
    {
        [JsonPropertyName("uuid")] 
        public Guid UUID { get; set; }

        [JsonPropertyName("itemId")]
        public Guid ItemId { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }
    }

  
    public class ItemDetailTeleprompter : ItemDetail
    {
        [JsonPropertyName("prompterText")]
        public string PrompterText { get; set; }

        [JsonPropertyName("detailType")]
        public string DetailType { get; set; } = "ItemDetailTeleprompter";
    }


    public class ItemDetailVideo : ItemDetail
    {
        [JsonPropertyName("title")]
        public string title { get; set; }

        [JsonPropertyName("videoPath")]
        public string VideoPath { get; set; }

        [JsonPropertyName("duration")]
        public TimeSpan Duration { get; set; }

        [JsonPropertyName("detailType")]
        public string DetailType { get; set; } = "ItemDetailVideo";
    }


    public class ItemDetailGraphic : ItemDetail
    {
        [JsonPropertyName("graphicId")]
        public string GraphicId { get; set; }

        [JsonPropertyName("duration")]
        public TimeSpan Duration { get; set; }

        [JsonPropertyName("detailType")]
        public string DetailType { get; set; } = "ItemDetailGraphic";
    }


    public class ItemDetailComment : ItemDetail
    {
        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("detailType")]
        public string DetailType { get; set; } = "ItemDetailComment";

    }
}
