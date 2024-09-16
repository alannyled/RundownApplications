using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RundownEditorCore.Services
{
    public class KafkaConsumer
    {
        private readonly ConsumerConfig _config;

        public KafkaConsumer(string bootstrapServers, string groupId)
        {
            // Initialiserer ConsumerConfig med de nødvendige Kafka-indstillinger
            _config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Latest, // Start fra den nyeste besked
            };
        }

        // Asynkron metode til at modtage beskeder fra Kafka
        public async Task ConsumeMessagesAsync(string topic, CancellationToken cancellationToken)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_config).Build())
            {
                consumer.Subscribe(topic);
                Console.WriteLine("Klar til at modtage beskeder");

                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        // Forbruger Kafka-beskeder
                        var consumeResult = consumer.Consume(cancellationToken);
                        string message = consumeResult.Message.Value;

                        // Processér den modtagne besked (her udskrevet til konsol)
                        Console.WriteLine($"Besked modtaget: {message}");
                    }
                }
                catch (OperationCanceledException)
                {
                    // Håndter når forbrug er annulleret
                    Console.WriteLine("Forbrug afbrudt");
                }
                finally
                {
                    consumer.Close();
                }
            }
        }
    }
}
