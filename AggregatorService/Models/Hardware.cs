using System.Text.Json.Serialization;

namespace AggregatorService.Models
{
    public class Hardware
    {
        [JsonPropertyName("uuid")]
        public Guid? Uuid { get; set; }

        [JsonPropertyName("controlRoomId")]
        public Guid? ControlRoomId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("vendor")]
        public string? Vendor { get; set; }

        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("macAddress")]
        public string? MacAddress { get; set; }

        [JsonPropertyName("ipAddress")]
        public string? IpAddress { get; set; }

        [JsonPropertyName("port")]
        public int Port { get; set; }

        [JsonPropertyName("createddate")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("archiveddate")]
        public DateTime? ArchivedDate { get; set; }

        [JsonPropertyName("archivedby")]
        public string? ArchivedBy { get; set; }
    }

}
