using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace RundownDbService.Models
{
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(ItemDetailTeleprompter), typeof(ItemDetailComment), typeof(ItemDetailVideo), typeof(ItemDetailGraphic))]
    public class ItemDetail
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid UUID { get; set; }

        [BsonElement("itemId")]
        public Guid ItemId { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("duration")]
        public TimeSpan Duration { get; set; }

        [BsonElement("order")]
        public int Order { get; set; }
    }

    [BsonDiscriminator("ItemDetailTeleprompter")]
    public class ItemDetailTeleprompter : ItemDetail
    {
        [BsonElement("prompterText")]
        public string PrompterText { get; set; }

        [BsonElement("detailType")]
        public string DetailType { get; set; } = "ItemDetailTeleprompter";
    }

    [BsonDiscriminator("ItemDetailVideo")]
    public class ItemDetailVideo : ItemDetail
    {

        [BsonElement("videoPath")]
        public string VideoPath { get; set; }

        [BsonElement("detailType")]
        public string DetailType { get; set; } = "ItemDetailVideo";
    }

    [BsonDiscriminator("ItemDetailGraphic")]
    public class ItemDetailGraphic : ItemDetail
    {
        [BsonElement("graphicId")]
        public string GraphicId { get; set; }

        [BsonElement("detailType")]
        public string DetailType { get; set; } = "ItemDetailGraphic";
    }

    [BsonDiscriminator("ItemDetailComment")]
    public class ItemDetailComment : ItemDetail
    {
        [BsonElement("comment")]
        public string Comment { get; set; }

        [BsonElement("detailType")]
        public string DetailType { get; set; } = "ItemDetailComment";
    }
}
