using System.Text.Json.Serialization;

namespace AggregatorService.DTO
{
    public class RundownDTO
    {
        [JsonPropertyName("uuid")]
        public string? Uuid { get; set; }

        [JsonPropertyName("controlRoomId")]
        public string? ControlRoomId { get; set; }

        [JsonPropertyName("items")]
        public List<RundownItemDTO> Items { get; set; } = new List<RundownItemDTO>();
    }
}
