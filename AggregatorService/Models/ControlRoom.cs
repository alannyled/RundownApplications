using System.Text.Json.Serialization;

namespace AggregatorService.Models
{
    public class ControlRoom
    {
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        public List<Hardware> HardwareItems { get; set; }
    }

}
