﻿namespace TemplateDbService.BLL.Interfaces
{
    public interface IKafkaService
    {
        void SendMessage(string topic, string message);

    }
}
