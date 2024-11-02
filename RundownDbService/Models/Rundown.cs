using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace RundownDbService.Models
{
    public class Rundown
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid UUID { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("type")]
        public string? Type { get; set; }

        [BsonElement("controlRoomId")]
        public Guid ControlRoomId { get; set; }

        [BsonElement("broadcastDate")]
        public DateTime BroadcastDate { get; set; }

        [BsonElement("archivedDate")]
        public DateTime? ArchivedDate { get; set; } = null;

        [BsonElement("archivedBy")]
        public string? ArchivedBy { get; set; }

        [BsonElement("items")]
        public List<RundownItem> Items { get; set; } = [];
    }
}
