﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TemplateDbService.Models
{
    public class RundownTemplate
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid UUID { get; set; } = Guid.NewGuid();

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("createdDate")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("archivedDate")]
        public DateTime? ArchivedDate { get; set; }

        [BsonElement("archivedBy")]
        public string? ArchivedBy { get; set; }

        [BsonElement("stories")]
        public List<StoryTemplate> Stories { get; set; } = [];
    }
}
