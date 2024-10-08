using TemplateDbService.BLL.Interfaces;
using TemplateDbService.DAL.Interfaces;
using TemplateDbService.DAL.Repositories;
using TemplateDbService.Models;

namespace TemplateDbService.BLL.Services
{
    public class RundownTemplateService : IRundownTemplateService
    {
        private readonly IRundownTemplateRepository _repository;

        public RundownTemplateService(IRundownTemplateRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RundownTemplate>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<RundownTemplate> GetByIdAsync(Guid uuid) => await _repository.GetByIdAsync(uuid);

        public async Task CreateAsync(RundownTemplate template)
        {
            
            template.UUID = Guid.NewGuid();
            template.CreatedDate = DateTime.UtcNow;
            await _repository.CreateAsync(template);
        }

        public async Task UpdateAsync(Guid uuid, RundownTemplate template)
        {
            
            await _repository.UpdateAsync(uuid, template);
        }

        public async Task DeleteAsync(Guid uuid) => await _repository.DeleteAsync(uuid);
    }
}

