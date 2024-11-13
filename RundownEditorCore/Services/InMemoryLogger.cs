using System.Collections.Concurrent;


namespace RundownEditorCore.Services
{
    public class InMemoryLogger(string name) : ILogger
    {
        private readonly string _name = name;
        private static readonly ConcurrentQueue<string> _logs = new();
        private static readonly ConcurrentQueue<string> _simpleLogs = new();

        IDisposable? ILogger.BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                var logMessage = $"<td>{DateTime.Now}</td><td>{logLevel}</td><td> {_name}</td><td>{formatter(state, exception)}</td>";                 
                _logs.Enqueue(logMessage);

                if (_name.Contains("RundownEditorCore"))
                {
                    var simpleLogMessage = $"{formatter(state, exception)}";
                    _simpleLogs.Enqueue(simpleLogMessage);
                }

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
