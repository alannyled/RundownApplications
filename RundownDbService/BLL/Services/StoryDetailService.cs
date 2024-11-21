using RundownDbService.BLL.Interfaces;
using RundownDbService.DAL.Interfaces;
using RundownDbService.DAL.Repositories;
using RundownDbService.Models;
using Newtonsoft.Json;
using CommonClassLibrary.Enum;

namespace RundownDbService.BLL.Services
{
    public class StoryDetailService(IRundownRepository rundownRepository, IKafkaService kafkaService) : IStoryDetailService
    {
        private readonly IRundownRepository _rundownRepository = rundownRepository;
        private readonly IKafkaService _kafkaService = kafkaService;
        public StoryDetail? GetModel(string type)
        {
            return type switch
            {
                "Video" => new StoryDetailVideo(),
                "Teleprompter" => new StoryDetailTeleprompter(),
                "Grafik" => new StoryDetailGraphic(),
                "Kommentar" => new StoryDetailComment(),
                "Voiceover" => new StoryDetailTeleprompter(),
                _ => new StoryDetail()
            };
        }      

        public async Task<Rundown> CreateStoryDetailAsync(Rundown rundown, RundownStory existingStory)
        {
            await _rundownRepository.UpdateStoryAsync(rundown.UUID, existingStory);
            var messageObject = new
            {
                Action = MessageAction.Update.ToString(),
                Story = existingStory,
                Rundown = rundown
            };
            string message = JsonConvert.SerializeObject(messageObject);
            string topic = MessageTopic.Rundown.ToKafkaTopic();
            _kafkaService.SendMessage(topic, message);
            return rundown;
        }
    }
}
