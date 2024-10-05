using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace RundownService.Models
{
    public class Rundown
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid UUID { get; set; } = Guid.NewGuid();

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("controlRoomId")]
        public Guid ControlRoomId { get; set; }

        [BsonElement("broadcastDate")]
        public DateTime BroadcastDate { get; set; }

        [BsonElement("archivedDate")]
        public DateTime? ArchivedDate { get; set; }

        [BsonElement("archivedBy")]
        public string? ArchivedBy { get; set; }
    }
}
