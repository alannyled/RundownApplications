using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TemplateDbService.Models
{
    public class StoryTemplate
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid UUID { get; set; }

        [BsonElement("rundownId")]
        public Guid RundownId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("duration")]
        public TimeSpan Duration { get; set; }

        [BsonElement("order")]
        public int Order { get; set; }

        //[BsonElement("details")]
        //public List<StoryDetail> Details { get; set; } = [];
    }
}
