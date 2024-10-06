using System.Text.Json.Serialization;

namespace AggregatorService.Models
{
    public class ControlRoom
    {
        [JsonPropertyName("uuid")]
        public Guid? Uuid { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("archivedDate")]
        public DateTime? ArchivedDate { get; set; }

        [JsonPropertyName("archivedBy")]
        public string? ArchivedBy { get; set; }

        public List<Hardware>? HardwareItems { get; set; }
    }

}
