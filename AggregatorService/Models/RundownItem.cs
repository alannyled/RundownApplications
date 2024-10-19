using System.Text.Json.Serialization;

namespace AggregatorService.Models
{
    public class RundownItem
    {
        [JsonPropertyName("uuid")]
        public Guid UUID { get; set; }

        [JsonPropertyName("rundownId")]
        public Guid RundownId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("duration")]
        public string Duration { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("details")]
        public List<ItemDetail> Details { get; set; } = new List<ItemDetail>();


    }
}
