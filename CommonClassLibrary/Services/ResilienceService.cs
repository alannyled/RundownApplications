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
                .WaitAndRetryAsync(3, retryAttempt =>
                {
                    return retryAttempt == 1 ? TimeSpan.FromSeconds(1) : TimeSpan.FromSeconds(Math.Pow(2, retryAttempt - 1));
                },
                    (exception, timeSpan, retryCount, context) =>
                    {
                        Console.WriteLine($"Retry {retryCount} efter fejl: {exception.Message}. Forsøger igen om {timeSpan.Seconds} sekunder.");
                        var messageObject = new
                        {
                            Action = "retry",
                            Count = retryCount,
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
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30),
                    onBreak: (exception, timespan) =>
                    {
                        Console.WriteLine($"Circuit Breaker aktiveret! Ingen forsøg de næste {timespan.TotalSeconds} sekunder. Fejl: {exception.Message}");
                        var messageObject = new
                        {
                            Action = "circuit_breaker_activated",
                            Duration = timespan.TotalSeconds,
                            Message = exception.Message
                        };
                        string message = JsonConvert.SerializeObject(messageObject);
                        SendMessage("error", message);
                    },
                    onReset: () =>
                    {
                        Console.WriteLine("Circuit Breaker reset! Klar til at modtage forespørgsler igen.");
                        var messageObject = new
                        {
                            Action = "circuit_breaker_reset",
                            Message = "Circuit Breaker reset! Klar til at modtage forespørgsler igen."
                        };
                        string message = JsonConvert.SerializeObject(messageObject);
                        SendMessage("error", message);
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
            try
            {
                await _circuitBreakerPolicy.ExecuteAsync(async () =>
                {
                    await _retryPolicy.ExecuteAsync(action);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Handling mislykkedes efter alle forsøg: {ex.Message}");
                var messageObject = new
                {
                    Action = "final_failure",
                    Message = ex.Message
                };
                string message = JsonConvert.SerializeObject(messageObject);
                SendMessage("error", message);
            }
        }

        public async Task<T?> ExecuteWithResilienceAsync<T>(Func<Task<T>> action)
        {
            try
            {
                return await _circuitBreakerPolicy.ExecuteAsync(async () =>
                {
                    return await _retryPolicy.ExecuteAsync(action);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Handling mislykkedes efter alle forsøg: {ex.Message}");
                var messageObject = new
                {
                    Action = "final_failure",
                    Message = ex.Message
                };
                string message = JsonConvert.SerializeObject(messageObject);
                SendMessage("error", message);
                return default;
                //throw;
            }
        }

        public virtual void SendMessage(string topic, string message)
        {
            string key = Guid.NewGuid().ToString();
            _producerClient.Producer.Produce(topic, new Message<string, string> { Key = key, Value = message });
        }
    }
}
