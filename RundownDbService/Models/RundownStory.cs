using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RundownDbService.Models
{
    public class RundownStory
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid UUID { get; set; }

        [BsonElement("rundownId")]
        public Guid RundownId { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("duration")]
        public TimeSpan Duration { get; set; } // eller string?

        [BsonElement("order")]
        public int Order { get; set; }

        [BsonElement("details")]
        public List<StoryDetail> Details { get; set; } = [];
    }
}
