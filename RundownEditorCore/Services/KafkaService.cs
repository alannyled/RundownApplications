using KafkaServiceLibrary;
using Confluent.Kafka;
using RundownEditorCore.States;
using System.Text.Json;
using CommonClassLibrary.DTO;
using System.Composition;



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
            try
            {
                _producerClient.Producer.Produce(topic, new Message<string, string> { Key = Guid.NewGuid().ToString(), Value = message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"KAFKA SendMessage: Error - {ex.Message}");
            }
        }


    }

    public class KafkaBackgroundService : BackgroundService
    {

        private readonly KafkaServiceLibrary.KafkaService _kafkaService;
        private KafkaConsumerClient _consumerClient;
        private DetailLockState _detailLockState;
        private SharedStates _sharedStates;
        private readonly ILogger<RundownService> _logger;

        public KafkaBackgroundService(
            KafkaServiceLibrary.KafkaService kafkaService,
            DetailLockState detailLockState,
            SharedStates sharedStates,
            ILogger<RundownService> logger
            )
        {

            _kafkaService = kafkaService;
            _detailLockState = detailLockState;
            _sharedStates = sharedStates;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string[] topics = { "rundown_story", "rundown_detail", "rundown" };
            _consumerClient = (KafkaConsumerClient)_kafkaService.CreateKafkaClient("consumer", "Gruppe_id", topics);

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
                            if (message.Topic == "rundown_detail")
                            {
                                var messageObject = JsonSerializer.Deserialize<DetailMessage>(message.Message.Value);
                                if (messageObject != null)
                                {
                                    _logger.LogInformation($"MESSAGE: {(messageObject.Locked ? "lås" : "oplås")} Detail Id '{messageObject.Detail?.UUID.ToString()}'");
                                    _detailLockState.SetLockState(messageObject.Detail, messageObject.Locked, messageObject.UserName);
                                }
                            }

                            if (message.Topic == "rundown_story")
                            {
                                var messageObject = JsonSerializer.Deserialize<ItemMessage>(message.Message.Value);
                                if (messageObject != null)
                                {
                                    _logger.LogInformation($"MESSAGE: Ny detail tilføjet {messageObject.Item.Name}");
                                    _sharedStates.SharedItem(messageObject.Item);

                                }

                            }

                            if (message.Topic == "rundown")
                            {

                            }
                        }
                        catch (JsonException ex)
                        {
                            _logger.LogError($"Fejl ved deserialisering af JSON: {ex.Message}");
                            _logger.LogError($"Modtaget besked: {message.Message.Value}");
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

    public class ItemMessage
    {
        public string? Action { get; set; }
        public RundownItemDTO? Item { get; set; }
        public string? Rundown { get; set; }
    }

    public class DetailMessage : ItemMessage
    {
        public DetailDTO? Detail { get; set; }
        public string? ItemId { get; set; }
        public bool Locked { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
    }



}
