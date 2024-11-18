using KafkaServiceLibrary;
using Confluent.Kafka;
using RundownEditorCore.States;
using RundownEditorCore.Interfaces;
using CommonClassLibrary.DTO;
using Newtonsoft.Json;
using System.Linq;
using RundownEditorCore.Components.Account.Pages.Manage;
using System.Net.Sockets;
using CommonClassLibrary.Enum;

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
        IControlRoomService controlRoomService,
        IMessageBuilderService messageBuilderService
            ) : BackgroundService
    {

        private readonly KafkaServiceLibrary.KafkaService _kafkaService = kafkaService;
        private readonly DetailLockState _detailLockState = detailLockState;
        private readonly SharedStates _sharedStates = sharedStates;
        private readonly ILogger<RundownService> _logger = logger;
        private readonly IControlRoomService _controlRoomService = controlRoomService;
        private readonly IMessageBuilderService _messageBuilderService = messageBuilderService;
        private static readonly LogRingBuffer<LogMessageDTO> _logBuffer = new(100); // Buffer med plads til 100 beskeder
        public static event Action<LogMessageDTO>? LogMessageAdded;
        public static IEnumerable<LogMessageDTO> RecentLogs => _logBuffer;


        private KafkaConsumerClient? _consumerClient;

        /// <summary>
        /// Deserialiserer Kafka besked til JSON
        /// </summary>
        /// <returns>JSON</returns>
        private static T? ConvertMessageToJson<T>(ConsumeResult<string, string> message)
        {
            return JsonConvert.DeserializeObject<T>(message.Message.Value);
        }

        private readonly string[] topics = [
                    MessageTopic.DetailLock.ToKafkaTopic(),
                    MessageTopic.Rundown.ToKafkaTopic(),
                    MessageTopic.ControlRoom.ToKafkaTopic(),
                    MessageTopic.Error.ToKafkaTopic(),
                    MessageTopic.Log.ToKafkaTopic()
                ];
        private async Task InitializeTopics()
        {
            await _kafkaService.CreateMissingTopicsAsync(topics, numPartitions: 3, replicationFactor: 1);
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await InitializeTopics();
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
                            if (message.Topic == MessageTopic.DetailLock.ToKafkaTopic())
                            {
                                var messageObject = ConvertMessageToJson<DetailMessage>(message);
                                if (messageObject != null)
                                {

                                    _detailLockState.SetLockState(messageObject.Detail, messageObject.Locked, messageObject.UserName);
                                    var log = new LogMessageDTO
                                    {
                                        TimeStamp = DateTime.UtcNow,
                                        Message = $"{messageObject.Name} er blevet {(messageObject.Locked ? $"låst for redigering af {messageObject.UserName}" : "låst op")}",
                                        LogLevel = LogLevel.Information,
                                    };
                                    _logBuffer.Add(log);
                                    LogMessageAdded?.Invoke(log);
                                }
                            }
                            if (message.Topic == MessageTopic.Rundown.ToKafkaTopic())
                            {
                                HandleRundownMessage(message);
                            }

                            if (message.Topic == MessageTopic.ControlRoom.ToKafkaTopic())
                            {
                                var messageObject = ConvertMessageToJson<ControlRoomMessage>(message);
                                //var controlrooms = messageObject.ControlRooms; (der mangler hardware her)                       
                                var controlrooms = await _controlRoomService.GetControlRoomsAsync();
                                _sharedStates.SharedControlRoom(controlrooms);
                            }

                            if (message.Topic == MessageTopic.Error.ToKafkaTopic())
                            {
                                var messageObject = ConvertMessageToJson<ErrorMessageDTO>(message);
                                _sharedStates.SharedError(messageObject);
                                _logger.LogError($"Kritisk Fejlbesked: {message.Message.Value}");
                            }
                            if (message.Topic == MessageTopic.Log.ToKafkaTopic())
                            {
                                var msg = ConvertMessageToJson<LogMessageDTO>(message);
                                if (msg != null)
                                {
                                    _logBuffer.Add(msg);
                                    LogMessageAdded?.Invoke(msg);
                                }
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

            if (messageObject?.Action == MessageAction.Update.ToString() && messageObject?.Rundown != null)
            {
                UpdateRundownInSharedStates(messageObject.Rundown);
            }
            if (messageObject?.Action == MessageAction.Create.ToString() && messageObject?.Rundown != null)
            {
                AddNewRundownToSharedStates(messageObject.Rundown);
            }
        }

        private void UpdateRundownInSharedStates(RundownDTO rundown)
        {
            var allRundowns = new List<RundownDTO>(sharedStates.AllRundowns);
            var updatedRundown = allRundowns.Find(r => r.UUID == rundown.UUID);
            var index = allRundowns.FindIndex(r => r.UUID == rundown.UUID);

            if (index >= 0 && updatedRundown != null)
            {
                rundown.ControlRoomId = updatedRundown.ControlRoomId;
                rundown.ControlRoomName = updatedRundown.ControlRoomName;
                allRundowns[index] = rundown;
                sharedStates.SharedAllRundowns(allRundowns);
            }
        }

        private void AddNewRundownToSharedStates(RundownDTO rundown)
        {
            var controlRoom = sharedStates.ControlRooms.FirstOrDefault(c => c.Uuid.ToString() == rundown.ControlRoomId);
            rundown.ControlRoomName = controlRoom?.Name;
            var allRundowns = new List<RundownDTO>(sharedStates.AllRundowns)
                {
                    rundown
                };
            sharedStates.SharedAllRundowns(allRundowns);
        }
    }

    public class ControlRoomMessage
    {
        public string? Action { get; set; }
        public DateTime TimeStamp { get; set; }
        public List<ControlRoomDTO> ControlRooms { get; set; }
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
