using CommonClassLibrary.Services;
using TemplateDbService.BLL.Interfaces;
using TemplateDbService.DAL.Interfaces;
using TemplateDbService.DAL.Repositories;
using TemplateDbService.Models;

namespace TemplateDbService.BLL.Services
{
    public class RundownTemplateService(IRundownTemplateRepository repository, ResilienceService resilienceService, ILogger<RundownTemplateService> logger) : IRundownTemplateService
    {
        private readonly IRundownTemplateRepository _repository = repository;
        private readonly ResilienceService _resilienceService = resilienceService;
        private readonly ILogger<RundownTemplateService> _logger = logger;

        public async Task<List<RundownTemplate>> GetAllAsync()
        {
            var templates = await _resilienceService.ExecuteWithResilienceAsync(() => _repository.GetAllAsync());
            _logger.LogInformation("Alle templates er hentet i databasen");
            return templates;
        }

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

