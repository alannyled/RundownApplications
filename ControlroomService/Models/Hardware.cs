using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ControlRoomService.Models
{
    public class Hardware
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid UUID { get; set; } = Guid.NewGuid();

        [BsonElement("controlRoomId")]
        public string ControlRoomId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("model")]
        public string Model { get; set; }

        [BsonElement("macAddress")]
        public string MacAddress { get; set; }

        [BsonElement("ipAddress")]
        public string IpAddress { get; set; }

        [BsonElement("port")]
        public int Port { get; set; }
    }
}
