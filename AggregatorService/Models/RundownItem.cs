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

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("itemDetail")]
        public List<ItemDetail> ItemDetails { get; set; } = new List<ItemDetail>();


    }
}
