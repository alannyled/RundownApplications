using KafkaServiceLibrary;
using Confluent.Kafka;
using ControlRoomDbService.BLL.Interfaces;

namespace ControlRoomDbService.BLL.Services
{

    public class KafkaService : IKafkaService
    {
        private readonly KafkaServiceLibrary.KafkaService _kafkaService;
        private readonly KafkaProducerClient _producerClient;

        public KafkaService()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _kafkaService = new KafkaServiceLibrary.KafkaService(configuration);
            _producerClient = (KafkaProducerClient)_kafkaService.CreateKafkaClient("producer");

        }

        public virtual void SendMessage(string topic, string message)
        {
            string key = Guid.NewGuid().ToString();
            _producerClient.Producer.Produce(topic, new Message<string, string> { Key = key, Value = message });
        }
    }

}
