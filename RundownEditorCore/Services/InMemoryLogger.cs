using System.Collections.Concurrent;

namespace RundownEditorCore.Services
{
    public class InMemoryLogger : ILogger
    {
        private readonly string _name;
        private static readonly ConcurrentQueue<string> _logs = new();
        private static readonly ConcurrentQueue<string> _simpleLogs = new();

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
                var logMessage = $"<td>{DateTime.Now}</td><td>{logLevel}</td><td> {_name}</td><td>{formatter(state, exception)}</td>"; 
                var simpleLogMessage = $"{DateTime.Now:HH:mm:ss} {formatter(state, exception)}";
                _logs.Enqueue(logMessage);
                _simpleLogs.Enqueue(simpleLogMessage);
            }
        }

        public static IEnumerable<string> GetLogs() => _logs;
        public static IEnumerable<string> GetSimpleLogs() => _simpleLogs;
    }

    public class InMemoryLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => new InMemoryLogger(categoryName);

        public void Dispose() { }
    }

}
