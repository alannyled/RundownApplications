using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace KafkaFactoryLibrary
{
    public class KafkaFactory
    {
        private readonly ProducerConfig _producerConfig;
        private readonly ConsumerConfig _consumerConfig;
        private static string _bootstrapServers;

        // Vigtigt tilføj boostrap servers til appsettings.json i den applikation der bruger KafkaFactory
        public KafkaFactory(IConfiguration configuration)
        {
            _bootstrapServers = configuration.GetValue<string>("Kafka:BootstrapServers");

            _producerConfig = new ProducerConfig { BootstrapServers = _bootstrapServers };

            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };
        }

        // Opret producer
        public IProducer<string, string> CreateProducer()
        {
            return new ProducerBuilder<string, string>(_producerConfig).Build();
        }

        // Opret consumer
        public IConsumer<string, string> CreateConsumer(string groupId, IEnumerable<string> topics)
        {
            _consumerConfig.GroupId = groupId;
            var consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build();
            consumer.Subscribe(topics);
            return consumer;
        }
    }
}
