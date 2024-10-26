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

        public async Task<RundownItem> CreateItemDetailAsync(Guid rundownId, RundownItem existingItem)
        {
            await _rundownRepository.UpdateItemAsync(rundownId, existingItem);
            var messageObject = new
            {
                Action = "new_detail",
                Item = existingItem
            };
            string message = JsonConvert.SerializeObject(messageObject);
            Console.WriteLine($"Sending message to TOPIC STORY: {message}");
            _kafkaService.SendMessage("story", message);

            return existingItem;
        }



        //public async Task DeleteItemDetailAsync(Guid uuid)
        //{
        //    await _itemDetailRepository.DeleteAsync(uuid);
        //}

        //public async Task<List<ItemDetail>> GetAllItemDetailsAsync()
        //{
        //    return await _itemDetailRepository.GetAllAsync();
        //}

        //public async Task<ItemDetail> GetItemDetailByIdAsync(Guid uuid)
        //{
        //    return await _itemDetailRepository.GetByIdAsync(uuid);
        //}
    }
}
