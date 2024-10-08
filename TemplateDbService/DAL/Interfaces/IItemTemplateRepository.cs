using TemplateDbService.Models;

namespace TemplateDbService.DAL.Interfaces
{
    public interface IItemTemplateRepository
    {
        Task<List<ItemTemplate>> GetAllAsync();
        Task<ItemTemplate> GetByIdAsync(Guid uuid);
        Task CreateAsync(ItemTemplate itemTemplate);
        Task UpdateAsync(Guid uuid, ItemTemplate itemTemplate);
        Task DeleteAsync(Guid uuid);
    }
}

