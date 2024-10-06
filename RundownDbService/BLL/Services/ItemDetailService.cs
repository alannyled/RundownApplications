using RundownDbService.BLL.Interfaces;
using RundownDbService.DAL.Interfaces;
using RundownDbService.Models;

namespace RundownDbService.BLL.Services
{
    public class ItemDetailService : IItemDetailService
    {
        private readonly IItemDetailRepository _itemDetailRepository;

        public ItemDetailService(IItemDetailRepository itemDetailRepository)
        {
            _itemDetailRepository = itemDetailRepository;
        }

        public async Task<List<ItemDetail>> GetAllItemDetailsAsync()
        {
            return await _itemDetailRepository.GetAllAsync();
        }

        public async Task<ItemDetail> GetItemDetailByIdAsync(Guid uuid)
        {
            return await _itemDetailRepository.GetByIdAsync(uuid);
        }

        public async Task CreateItemDetailAsync(ItemDetail newItemDetail)
        {
            // Eventuel forretningslogik
            await _itemDetailRepository.CreateAsync(newItemDetail);
        }

        public async Task UpdateItemDetailAsync(Guid uuid, ItemDetail updatedItemDetail)
        {
            await _itemDetailRepository.UpdateAsync(uuid, updatedItemDetail);
        }

        public async Task DeleteItemDetailAsync(Guid uuid)
        {
            await _itemDetailRepository.DeleteAsync(uuid);
        }
    }
}
