using RundownDbService.BLL.Interfaces;
using RundownDbService.DAL.Interfaces;
using RundownDbService.Models;

namespace RundownDbService.BLL.Services
{
    public class RundownItemService : IRundownItemService
    {
        private readonly IRundownItemRepository _rundownItemRepository;

        public RundownItemService(IRundownItemRepository rundownItemRepository)
        {
            _rundownItemRepository = rundownItemRepository;
        }

        public async Task<List<RundownItem>> GetAllRundownItemsAsync()
        {
            return await _rundownItemRepository.GetAllAsync();
        }

        public async Task<List<RundownItem>> GetRundownItemsAsync(Guid rundownUuid)
        {
            return await _rundownItemRepository.GetByRundownUuidAsync(rundownUuid);
        }

        public async Task<RundownItem> GetRundownItemByIdAsync(Guid uuid)
        {
            return await _rundownItemRepository.GetByIdAsync(uuid);
        }

        public async Task CreateRundownItemAsync(RundownItem newItem)
        {
            // Eventuel forretningslogik
            await _rundownItemRepository.CreateAsync(newItem);
        }

        public async Task UpdateRundownItemAsync(Guid uuid, RundownItem updatedItem)
        {
            await _rundownItemRepository.UpdateAsync(uuid, updatedItem);
        }

        public async Task DeleteRundownItemAsync(Guid uuid)
        {
            await _rundownItemRepository.DeleteAsync(uuid);
        }
    }
}
