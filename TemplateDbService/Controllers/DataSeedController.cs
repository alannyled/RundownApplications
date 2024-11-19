using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TemplateDbService.DAL;
using TemplateDbService.DemoData;
using TemplateDbService.Models;

namespace TemplateDbService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataSeedController : ControllerBase
    {
        private readonly IMongoCollection<RundownTemplate> _templateCollection;

        public DataSeedController(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _templateCollection = mongoDatabase.GetCollection<RundownTemplate>("RundownTemplates");
        }


        [HttpPost("reset-data")]
        public async Task<ActionResult<List<RundownTemplate>>> ResetAndSeedAsync()
        {
            try
            {
                await _templateCollection.DeleteManyAsync(FilterDefinition<RundownTemplate>.Empty);

                var templates = TemplateDemoData.TemplateData();
                await _templateCollection.InsertManyAsync(templates);
                Console.WriteLine("Data seed complete");
                return Ok(templates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"En fejl opstod: {ex.Message}");
            }
        }
    }
}
