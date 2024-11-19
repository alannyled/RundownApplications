using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RundownDbService.DAL;
using RundownDbService.DemoData;
using RundownDbService.Models;

namespace RundownDbService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataSeedController : ControllerBase
    {
        private readonly IMongoCollection<Rundown> _rundownCollection;

        public DataSeedController(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _rundownCollection = mongoDatabase.GetCollection<Rundown>("Rundowns");
        }

        [HttpPost("reset-data")]
        public async Task<ActionResult<List<Rundown>>> ResetAndSeedAsync()
        {
            try
            {
                await _rundownCollection.DeleteManyAsync(FilterDefinition<Rundown>.Empty);

                var rundowns = RundownDemoData.RundownData();
                await _rundownCollection.InsertManyAsync(rundowns);

                return Ok(rundowns);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"En fejl opstod: {ex.Message}");
            }
        }

       
    }
}
