using CommonClassLibrary.DTO;
using Confluent.Kafka;
using RundownEditorCore.Interfaces;
using System.Text.Json;

namespace RundownEditorCore.Services
{
    public class MessageBuilderService : IMessageBuilderService
    {
        public string BuildLogMessage(string message, LogLevel logLevel)
        {
            var messageObject = new LogMessageDTO
            {
                TimeStamp = System.DateTime.Now,
                Message = message,
                LogLevel = logLevel
            };
            return JsonSerializer.Serialize(messageObject);
        }

        public string BuildDetailLockMessage(DetailDTO detail, bool locked, string userName)
        {
            string action = locked ? "lock" : "unlock";

            var messageObject = new
            {
                Name = detail.Title,
                Action = action,
                Locked = locked,
                Detail = detail,
                UserName = userName,
                Timestamp = System.DateTime.Now
            };
            return JsonSerializer.Serialize(messageObject);
        }

    }
}
