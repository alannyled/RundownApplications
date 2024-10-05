using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RundownService.Models
{
    public class RundownItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid UUID { get; set; } = Guid.NewGuid();

        [BsonElement("rundownId")]
        public Guid RundownId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }


    }
}
