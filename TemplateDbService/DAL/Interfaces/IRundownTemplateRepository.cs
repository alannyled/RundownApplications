using TemplateDbService.Models;

namespace TemplateDbService.DAL.Interfaces
{
    public interface IRundownTemplateRepository
    {
        Task<List<RundownTemplate>> GetAllAsync();
        Task<RundownTemplate> GetByIdAsync(Guid uuid);
        Task CreateAsync(RundownTemplate template);
        Task UpdateAsync(Guid uuid, RundownTemplate template);
        Task DeleteAsync(Guid uuid);
    }
}

