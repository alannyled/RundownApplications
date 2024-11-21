using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace RundownDbService.Models
{
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(StoryDetailTeleprompter), typeof(StoryDetailComment), typeof(StoryDetailVideo), typeof(StoryDetailGraphic))]
    public class StoryDetail
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid UUID { get; set; }

        [BsonElement("storyId")]
        public Guid StoryId { get; set; }

        [BsonElement("type")]
        public string? Type { get; set; }

        [BsonElement("title")]
        public string? Title { get; set; }

        [BsonElement("duration")]
        public TimeSpan Duration { get; set; }

        [BsonElement("order")]
        public int Order { get; set; }
    }

    [BsonDiscriminator("StoryDetailTeleprompter")]
    public class StoryDetailTeleprompter : StoryDetail
    {
        [BsonElement("prompterText")]
        public string? PrompterText { get; set; }

        [BsonElement("detailType")]
        public string? DetailType { get; set; } = "StoryDetailTeleprompter";
    }

    [BsonDiscriminator("StoryDetailVideo")]
    public class StoryDetailVideo : StoryDetail
    {

        [BsonElement("videoPath")]
        public string? VideoPath { get; set; }

        [BsonElement("detailType")]
        public string? DetailType { get; set; } = "StoryDetailVideo";
    }

    [BsonDiscriminator("StoryDetailGraphic")]
    public class StoryDetailGraphic : StoryDetail
    {
        [BsonElement("graphicId")]
        public string? GraphicId { get; set; }

        [BsonElement("detailType")]
        public string? DetailType { get; set; } = "StoryDetailGraphic";
    }

    [BsonDiscriminator("StoryDetailComment")]
    public class StoryDetailComment : StoryDetail
    {
        [BsonElement("comment")]
        public string? Comment { get; set; }

        [BsonElement("detailType")]
        public string? DetailType { get; set; } = "StoryDetailComment";
    }
}
