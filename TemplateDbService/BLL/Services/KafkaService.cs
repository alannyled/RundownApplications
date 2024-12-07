﻿using KafkaServiceLibrary;
using Confluent.Kafka;
using TemplateDbService.BLL.Interfaces;

namespace TemplateDbService.BLL.Services
{
    public class KafkaService : IKafkaService
    {
        private readonly KafkaServiceLibrary.KafkaService _kafkaService;
        private readonly KafkaProducerClient _producerClient;
       // private readonly KafkaConsumerClient _consumerClient;

        public KafkaService()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _kafkaService = new KafkaServiceLibrary.KafkaService(configuration);
            _producerClient = (KafkaProducerClient)_kafkaService.CreateKafkaClient("producer");
           // _consumerClient = (KafkaConsumerClient)_kafkaService.CreateKafkaClient("consumer", "TemplateDbService", ["rundown"]);

        }

        public virtual void SendMessage(string topic, string message)
        {
            string key = Guid.NewGuid().ToString();
            _producerClient.Producer.Produce(topic, new Message<string, string> { Key = key, Value = message });
        }
    }

    
}
