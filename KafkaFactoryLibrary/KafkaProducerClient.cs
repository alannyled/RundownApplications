using Confluent.Kafka;

namespace KafkaServiceLibrary
{
    public class KafkaProducerClient : IKafkaClient
    {
        public IProducer<string, string> Producer { get; }

        public KafkaProducerClient(ProducerConfig config)
        {
            Producer = new ProducerBuilder<string, string>(config).Build();
        }

        public void Connect()
        {
            Console.WriteLine("Producer connected.");
        }
    }
}
