using System.Text.Json.Serialization;

namespace AggregatorService.DTO
{
    public class RundownItemDTO
    {
        [JsonPropertyName("uuid")]
        public Guid UUID { get; set; }

        [JsonPropertyName("rundownId")]
        public Guid RundownId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("duration")]
        public string Duration { get; set; } = string.Empty;

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("details")]
        public List<ItemDetailDTO> Details { get; set; } = [];

    }
}
