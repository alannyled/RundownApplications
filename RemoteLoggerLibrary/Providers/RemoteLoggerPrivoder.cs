
using RemoteLoggerLibrary.Interfaces;
using RemoteLoggerLibrary.Providers;
using Microsoft.Extensions.Logging;

namespace RemoteLoggerLibrary.Providers
{
    public class RemoteLoggerProvider(ILogService logService) : ILoggerProvider
    {
        private readonly ILogService _logService = logService;

        public ILogger CreateLogger(string categoryName)
        {
            return new RemoteLogger(_logService);
        }

        public void Dispose()
        {
            // Intet cleanup
        }
    }
}
