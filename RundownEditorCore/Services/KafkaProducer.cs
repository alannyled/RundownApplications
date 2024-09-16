using Confluent.Kafka;
using RundownEditorCore.Services;
using System.Net;
using System.Threading.Tasks;

namespace RundownEditorCore.Services
{
    public class KafkaProducer
    {
        private readonly ProducerConfig _config;

        public KafkaProducer(string bootstrapServers)
        {
            // Initialiserer ProducerConfig med de nødvendige Kafka-indstillinger
            _config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                ClientId = Dns.GetHostName(),
            };
        }

        // Asynkron metode til at sende beskeder til Kafka
        public async Task ProduceMessageAsync(string topic, string message)
        {
            // Opretter en Kafka-producer og sender beskeden til det specificerede topic
            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                var kafkaMessage = new Message<Null, string> { Value = message };
                var deliveryResult = await producer.ProduceAsync(topic, kafkaMessage);

                // Du kan tilføje logik her for at håndtere succes eller fejl
                Console.WriteLine($"Delivered '{deliveryResult.Value}' to '{deliveryResult.TopicPartitionOffset}'");
            }
        }
    }
}

// sådan bruges den
//var kafkaProducer = new KafkaProducer("172.19.95.154:9092");
//await kafkaProducer.ProduceMessageAsync("quickstart-events", "Her er beskeden ");

