using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace KafkaFactoryLibrary
{
    public class KafkaFactory
    {
        private readonly ProducerConfig _producerConfig;
        private readonly ConsumerConfig _consumerConfig;

        public KafkaFactory(IConfiguration configuration)
        {
            var bootstrapServers = configuration.GetValue<string>("Kafka:BootstrapServers");

            _producerConfig = new ProducerConfig { BootstrapServers = bootstrapServers };
            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = true
            };
        }

        public IKafkaClient CreateKafkaClient(string type, string groupId = null, IEnumerable<string> topics = null)
        {
            if (type.Equals("producer", StringComparison.OrdinalIgnoreCase))
            {
                return new KafkaProducerClient(_producerConfig);
            }
            else if (type.Equals("consumer", StringComparison.OrdinalIgnoreCase) && groupId != null && topics != null)
            {
                _consumerConfig.GroupId = groupId;
                return new KafkaConsumerClient(_consumerConfig, topics);
            }

            throw new ArgumentException("Invalid type or missing parameters for Kafka client creation.");
        }
    }
}
