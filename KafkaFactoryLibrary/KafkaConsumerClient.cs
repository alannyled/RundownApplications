using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KafkaFactoryLibrary
{
    public class KafkaConsumerClient : IKafkaClient
    {
        public IConsumer<string, string> Consumer { get; }
        private readonly IEnumerable<string> _topics;

        public KafkaConsumerClient(ConsumerConfig config, IEnumerable<string> topics)
        {
            Consumer = new ConsumerBuilder<string, string>(config).Build();
            _topics = topics;
            Consumer.Subscribe(_topics);
        }

        public void Connect()
        {
            string topicList = string.Join(", ", _topics);
            Console.WriteLine($"Consumer connected and subscribed to topics: {topicList}");
        }
    }
}
