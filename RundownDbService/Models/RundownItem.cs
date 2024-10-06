using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RundownDbService.Models
{
    public class RundownItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid UUID { get; set; }

        [BsonElement("rundownId")]
        public Guid RundownId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }


    }
}
