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

        public Guid Rundown4Uuid { get; private set; }

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
                var Rundown3Uuid = Guid.NewGuid();
                var Rundown4Uuid = Guid.NewGuid();
                var Rundown5Uuid = Guid.NewGuid();
         
                var rundowns = new List<Rundown>
                {
                    new() { UUID = Rundown1Uuid ,Name = "TVA 1830", ControlRoomId = Guid.Parse("ebf89c25-90c8-4a2e-bdca-91db0eb39c93"), BroadcastDate = now.AddDays(5), Items = [
                        new RundownItem { UUID = Guid.NewGuid(), RundownId = Rundown1Uuid, Name = "Intro", Duration = TimeSpan.FromSeconds(12) , Order = 0, Details = []},
                        new RundownItem { UUID = Guid.NewGuid(), RundownId = Rundown1Uuid, Name = "Velkomst", Duration = TimeSpan.FromSeconds(15), Order = 1, Details = [] },
                        new RundownItem { UUID = Guid.NewGuid(), RundownId = Rundown1Uuid, Name = "Vejr", Duration = TimeSpan.FromSeconds(113), Order = 2, Details = [] },
                        new RundownItem { UUID = Guid.NewGuid(), RundownId = Rundown1Uuid, Name = "Sport", Duration = TimeSpan.FromSeconds(198), Order = 3, Details = [] }
                    ] },
                    new() { UUID = Rundown2Uuid, Name = "21 Søndag", ControlRoomId = Guid.Parse("7b4c4fe6-2d9e-4276-949f-79cac408858c"), BroadcastDate = now, Items = [] },
                    new() { UUID = Rundown3Uuid, Name = "Nyheder 1200", ControlRoomId = Guid.Parse("ebf89c25-90c8-4a2e-bdca-91db0eb39c93"), BroadcastDate = now.AddDays(2), Items = [] },
                    new() { UUID = Rundown4Uuid, Name = "Deadline", ControlRoomId = Guid.Parse("7b4c4fe6-2d9e-4276-949f-79cac408858c"), BroadcastDate = now.AddDays(-5), ArchivedDate = now, Items = [] },
                    new() { UUID = Rundown5Uuid, Name = "Nyheder 0900", ControlRoomId = Guid.Parse("ebf89c25-90c8-4a2e-bdca-91db0eb39c93"), BroadcastDate = now.AddDays(-2), ArchivedDate = now, Items = [] }

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
