using RundownDbService.BLL.Interfaces;
using RundownDbService.DAL.Interfaces;
using RundownDbService.DAL.Repositories;
using RundownDbService.Models;
using Newtonsoft.Json;
using CommonClassLibrary.Enum;

namespace RundownDbService.BLL.Services
{
    public class ItemDetailService(IRundownRepository rundownRepository, IKafkaService kafkaService) : IItemDetailService
    {
        private readonly IRundownRepository _rundownRepository = rundownRepository;
        private readonly IKafkaService _kafkaService = kafkaService;
        public ItemDetail? GetModel(string type)
        {
            return type switch
            {
                "Video" => new ItemDetailVideo(),
                "Teleprompter" => new ItemDetailTeleprompter(),
                "Grafik" => new ItemDetailGraphic(),
                "Kommentar" => new ItemDetailComment(),
                "Voiceover" => new ItemDetailTeleprompter(),
                _ => new ItemDetail()
            };
        }      

        public async Task<Rundown> CreateItemDetailAsync(Rundown rundown, RundownItem existingItem)
        {
            await _rundownRepository.UpdateItemAsync(rundown.UUID, existingItem);
            var messageObject = new
            {
                Action = MessageAction.Update.ToString(),
                Item = existingItem,
                Rundown = rundown
            };
            string message = JsonConvert.SerializeObject(messageObject);
            string topic = MessageTopic.Rundown.ToKafkaTopic();
            _kafkaService.SendMessage(topic, message);
            return rundown;
        }
    }
}
