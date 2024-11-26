using System.Text.Json.Serialization;

namespace AggregatorService.Models
{
    public class Rundown
    {
        [JsonPropertyName("uuid")]
        public Guid Uuid { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("broadcastDate")]
        public DateTime BroadcastDate { get; set; }

        [JsonPropertyName("archivedDate")]
        public DateTime? ArchivedDate { get; set; } = null;

        [JsonPropertyName("archivedBy")]
        public string? ArchivedBy { get; set; } = null;

        [JsonPropertyName("controlRoomId")]
        public Guid ControlRoomId { get; set; }

        [JsonPropertyName("controlRoomName")]
        public string ControlRoomName { get; set; } = string.Empty;

        [JsonPropertyName("stories")]
        public List<RundownStory> Stories { get; set; } = [];
    }
}
