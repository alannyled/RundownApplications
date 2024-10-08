using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TemplateDbService.DAL.Interfaces;
using TemplateDbService.Models;

namespace TemplateDbService.DAL.Repositories 
{
    public class RundownTemplateRepository : IRundownTemplateRepository
    {
        private readonly IMongoCollection<RundownTemplate> _rundownTemplates;

        public RundownTemplateRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _rundownTemplates = mongoDatabase.GetCollection<RundownTemplate>("RundownTemplates");
        }

        public async Task<List<RundownTemplate>> GetAllAsync() =>
            await _rundownTemplates.Find(_ => true).ToListAsync();

        public async Task<RundownTemplate> GetByIdAsync(Guid uuid) =>
            await _rundownTemplates.Find(x => x.UUID == uuid).FirstOrDefaultAsync();

        public async Task CreateAsync(RundownTemplate template) =>
            await _rundownTemplates.InsertOneAsync(template);
        
        public async Task UpdateAsync(Guid uuid, RundownTemplate template) =>
            await _rundownTemplates.ReplaceOneAsync(x => x.UUID == uuid, template);

        public async Task DeleteAsync(Guid uuid) =>
            await _rundownTemplates.DeleteOneAsync(x => x.UUID == uuid);
    }
}

