﻿using RemoteLoggerLibrary.Interfaces;
using RundownDbService.BLL.Interfaces;

namespace RundownDbService.BLL.Services
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
