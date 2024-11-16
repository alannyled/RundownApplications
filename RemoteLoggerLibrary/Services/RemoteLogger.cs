using CommonClassLibrary.DTO;
using RemoteLoggerLibrary.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace RemoteLoggerLibrary.Providers
{
    public class RemoteLogger : ILogger
    {
        private readonly ILogService _logService;

        public RemoteLogger(ILogService logService)
        {
            _logService = logService;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        //public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;
        //public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;
        public bool IsEnabled(LogLevel logLevel) => true;


        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            if (formatter != null)
            {
                string message = formatter(state, exception);
                var logMessage = new LogMessageDTO
                {
                    TimeStamp = DateTime.UtcNow,
                    LogLevel = logLevel,
                    Host = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name,
                    Message = message,
                    Exception = exception?.ToString()
                };

                var logJson = JsonConvert.SerializeObject(logMessage);
                _logService.SendMessage("log", logJson);
            }
        }
    }
}
