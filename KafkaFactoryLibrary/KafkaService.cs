﻿using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace KafkaServiceLibrary
{
    public class KafkaService
    {
        private readonly ProducerConfig _producerConfig;
        private readonly ConsumerConfig _consumerConfig;
        private readonly string _bootstrapServers;

        public KafkaService(IConfiguration configuration)
        {
            _bootstrapServers = configuration.GetValue<string>("Kafka:BootstrapServers") ?? throw new ArgumentNullException(nameof(_bootstrapServers));

            _producerConfig = new ProducerConfig { BootstrapServers = _bootstrapServers };
            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = true,
                AutoCommitIntervalMs = 100,
                FetchMinBytes = 1,
                FetchWaitMaxMs = 100
            };
        }

        public IKafkaClient CreateKafkaClient(string type, string? groupId = null, IEnumerable<string>? topics = null)
        {
            if (type.Equals("producer", StringComparison.OrdinalIgnoreCase))
            {
                return new KafkaProducerClient(_producerConfig);
            }
            else if (type.Equals("consumer", StringComparison.OrdinalIgnoreCase) && groupId != null && topics != null)
            {
                _consumerConfig.GroupId = groupId;
                return new KafkaConsumerClient(_consumerConfig, topics);
            }

            throw new ArgumentException("Invalid type or missing parameters for Kafka client creation.");
        }

        public async Task CreateTopicAsync(string[] topics, int numPartitions = 1, short replicationFactor = 1)
        {
            using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = _bootstrapServers }).Build())
            {
                var topicSpecifications = new List<TopicSpecification>();

                foreach (var topic in topics)
                {
                    topicSpecifications.Add(new TopicSpecification
                    {
                        Name = topic.Trim(),
                        NumPartitions = numPartitions,
                        ReplicationFactor = replicationFactor
                    });
                }

                try
                {
                    await adminClient.CreateTopicsAsync(topicSpecifications);
                    Console.WriteLine($"Topic(s) '{string.Join(", ", topics)}' created successfully.");
                }
                catch (CreateTopicsException e)
                {
                    foreach (var result in e.Results)
                    {
                        if (result.Error.Code == ErrorCode.TopicAlreadyExists)
                        {
                            Console.WriteLine($"Topic '{result.Topic}' already exists.");
                        }
                        else if (result.Error.Code == ErrorCode.NoError)
                        {
                            Console.WriteLine($"Topic '{result.Topic}' was created successfully.");
                        }
                        else
                        {
                            Console.WriteLine($"An error occurred creating topic '{result.Topic}': {result.Error.Reason}");
                        }
                    }
                }
            }
        }

        public async Task DeleteTopicsAsync(string[] topics)
        {
            using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = _bootstrapServers }).Build())
            {
                try
                {
                    await adminClient.DeleteTopicsAsync(topics);
                    Console.WriteLine($"Topic(s) '{string.Join(", ", topics)}' deleted successfully.");
                }
                catch (DeleteTopicsException e)
                {
                    foreach (var result in e.Results)
                    {
                        if (result.Error.Code == ErrorCode.UnknownTopicOrPart)
                        {
                            Console.WriteLine($"Topic '{result.Topic}' does not exist.");
                        }
                        else
                        {
                            Console.WriteLine($"An error occurred deleting topic '{result.Topic}': {result.Error.Reason}");
                        }
                    }
                }
            }
        }

        public async Task ListTopicsAsync()
        {
            using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = _bootstrapServers }).Build())
            {
                try
                {
                    var metadata = await Task.Run(() => adminClient.GetMetadata(TimeSpan.FromSeconds(10)));
                    Console.WriteLine("Available topics:");

                    foreach (var topic in metadata.Topics)
                    {
                        Console.WriteLine($"- {topic.Topic}");
                    }
                }
                catch (KafkaException ex)
                {
                    Console.WriteLine($"An error occurred while listing topics: {ex.Error.Reason}");
                }
            }
        }

        public bool TopicExistsAsync(string topicName)
        {
            using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = _bootstrapServers }).Build())
            {
                try
                {
                    var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(5));
                    return metadata.Topics.Any(t => t.Topic == topicName && t.Error.Code == ErrorCode.NoError);
                }
                catch (KafkaException ex)
                {
                    Console.WriteLine($"Fejl ved forespørgsel af topics: {ex.Message}");
                    return false;
                }
            }
        }

        public async Task CreateMissingTopicsAsync(IEnumerable<string> topics, int numPartitions = 1, short replicationFactor = 1)
        {
            using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = _bootstrapServers }).Build())
            {
                var missingTopics = new List<string>();

                foreach (var topic in topics)
                {
                    if (!TopicExistsAsync(topic))
                    {
                        missingTopics.Add(topic);
                    }
                }

                if (missingTopics.Any())
                {
                    var topicSpecifications = missingTopics.Select(topic => new TopicSpecification
                    {
                        Name = topic,
                        NumPartitions = numPartitions,
                        ReplicationFactor = replicationFactor
                    }).ToList();

                    try
                    {
                        await adminClient.CreateTopicsAsync(topicSpecifications);
                        Console.WriteLine($"Manglende topics oprettet: {string.Join(", ", missingTopics)}");
                    }
                    catch (CreateTopicsException ex)
                    {
                        foreach (var result in ex.Results)
                        {
                            Console.WriteLine($"Fejl ved oprettelse af topic '{result.Topic}': {result.Error.Reason}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Alle nødvendige topics findes i Kafka.");
                }
            }
        }

    }
}
