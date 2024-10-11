using RundownDbService.BLL.Interfaces;
using RundownDbService.DAL.Interfaces;
using RundownDbService.Models;

namespace RundownDbService.BLL.Services
{
    public class RundownService : IRundownService
    {
        private readonly IRundownRepository _rundownRepository;

        public RundownService(IRundownRepository rundownRepository)
        {
            _rundownRepository = rundownRepository;
        }

        public async Task<List<Rundown>> GetAllRundownsAsync()
        {
            return await _rundownRepository.GetAllAsync();
        }

        public async Task<Rundown> GetRundownByIdAsync(Guid uuid)
        {
            return await _rundownRepository.GetByIdAsync(uuid);
        }

        public async Task CreateRundownAsync(Rundown newRundown)
        {
            // Eventuel forretningslogik, før rundown oprettes
            await _rundownRepository.CreateAsync(newRundown);
        }

        public async Task<Rundown> UpdateRundownAsync(Guid uuid, Rundown updatedRundown)
        {
            // Eventuel forretningslogik, før rundown opdateres
            return await _rundownRepository.UpdateAsync(uuid, updatedRundown);
        }

        public async Task DeleteRundownAsync(Guid uuid)
        {
            // Eventuel forretningslogik, før rundown slettes
            await _rundownRepository.DeleteAsync(uuid);
        }
    }
}
