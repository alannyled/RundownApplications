using CommonClassLibrary.DTO;
using RundownEditorCore.Interfaces;
using System.Text.Json;

namespace RundownEditorCore.Services
{
    public class MessageBuilderService : IMessageBuilderService
    {
        //public string BuildRundownUpdateMessage(RundownDTO rundown)
        //    {
        //        var messageObject = new
        //        {
        //            Action = "update",
        //            Rundown = rundown
        //        };
        //        return JsonSerializer.Serialize(messageObject);
        //    }

        //public string BuildItemMessage(RundownItemDTO item)
        //{
        //    var messageObject = new
        //    {
        //        Action = "selected_item",
        //        Item = item
        //    };
        //    return JsonSerializer.Serialize(messageObject);
        //}

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
                Action = action,
                Locked = locked,
                Detail = detail,
                UserName = userName
            };
            return JsonSerializer.Serialize(messageObject);
        }

    }
}
