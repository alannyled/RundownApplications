using RundownDbService.BLL.Interfaces;
using RundownDbService.DAL.Interfaces;
using RundownDbService.DAL.Repositories;
using RundownDbService.Models;
using Newtonsoft.Json;

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
                Action = "update",
                Item = existingItem,
                Rundown = rundown
            };
            string message = JsonConvert.SerializeObject(messageObject);
            _kafkaService.SendMessage("rundown", message);
            return rundown;
        }
    }
}
