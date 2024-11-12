using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Concurrent;

using KafkaServiceLibrary;
using CommonClassLibrary.DTO;

namespace LogStoreService.Services
{
    public class CustomLoggerFactory
    {
        public static ILogger CreateLogger(string categoryName, Func<string, string, bool> sendMessage)
        {
            return new LogStoreLogger(sendMessage, categoryName);
        }
    }


    public class LogStoreLogger : ILogger
    {
        private readonly Func<string, string, bool> _sendMessage;
        private readonly string _categoryName;

        public LogStoreLogger(Func<string, string, bool> sendMessage, string categoryName)
        {
            _sendMessage = sendMessage;
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var message = formatter(state, exception);
            var logMessage = new LogMessageDTO
            {
                TimeStamp = DateTime.UtcNow,
                Message = message,
                LogLevel = logLevel,
                Host = Environment.MachineName
            };

            var logJson = JsonConvert.SerializeObject(logMessage);
            _sendMessage("log", logJson); 
        }
    }


}
