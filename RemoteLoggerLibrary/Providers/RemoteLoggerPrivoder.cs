
using RemoteLoggerLibrary.Interfaces;
using RemoteLoggerLibrary.Providers;
using Microsoft.Extensions.Logging;

namespace RemoteLoggerLibrary.Providers
{
    public class RemoteLoggerProvider : ILoggerProvider
    {
        private readonly ILogService _logService;

        public RemoteLoggerProvider(ILogService logService)
        {
            _logService = logService;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new RemoteLogger(_logService);
        }

        public void Dispose()
        {
            // Intet cleanup nødvendigt her, men det er her for at opfylde ILoggerProvider interface
        }
    }
}
