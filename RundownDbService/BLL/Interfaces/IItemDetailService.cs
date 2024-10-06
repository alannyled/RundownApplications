using RundownDbService.Models;

namespace RundownDbService.BLL.Interfaces
{
    public interface IItemDetailService
    {
        Task<List<ItemDetail>> GetAllItemDetailsAsync();
        Task<ItemDetail> GetItemDetailByIdAsync(Guid uuid);
        Task CreateItemDetailAsync(ItemDetail newItemDetail);
        Task UpdateItemDetailAsync(Guid uuid, ItemDetail updatedItemDetail);
        Task DeleteItemDetailAsync(Guid uuid);
    }
}
