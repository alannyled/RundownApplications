using KafkaServiceLibrary;
using Confluent.Kafka;
using RundownEditorCore.States;
using System.Text.Json;

namespace RundownEditorCore.Services
{
    public class KafkaService
    {
        private readonly RundownState _rundownState;
        private readonly KafkaServiceLibrary.KafkaService _kafkaService;
        private readonly KafkaProducerClient _producerClient;
        private readonly KafkaConsumerClient _consumerClient;
        private readonly KafkaConsumerClient _rundownStoryConsumerKlient;

        public KafkaService()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _kafkaService = new KafkaServiceLibrary.KafkaService(configuration);
            _producerClient = (KafkaProducerClient)_kafkaService.CreateKafkaClient("producer");
            _consumerClient = (KafkaConsumerClient)_kafkaService.CreateKafkaClient("consumer", "Gruppe_id", ["rundown_story", "rundown"]);

            var consumeTask = Task.Run(() =>
            {
                while (true)
                {
                    var message = _consumerClient.Consumer.Consume();
                    Console.WriteLine($"Modtaget besked fra topic '{message.Topic}': Key={message.Key}, Value={message.Value}");
                    Test(message.Topic, message.Value);


                }
            });
        }

        public void Test(string topic, string value)
        {
            if (topic == "rundown_story")
            {
                Console.WriteLine($"Modtaget besked fra topic '{topic}'");
                var messageObject = JsonSerializer.Deserialize<ItemDetailMessage>(value);
                Console.WriteLine($"Modtaget besked med Detail ID '{messageObject.Detail}'");          
              Console.WriteLine($"FOUND RUndown ID '{_rundownState.Rundown.Uuid}'");
                var itemId = messageObject.Item;
                var detailId = messageObject.Detail;
                var item = _rundownState.Rundown.Items.FirstOrDefault(i => i.UUID.ToString() == itemId);
                Console.WriteLine($"FOUND Item ID '{item.UUID.ToString()}'");
                var detailExists = item.Details.Any(d => d.UUID.ToString() == detailId);
                Console.WriteLine($"FOUND Detail EXISTS?? '{detailExists}'");

                if (detailExists)
                {
                    Console.WriteLine($"Detail med DetailId = {detailId} findes i listen.");
                }
                else
                {
                    Console.WriteLine($"Detail med DetailId = {detailId} findes ikke.");
                }

            }
        }

        public virtual void SendMessage(string topic, string message)
        {
            _producerClient.Producer.Produce(topic, new Message<string, string> { Key = Guid.NewGuid().ToString(), Value = message });
        }

       
    }

    public class ItemDetailMessage {
        public string Item { get; set; }
        public string Detail { get; set; }
        public string Rundown { get; set; }
        public bool Locked { get; set; }
        public string Action { get; set; }
    }
}
