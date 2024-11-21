
using Microsoft.Extensions.Logging;

namespace CommonClassLibrary.DTO
{
    public class LogMessageDTO
    {
        public DateTime TimeStamp { get; set; }
        public string? Message { get; set; }
        public LogLevel LogLevel { get; set; }
        public string? LoggerName { get; set; }
        public string? Assembly { get; set; }
        public string Method { get; set; }
        public string? Exception { get; set; }
        public bool IsUserRelevant { get; set; } = false;
    }

}
