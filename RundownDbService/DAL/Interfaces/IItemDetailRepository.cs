using RundownDbService.Models;

namespace RundownDbService.DAL.Interfaces
{
    public interface IItemDetailRepository
    {
        Task<List<ItemDetail>> GetAllAsync();
        Task<ItemDetail> GetByIdAsync(Guid uuid);
        Task CreateAsync(ItemDetail detail);
        Task UpdateAsync(Guid uuid, ItemDetail detail);
        Task DeleteAsync(Guid uuid);
    }
}
