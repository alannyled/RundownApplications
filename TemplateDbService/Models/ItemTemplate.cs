using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TemplateDbService.Models
{
    public class ItemTemplate
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid UUID { get; set; }

        [BsonElement("rundownId")]
        public Guid RundownId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("duration")]
        public TimeSpan Duration { get; set; }

        [BsonElement("order")]
        public int Order { get; set; }

        //[BsonElement("details")]
        //public List<ItemDetail> Details { get; set; } = new List<ItemDetail>();
    }
}
