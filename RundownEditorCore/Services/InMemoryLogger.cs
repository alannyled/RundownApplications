using System.Collections.Concurrent;

namespace RundownEditorCore.Services
{
    public class InMemoryLogger : ILogger
    {
        private readonly string _name;
        private static readonly ConcurrentQueue<string> _logs = new();

        public InMemoryLogger(string name)
        {
            _name = name;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                var logMessage = $"{DateTime.Now}: [{logLevel}]<br>&emsp;{formatter(state, exception)}"; //{_name}
                _logs.Enqueue(logMessage);
            }
        }

        public static IEnumerable<string> GetLogs() => _logs;
    }

    public class InMemoryLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => new InMemoryLogger(categoryName);

        public void Dispose() { }
    }

}
