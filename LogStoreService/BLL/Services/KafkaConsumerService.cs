﻿using Confluent.Kafka;
using KafkaServiceLibrary;
using CommonClassLibrary.DTO;
using Newtonsoft.Json;
using LogStoreService.BLL.Interfaces;
using LogStoreService.Models;

namespace LogStoreService.BLL.Services
{
    /// <summary>
    /// Baggrundsservice til at lytte på Kafka beskeder
    /// </summary>
    public class KafkaBackgroundService(KafkaService kafkaService, ILogStoreService logStoreService) : BackgroundService
    {

        private readonly KafkaService _kafkaService = kafkaService;
        private readonly ILogStoreService _logStoreService = logStoreService;


        private KafkaConsumerClient? _consumerClient;

        /// <summary>
        /// Deserialiserer Kafka besked til JSON
        /// </summary>
        /// <returns>JSON</returns>
        private static T? ConvertMessageToJson<T>(ConsumeResult<string, string> message)
        {
            return JsonConvert.DeserializeObject<T>(message.Message.Value);
        }

        private readonly string[] topics = { "log" };
        private async Task InitializeTopics()
        {
            await _kafkaService.CreateMissingTopicsAsync(topics, numPartitions: 3, replicationFactor: 1);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await InitializeTopics();
            _consumerClient = (KafkaConsumerClient)_kafkaService.CreateKafkaClient("consumer", "LoggerService", topics);

            await Task.Yield();

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var message = _consumerClient.Consumer.Consume(stoppingToken);
                    if (message != null)
                    {
                        try
                        {
                            if (message.Topic == "log")
                            {
                                var messageObject = ConvertMessageToJson<Log>(message);
                                if (messageObject != null)
                                {
                                    await _logStoreService.CreateLogAsync(messageObject);
                                }
                            }
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"Fejl ved deserialisering af JSON: {ex.Message}");
                            Console.WriteLine($"Modtaget besked: {message.Message.Value}");
                        }
                    }
                }
            }
            catch (ConsumeException ex)
            {
                Console.WriteLine($"Fejl ved læsning af besked: {ex.Error.Reason}");
            }
        }


    }

}

