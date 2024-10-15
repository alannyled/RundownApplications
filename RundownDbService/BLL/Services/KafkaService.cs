using KafkaServiceLibrary;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace RundownDbService.BLL.Services
{
    public class KafkaService
    {
        private readonly KafkaServiceLibrary.KafkaService _kafkaService;
        private readonly KafkaProducerClient _producerClient;
        private readonly KafkaConsumerClient _consumerClient;

        public KafkaService()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _kafkaService = new KafkaServiceLibrary.KafkaService(configuration);
            _producerClient = (KafkaProducerClient)_kafkaService.CreateKafkaClient("producer");
            _consumerClient = (KafkaConsumerClient)_kafkaService.CreateKafkaClient("consumer", "rundown", ["media_related"]);

        }

        public virtual void SendMessage(string topic, string message)
        {
            _producerClient.Producer.Produce(topic, new Message<string, string> { Key = Guid.NewGuid().ToString(), Value = message });
        }
    }
}
