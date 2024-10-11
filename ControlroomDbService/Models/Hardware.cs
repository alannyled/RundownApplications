using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ControlRoomDbService.Models
{
    public class Hardware
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid UUID { get; set; }

        [BsonElement("controlRoomId")]
        public string ControlRoomId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("vendor")]
        public string Vendor { get; set; }

        [BsonElement("model")]
        public string Model { get; set; }

        [BsonElement("macAddress")]
        public string MacAddress { get; set; }

        [BsonElement("ipAddress")]
        public string IpAddress { get; set; }

        [BsonElement("port")]
        public int Port { get; set; }

        [BsonElement("createddate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [BsonElement("archiveddate")]
        public DateTime? ArchivedDate { get; set; }

        [BsonElement("archivedby")]
        public string? ArchivedBy { get; set; }
    }
}
