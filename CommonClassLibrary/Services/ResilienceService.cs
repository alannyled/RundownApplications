using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using KafkaServiceLibrary;
using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CommonClassLibrary.Services
{
    public class ResilienceService
    {
        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
        private readonly KafkaService _kafkaService;
        private readonly KafkaProducerClient _producerClient;

        public ResilienceService()
        {
            // Retry
            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        Console.WriteLine($"Retry {retryCount} efter fejl: {exception.Message}. Forsøger igen om {timeSpan.Seconds} sekunder.");
                        var messageObject = new
                        {
                            Action = "retry",
                            Count= retryCount,
                            Time = timeSpan.Seconds,
                            Message = exception.Message
                        };
                        string message = JsonConvert.SerializeObject(messageObject);
                        Console.WriteLine(message);
                        SendMessage("error", message);
                    });

            // Circuit breaker
            _circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1),
                    onBreak: (exception, timespan) =>
                    {
                        Console.WriteLine($"Circuit Breaker aktiveret! Ingen forsøg de næste {timespan.TotalSeconds} sekunder. Fejl: {exception.Message}");
                    },
                    onReset: () =>
                    {
                        Console.WriteLine("Circuit Breaker reset! Klar til at modtage forespørgsler igen.");
                    });

            // Kafka producer
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _kafkaService = new KafkaService(configuration);
            _producerClient = (KafkaProducerClient)_kafkaService.CreateKafkaClient("producer");
        }

        public async Task ExecuteWithResilienceAsync(Func<Task> action)
        {
            await _circuitBreakerPolicy.ExecuteAsync(async () =>
            {
                await _retryPolicy.ExecuteAsync(action);
            });
        }

        public async Task<T> ExecuteWithResilienceAsync<T>(Func<Task<T>> action)
        {
            return await _circuitBreakerPolicy.ExecuteAsync(async () =>
            {
                return await _retryPolicy.ExecuteAsync(action);
            });
        }
        public virtual void SendMessage(string topic, string message)
        {
            string key = Guid.NewGuid().ToString();
            _producerClient.Producer.Produce(topic, new Message<string, string> { Key = key, Value = message });
            //Console.WriteLine($"Sending message to TOPIC: {topic}, KEY: {key}, VALUE: {message}");
        }
    }
}

