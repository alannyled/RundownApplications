using RemoteLoggerLibrary.Interfaces;
using TemplateDbService.BLL.Interfaces;

namespace TemplateDbService.BLL.Services
{
    public class LogService : ILogService
    {
        private readonly IKafkaService _kafkaService;

        public LogService(IKafkaService kafkaService)
        {
            _kafkaService = kafkaService;
        }

        public void SendMessage(string topic, string message)
        {
            _kafkaService.SendMessage(topic, message);
        }
    }
}
