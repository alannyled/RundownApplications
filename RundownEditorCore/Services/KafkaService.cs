﻿using KafkaServiceLibrary;
using Confluent.Kafka;
using RundownEditorCore.States;
using System.Text.Json;
using RundownEditorCore.DTO;


namespace RundownEditorCore.Services
{
    public class KafkaService
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
            _producerClient.Producer.Produce(topic, new Message<string, string> { Key = Guid.NewGuid().ToString(), Value = message });
        }


    }

    public class KafkaBackgroundService : BackgroundService
    {
        
        private readonly KafkaServiceLibrary.KafkaService _kafkaService;
        private KafkaConsumerClient _consumerClient;
        private DetailLockState _detailLockState;
        private readonly ILogger<RundownService> _logger;

        public KafkaBackgroundService(KafkaServiceLibrary.KafkaService kafkaService, DetailLockState detailLockState, ILogger<RundownService> logger)
        {
            
            _kafkaService = kafkaService;
            _detailLockState = detailLockState;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string[] topics = { "rundown_story", "rundown" };
            _consumerClient = (KafkaConsumerClient)_kafkaService.CreateKafkaClient("consumer", "Gruppe_id", topics);

            await Task.Yield();

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var message = _consumerClient.Consumer.Consume(stoppingToken);
                    if (message != null)
                    {
                        if(message.Topic == "rundown_story")
                        {
                            var messageObject = JsonSerializer.Deserialize<ItemDetailMessage>(message.Message.Value);
                            _logger.LogInformation($"MESSAGE: {(messageObject.Locked ? "lås" : "oplås")} Detail Id '{messageObject.Detail.UUID.ToString()}'");
                            //_detailLockState.SetLockState(messageObject.Detail.UUID.ToString(), messageObject.Locked, messageObject.UserName);
                            _detailLockState.SetLockState(messageObject.Detail, messageObject.Locked, messageObject.UserName);
                        }  
                        
                        if(message.Topic == "rundown")
                        {
                            var messageObject = JsonSerializer.Deserialize<ItemDetailMessage>(message.Message.Value);
                            _logger.LogInformation($"MESSAGE: {messageObject.Action} Rundown Id '{messageObject.Rundown}'");
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

    public class ItemDetailMessage
    {
        public string? Item { get; set; }
        public DetailDTO? Detail { get; set; }
        public string? Rundown { get; set; }
        public bool Locked { get; set; }
        public string? Action { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
    }
}