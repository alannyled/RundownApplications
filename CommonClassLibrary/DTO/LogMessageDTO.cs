
using Microsoft.Extensions.Logging;

namespace CommonClassLibrary.DTO
{
    public class LogMessageDTO
    {
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; } = string.Empty;
        public LogLevel LogLevel { get; set; }
        public string LoggerName { get; set; } = string.Empty;
        public string Assembly { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string Exception { get; set; } = string.Empty;
        public bool IsUserRelevant { get; set; } = false;
    }

}
