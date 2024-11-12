using Newtonsoft.Json;
using RundownDbService.BLL.Interfaces;

namespace RundownDbService.BLL.Services
{
    public class RemoteLogger
    {
        private readonly IKafkaService _kafkaService;

        public RemoteLogger(IKafkaService kafkaService)
        {
            _kafkaService = kafkaService;
        }

        public void LogInformation(string message)
        {
            Log("Information", message);
        }

        private void Log(string level, string message)
        {
            var logMessage = new
            {
                TimeStamp = DateTime.UtcNow,
                Message = message,
                LogLevel = level,
                Host = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name
            };

            var logJson = JsonConvert.SerializeObject(logMessage);
            _kafkaService.SendMessage("log", logJson); 
        }
    }

}
