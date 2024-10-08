using TemplateDbService.Models;

namespace TemplateDbService.BLL.Interfaces
{
    public interface IItemTemplateService
    {
        Task<List<ItemTemplate>> GetAllAsync();
        Task<ItemTemplate> GetByIdAsync(Guid uuid);
        Task CreateAsync(ItemTemplate itemTemplate);
        Task UpdateAsync(Guid uuid, ItemTemplate itemTemplate);
        Task DeleteAsync(Guid uuid);
    }
}

