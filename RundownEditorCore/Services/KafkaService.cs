using KafkaServiceLibrary;
using Confluent.Kafka;
using RundownEditorCore.States;
using RundownEditorCore.Interfaces;
using CommonClassLibrary.DTO;
using Newtonsoft.Json;

namespace RundownEditorCore.Services
{
    /// <summary>
    /// Kafka producer service og implementering af IKafkaService Sendmessage
    /// </summary>  
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

    /// <summary>
    /// Baggrundsservice til at lytte på Kafka beskeder
    /// </summary>
    public class KafkaBackgroundService(
        KafkaServiceLibrary.KafkaService kafkaService,
        DetailLockState detailLockState,
        SharedStates sharedStates,
        ILogger<RundownService> logger,
        IControlRoomService controlRoomService
            ) : BackgroundService
    {

        private readonly KafkaServiceLibrary.KafkaService _kafkaService = kafkaService;        
        private readonly DetailLockState _detailLockState = detailLockState;
        private readonly SharedStates _sharedStates = sharedStates;
        private readonly ILogger<RundownService> _logger = logger;
        private readonly IControlRoomService _controlRoomService = controlRoomService;

        private KafkaConsumerClient? _consumerClient;

        /// <summary>
        /// Deserialiserer Kafka besked til JSON
        /// </summary>
        /// <returns>JSON</returns>
        private static T? ConvertMessageToJson<T>(ConsumeResult<string, string> message)
        {
            return JsonConvert.DeserializeObject<T>(message.Message.Value);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string[] topics = { "rundown", "detail_lock", "story", "controlroom", "error" };
            _consumerClient = (KafkaConsumerClient)_kafkaService.CreateKafkaClient("consumer", "RundownEditorCore", topics);

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
                            if (message.Topic == "detail_lock")
                            {
                                var messageObject = ConvertMessageToJson<DetailMessage>(message);
                                if (messageObject != null)
                                {
                                    _logger.LogInformation($"MESSAGE: {(messageObject.Locked ? "lås" : "oplås")} Detail Id '{messageObject.Detail?.UUID.ToString()}'");
                                    _detailLockState.SetLockState(messageObject.Detail, messageObject.Locked, messageObject.UserName);
                                }
                            }
                            if (message.Topic == "story")
                            {
                                var messageObject = ConvertMessageToJson<ItemMessage>(message);
                                if (messageObject != null)
                                {
                                    _logger.LogInformation($"MESSAGE: Ny detail tilføjet {messageObject.Item.Name}");
                                    _sharedStates.SharedItem(messageObject.Item);

                                }

                            }
                            if (message.Topic == "rundown")
                            {
                                HandleRundownMessage(message);
                            }
                            if(message.Topic == "controlroom")
                            {
                                _logger.LogInformation($"MESSAGE: Kontrolrum opdateret");
                                var messageObject = ConvertMessageToJson<ControlRoomMessage>(message);
                                var controlrooms = await _controlRoomService.GetControlRoomsAsync();
                                _sharedStates.SharedControlRoom(controlrooms);
                            }
                            if(message.Topic == "error")
                            {
                                var messageObject = ConvertMessageToJson<ErrorMessageDTO>(message);
                                _sharedStates.SharedError(messageObject);
                                _logger.LogError($"Kritisk Fejlbesked: {message.Message.Value}");
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

        public void HandleRundownMessage(ConsumeResult<string, string> message)
        {
            var messageObject = ConvertMessageToJson<RundownMessage>(message);
            if (messageObject?.Action == "update")
            {
                _logger.LogInformation($"MESSAGE: Rundown opdateret UUID = {messageObject?.Rundown?.UUID}");               
                UpdateRundownInSharedStates(messageObject?.Rundown);
            }
            if (messageObject?.Action == "create")
            {
                _logger.LogInformation($"MESSAGE: Ny Rundown oprettet {messageObject?.Rundown?.Name}");
                _sharedStates.SharedNewRundown(messageObject?.Rundown);
            }
        }

        private void UpdateRundownInSharedStates(RundownDTO rundown)
        {
            var allRundowns = new List<RundownDTO>(sharedStates.AllRundowns);
            var updatedRundown = allRundowns.Find(r => r.UUID == rundown.UUID);
            var index = allRundowns.FindIndex(r => r.UUID == rundown.UUID);

            rundown.ControlRoomId = updatedRundown.ControlRoomId;
            rundown.ControlRoomName = updatedRundown.ControlRoomName;

            if (index >= 0)
            {
                allRundowns[index] = rundown;
                sharedStates.SharedAllRundowns(allRundowns);
            }
        }
    }

    public class ControlRoomMessage
    {
        public string? Action { get; set; }
    }
    public class ItemMessage
    {
        public RundownItemDTO? Item { get; set; }
    }

    public class RundownMessage
    {
        public string? Action { get; set; }
        public RundownItemDTO? Item { get; set; }
        public RundownDTO? Rundown { get; set; }
    }

    public class DetailMessage : RundownMessage
    {
        public DetailDTO? Detail { get; set; }
        public string? ItemId { get; set; }
        public bool Locked { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
    }

}
