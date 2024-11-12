using KafkaServiceLibrary;
using Confluent.Kafka;
using CommonClassLibrary.DTO;


namespace LogStoreService.Services
{
    public class Logger
    {
        public void Test(LogMessageDTO message)
        {
            Console.WriteLine($"{message.TimeStamp} - {message.LogLevel} - {message.Message}");
        }
    }
}
