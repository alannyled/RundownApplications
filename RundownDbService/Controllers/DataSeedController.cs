using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RundownDbService.BLL.Interfaces;
using RundownDbService.DAL;
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
        public async Task<IActionResult> ResetAndSeedAsync()
        {
            try
            {
                // Slet alle eksisterende data i kollektionen
                await _rundownCollection.DeleteManyAsync(FilterDefinition<Rundown>.Empty);
                DateTime now = DateTime.Now;
                var Rundown1Uuid = Guid.NewGuid();
                var Rundown2Uuid = Guid.NewGuid();
                // Seed nye data
                var rundowns = new List<Rundown>
                {
                    new() { UUID = Rundown1Uuid ,Name = "TVA 1830", ControlRoomId = Guid.Parse("ebf89c25-90c8-4a2e-bdca-91db0eb39c93"), BroadcastDate = now, Items = [
                        new RundownItem { UUID = Guid.NewGuid(), RundownId = Rundown1Uuid, Name = "Intro", Duration = TimeSpan.FromSeconds(300) , Order = 0, Details = []},
                        new RundownItem { UUID = Guid.NewGuid(), RundownId = Rundown1Uuid, Name = "Velkomst", Duration = TimeSpan.FromSeconds(60), Order = 1, Details = [] },
                        new RundownItem { UUID = Guid.NewGuid(), RundownId = Rundown1Uuid, Name = "Vejr", Duration = TimeSpan.FromSeconds(120), Order = 2, Details = [] },
                        new RundownItem { UUID = Guid.NewGuid(), RundownId = Rundown1Uuid, Name = "Sport", Duration = TimeSpan.FromSeconds(180), Order = 3, Details = [] }


                    ] },
                    new() { UUID = Rundown2Uuid, Name = "21 Søndag", ControlRoomId = Guid.Parse("ebf89c25-90c8-4a2e-bdca-91db0eb39c93"), BroadcastDate = now.AddDays(5), Items = [] }
                };

                await _rundownCollection.InsertManyAsync(rundowns);

                return Ok("Rundown data nulstillet og seedet.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"En fejl opstod: {ex.Message}");
            }
        }

       
    }
}
