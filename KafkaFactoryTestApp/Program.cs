using KafkaFactoryLibrary;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

// Indlæser konfiguration fra appsettings.json
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var kafkaFactory = new KafkaFactory(configuration);

Console.WriteLine("Vælg funktion: 1 for Producer, 2 for Consumer");
var choice = Console.ReadLine();

if (choice == "1")
{
    // Opret en producer via KafkaFactory
    IKafkaClient kafkaClient = kafkaFactory.CreateKafkaClient("producer");

    if (kafkaClient is KafkaProducerClient producerClient)
    {
        Console.WriteLine("Indtast topic:");
        var topic = Console.ReadLine();

        while (true)
        {
            Console.WriteLine("Indtast besked (eller 'exit' for at stoppe):");
            var message = Console.ReadLine();
            if (message?.ToLower() == "exit")
                break;

            producerClient.Producer.Produce(topic, new Message<string, string> { Key = Guid.NewGuid().ToString(), Value = message });
            Console.WriteLine($"Besked sendt til topic '{topic}': {message}");
        }
    }
}
else if (choice == "2")
{
    // Opret en consumer via KafkaFactory
    Console.WriteLine("Indtast group-id:");
    var groupId = Console.ReadLine();

    Console.WriteLine("Indtast topics (komma-separeret):");
    var topics = Console.ReadLine()?.Split(',');

    IKafkaClient kafkaClient = kafkaFactory.CreateKafkaClient("consumer", groupId, topics);

    if (kafkaClient is KafkaConsumerClient consumerClient)
    {
        var consumer = consumerClient.Consumer;

        Console.WriteLine("Lytter til beskeder...");
        await Task.Run(() =>
        {
            while (true)
            {
                try
                {
                    var result = consumer.Consume();
                    Console.WriteLine($"Modtaget besked fra topic '{result.Topic}': Key={result.Key}, Value={result.Value}");
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Fejl under forbrug af besked: {e.Error.Reason}");
                }
            }
        });

        // Hold applikationen kørende
        Console.ReadLine();
    }
}
else
{
    Console.WriteLine("Ugyldigt valg. Lukker programmet.");
}
