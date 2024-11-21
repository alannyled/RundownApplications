using RundownDbService.Models;

namespace RundownDbService.BLL.Interfaces
{
    public interface IStoryDetailService
    {
        StoryDetail? GetModel(string type);
        Task<Rundown> CreateStoryDetailAsync(Rundown rundown, RundownStory existingStory);
    }
}
