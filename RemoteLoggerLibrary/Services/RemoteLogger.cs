using CommonClassLibrary.DTO;
using RemoteLoggerLibrary.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;

namespace RemoteLoggerLibrary.Providers
{
    public class RemoteLogger(ILogService logService) : ILogger
    {
        private readonly ILogService _logService = logService;

        IDisposable? ILogger.BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;            

            if (formatter != null)
            {
                var assemblyName = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown";
                var methodName = new StackTrace().GetFrame(1)?.GetMethod()?.Name ?? string.Empty;
                string message = formatter(state, exception);
                var logMessage = new LogMessageDTO
                {
                    TimeStamp = DateTime.UtcNow,
                    LogLevel = logLevel,
                    Assembly = assemblyName,
                    Method = methodName,
                    Message = message,
                    Exception = exception?.ToString() ?? string.Empty
                };

                var logJson = JsonConvert.SerializeObject(logMessage);
                _logService.SendMessage("log", logJson);
            }
        }
    }
}
