using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RundownService.Models
{
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(ItemDetailTeleprompter), typeof(ItemDetailComment), typeof(ItemDetailVideo), typeof(ItemDetailGraphic))]
    public class ItemDetail
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid UUID { get; set; } = Guid.NewGuid();

        [BsonElement("itemId")]
        public Guid ItemId { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("order")]
        public int Order { get; set; }
    }

    [BsonDiscriminator("ItemDetailTeleprompter")]
    public class ItemDetailTeleprompter : ItemDetail
    {
        [BsonElement("prompterText")]
        public string PrompterText { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }
    }

    [BsonDiscriminator("ItemDetailVideo")]
    public class ItemDetailVideo : ItemDetail
    {
        [BsonElement("title")]
        public string title { get; set; }

        [BsonElement("videoPath")]
        public string VideoPath { get; set; }

        [BsonElement("duration")]
        public TimeSpan Duration { get; set; }
    }

    [BsonDiscriminator("ItemDetailGraphic")]
    public class ItemDetailGraphic : ItemDetail
    {
        [BsonElement("graphicId")]
        public string GraphicId { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("duration")]
        public TimeSpan Duration { get; set; }
    }

    [BsonDiscriminator("ItemDetailComment")]
    public class ItemDetailComment : ItemDetail
    {
        [BsonElement("comment")]
        public string Comment { get; set; }

    }
}
