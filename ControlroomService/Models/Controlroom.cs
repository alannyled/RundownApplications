using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ControlRoomDbService.Models
{
    public class ControlRoom
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid UUID { get; set; } = Guid.NewGuid();

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("location")]
        public string Location { get; set; }

        [BsonElement("createdDate")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("archivedDate")]
        public DateTime? ArchivedDate { get; set; }

        [BsonElement("archivedBy")]
        public string? ArchivedBy { get; set; }
    }
}

