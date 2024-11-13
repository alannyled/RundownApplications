using KafkaServiceLibrary;
using Confluent.Kafka;
using CommonClassLibrary.DTO;


namespace LogStoreService.BLL.Services
{
    public class Logger
    {
        public void Test(LogMessageDTO message)
        {
            Console.WriteLine($"{message.TimeStamp} - {message.LogLevel} From {message.Host}: {message.Message}");
        }
    }
}
