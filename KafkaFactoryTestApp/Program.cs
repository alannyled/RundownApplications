﻿using KafkaFactoryLibrary;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var kafkaFactory = new KafkaFactory(configuration);

while (true)
{
    Console.WriteLine("Vælg funktion: 1 for Producer, 2 for Consumer, 3 for Create Topics, 4 for Delete Topics, 5 for List Topics, eller 'exit' for at afslutte");
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
            await kafkaFactory.CreateTopicAsync(topics);
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
            await kafkaFactory.DeleteTopicsAsync(topics);
        }
        else
        {
            Console.WriteLine("Ingen topics angivet.");
        }
    }
    else if (choice == "5")
    {
        await kafkaFactory.ListTopicsAsync();
    }
    else if (choice == "1")
    {
        // Din eksisterende kode til producer
        var producerClient = (KafkaProducerClient)kafkaFactory.CreateKafkaClient("producer");
        Console.WriteLine("Indtast topic:");
        var topic = Console.ReadLine();

        while (true)
        {
            Console.WriteLine("Indtast besked (eller 'exit' for at stoppe producenten og vende tilbage til hovedmenuen):");
            var message = Console.ReadLine();
            if (message?.ToLower() == "exit")
                break;

            producerClient.Producer.Produce(topic, new Message<string, string> { Key = Guid.NewGuid().ToString(), Value = message });
            Console.WriteLine($"Besked sendt til topic '{topic}': {message}");
        }
    }
    else if (choice == "2")
    {
        // Din eksisterende kode til consumer
        Console.WriteLine("Indtast group-id:");
        var groupId = Console.ReadLine();

        Console.WriteLine("Indtast topics (komma-separeret):");
        var topics = Console.ReadLine()?.Split(',');

        var consumerClient = (KafkaConsumerClient)kafkaFactory.CreateKafkaClient("consumer", groupId, topics);
        var consumer = consumerClient.Consumer;

        Console.WriteLine("Lytter til beskeder... Tryk 'exit' for at vende tilbage til hovedmenuen.");

        var consumeTask = Task.Run(() =>
        {
            while (true)
            {
                var result = consumer.Consume();
                Console.WriteLine($"Modtaget besked fra topic '{result.Topic}': Key={result.Key}, Value={result.Value}");
            }
        });

        while (true)
        {
            var exitCommand = Console.ReadLine();
            if (exitCommand?.ToLower() == "exit")
            {
                consumer.Close();
                break;
            }
        }

        await consumeTask;
    }
    else if (choice?.ToLower() == "exit")
    {
        Console.WriteLine("Afslutter programmet.");
        break;
    }
    else
    {
        Console.WriteLine("Ugyldigt valg. Prøv igen.");
    }
}