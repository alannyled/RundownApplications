using MongoDB.Driver;
using TemplateDbService.Models;

namespace TemplateDbService.BLL.Services
{
    public class RundownTemplateService
    {
        private readonly IMongoCollection<RundownTemplate> _rundownTemplates;

        public RundownTemplateService(IMongoDatabase mongoDatabase)
        {
            _rundownTemplates = mongoDatabase.GetCollection<RundownTemplate>("RundownTemplates");
        }

        public async Task<List<RundownTemplate>> GetAllAsync() =>
            await _rundownTemplates.Find(_ => true).ToListAsync();

        public async Task<RundownTemplate> GetByIdAsync(Guid uuid) =>
            await _rundownTemplates.Find(x => x.UUID == uuid).FirstOrDefaultAsync();

        public async Task CreateAsync(RundownTemplate template)
        {            
            template.UUID = Guid.NewGuid();
            template.CreatedDate = DateTime.UtcNow;
            await _rundownTemplates.InsertOneAsync(template);
        }

        public async Task UpdateAsync(Guid uuid, RundownTemplate template) =>
            await _rundownTemplates.ReplaceOneAsync(x => x.UUID == uuid, template);

        public async Task DeleteAsync(Guid uuid) =>
            await _rundownTemplates.DeleteOneAsync(x => x.UUID == uuid);
    }
}

