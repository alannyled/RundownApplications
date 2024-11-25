using KafkaServiceLibrary;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var kafkaService = new KafkaService(configuration);

while (true)
{
    Console.WriteLine("Vælg funktion:");
    Console.WriteLine("  • 1: Producer.");
    Console.WriteLine("  • 2: Consumer.");
    Console.WriteLine("  • 3: Create Topics.");
    Console.WriteLine("  • 4: Delete Topics.");
    Console.WriteLine("  • 5: List Topics.");
    Console.WriteLine("  • x: for at afslutte.");


    var choice = Console.ReadLine();

    if (choice == "3")
    {
        Console.WriteLine("Indtast topics (komma-separeret):");
        var topics = Console.ReadLine()?.Split(',')
                                   .Select(topic => topic.Trim())
                                   .Where(topic => !string.IsNullOrWhiteSpace(topic))
                                   .ToArray();

        if (topics != null && topics.Length > 0)
        {
            await kafkaService.CreateTopicAsync(topics);
        }
        else
        {
            Console.WriteLine("Ingen topics angivet.");
        }
    }
    else if (choice == "4")
    {
        Console.WriteLine("Indtast topics, du vil slette (komma-separeret):");
        var topics = Console.ReadLine()?.Split(',')
                                   .Select(topic => topic.Trim())
                                   .Where(topic => !string.IsNullOrWhiteSpace(topic))
                                   .ToArray();

        if (topics != null && topics.Length > 0)
        {
            await kafkaService.DeleteTopicsAsync(topics);
        }
        else
        {
            Console.WriteLine("Ingen topics angivet.");
        }
    }
    else if (choice == "5")
    {
        await kafkaService.ListTopicsAsync();
    }
    else if (choice == "1")
    {
        
        var producerClient = (KafkaProducerClient)kafkaService.CreateKafkaClient("producer");
        Console.WriteLine("Indtast topic:");
        var topic = Console.ReadLine();

        while (true)
        {
            Console.WriteLine("Indtast besked (eller 'x' for at stoppe producenten og vende tilbage til hovedmenuen):");
            var message = Console.ReadLine();
            if (message?.ToLower() == "x")
                break;

            producerClient.Producer.Produce(topic, new Message<string, string> { Key = Guid.NewGuid().ToString(), Value = message });
            Console.WriteLine($"Besked sendt til topic '{topic}': {message}");
        }
    }
    else if (choice == "2")
    {
       
        Console.WriteLine("Indtast group-id:");
        var groupId = Console.ReadLine();

        Console.WriteLine("Indtast topics (komma-separeret):");
        var topics = Console.ReadLine()?.Split(',');

        var consumerClient = (KafkaConsumerClient)kafkaService.CreateKafkaClient("consumer", groupId, topics);
        var consumer = consumerClient.Consumer;

        Console.WriteLine("Lytter til beskeder... Tryk 'x' for at vende tilbage til hovedmenuen.");

        var consumeTask = Task.Run(() =>
        {
            while (true)
            {
                var result = consumer.Consume();
                Console.WriteLine($"Modtaget besked fra topic '{result.Topic}': Key={result.Message.Key}, Value={result.Message.Value}");
            }
        });

        while (true)
        {
            var exitCommand = Console.ReadLine();
            if (exitCommand?.ToLower() == "x")
            {
                consumer.Close();
                break;
            }
        }

        await consumeTask;
    }
    else if (choice?.ToLower() == "x")
    {
        Console.WriteLine("Afslutter programmet.");
        break;
    }
    else
    {
        Console.WriteLine("Ugyldigt valg. Prøv igen.");
    }
}
