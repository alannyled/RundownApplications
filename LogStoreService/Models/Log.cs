using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LogStoreService.Models
{
    public class Log
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid UUID { get; set; }

        [BsonElement("Host")]
        public string? Host { get; set; }

        [BsonElement("TimeStamp")]
        public DateTime TimeStamp { get; set; }

        [BsonElement("Message")]
        public string? Message { get; set; }

        [BsonElement("LogLevel")]
        public LogLevel LogLevel { get; set; }

        [BsonElement("LoggerName")]
        public string? LoggerName { get; set; }

        [BsonElement("Exception")]
        public string? Exception { get; set; }
        [BsonElement("IsUserRelevant")]
        public bool IsUserRelevant { get; set; } = false;
    }
}
