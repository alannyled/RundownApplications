using KafkaFactoryLibrary;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;


var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory()) 
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();


var kafkaFactory = new KafkaFactory(configuration);

Console.WriteLine("Vælg funktion: 1 for Producer, 2 for Consumer");
var choice = Console.ReadLine();

if (choice == "1")
{
    // Test som producer
    var producer = kafkaFactory.CreateProducer();
    Console.WriteLine("Indtast topic:");
    var topic = Console.ReadLine();

    while (true)
    {
        Console.WriteLine("Indtast besked (eller 'exit' for at stoppe):");
        var message = Console.ReadLine();
        if (message?.ToLower() == "exit")
            break;

        producer.Produce(topic, new Message<string, string> { Key = Guid.NewGuid().ToString(), Value = message });
        Console.WriteLine($"Besked sendt til topic '{topic}': {message}");
    }
}
else if (choice == "2")
{
    // Test som consumer
    Console.WriteLine("Indtast group-id:");
    var groupId = Console.ReadLine();

    Console.WriteLine("Indtast topics (komma-separeret):");
    var topics = Console.ReadLine()?.Split(',');

    var consumer = kafkaFactory.CreateConsumer(groupId, topics);
    Console.WriteLine("Lytter til beskeder...");

    Task.Run(() =>
    {
        while (true)
        {
            var result = consumer.Consume();
            Console.WriteLine($"Modtaget besked fra topic '{result.Topic}': Key={result.Key}, Value={result.Value}");
        }
    });

    // Hold applikationen kørende
    Console.ReadLine();
}
else
{
    Console.WriteLine("Ugyldigt valg. Lukker programmet.");
}
