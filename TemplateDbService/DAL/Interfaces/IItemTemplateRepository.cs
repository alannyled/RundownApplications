using TemplateDbService.Models;

namespace TemplateDbService.DAL.Interfaces
{
    public interface IItemTemplateRepository
    {
        Task<List<StoryTemplate>> GetAllAsync();
        Task<StoryTemplate> GetByIdAsync(Guid uuid);
        Task CreateAsync(StoryTemplate itemTemplate);
        Task UpdateAsync(Guid uuid, StoryTemplate itemTemplate);
        Task DeleteAsync(Guid uuid);
    }
}

