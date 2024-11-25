using ControlRoomDbService.DAL;
using ControlRoomDbService.Models;
using ControlRoomDbService.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using ControlRoomDbService.BLL.Services;
using CommonClassLibrary.DTO;

namespace ControlRoomDbService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataSeedController : ControllerBase
    {
        private readonly IMongoCollection<ControlRoom> _controlroomCollection;
        private readonly IMongoCollection<Hardware> _hardwareCollection;
        private readonly IKafkaService _kafkaService;

        public DataSeedController(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _controlroomCollection = mongoDatabase.GetCollection<ControlRoom>("ControlRoom");
            _hardwareCollection = mongoDatabase.GetCollection<Hardware>("Hardware");
            _kafkaService = new KafkaService();

        }

        [HttpPost("reset-data")]
        public async Task<IActionResult> ResetAndSeedAsync()
        {
            try
            {
                await _hardwareCollection.DeleteManyAsync(FilterDefinition<Hardware>.Empty);
                await _controlroomCollection.DeleteManyAsync(FilterDefinition<ControlRoom>.Empty);
                
                DateTime now = DateTime.Now;
             
                var controlrooms = new List<ControlRoom>
                {
                    new() { UUID = Guid.Parse("ebf89c25-90c8-4a2e-bdca-91db0eb39c93") ,Name = "PK14", Location = "DR Byen", CreatedDate = now},
                    new() { UUID = Guid.Parse("7b4c4fe6-2d9e-4276-949f-79cac408858c") ,Name = "PK13", Location = "DR Byen", CreatedDate = now},
                    new() { UUID = Guid.NewGuid() ,Name = "PK20", Location = "DR Århus", CreatedDate = now},
                    new() { UUID = Guid.NewGuid() ,Name = "PK5", Location = "DR Byen", CreatedDate = now},
                    new() { UUID = Guid.NewGuid() ,Name = "PK6", Location = "DR Byen", CreatedDate = now},
                    new() { UUID = Guid.NewGuid() ,Name = "PK01", Location = "Koncerthuset", CreatedDate = now},

                };

                var hardware = new List<Hardware>
                {
                    new() { UUID = Guid.NewGuid(), ControlRoomId = "ebf89c25-90c8-4a2e-bdca-91db0eb39c93", Name = "Videomixer", Vendor="Snell", Model="Kahuna 360", CreatedDate = now},
                    new() { UUID = Guid.NewGuid(), ControlRoomId = "ebf89c25-90c8-4a2e-bdca-91db0eb39c93", Name = "Teleprompter", Vendor="AutoScript", Model="IP", CreatedDate = now},
                    new() { UUID = Guid.NewGuid(), ControlRoomId = "ebf89c25-90c8-4a2e-bdca-91db0eb39c93", Name = "Kamera", Vendor="Sony", Model="2500", CreatedDate = now},
                    new() { UUID = Guid.NewGuid(), ControlRoomId = "7b4c4fe6-2d9e-4276-949f-79cac408858c", Name = "Videomixer", Vendor="Snell", Model="Kahuna 360", CreatedDate = now},
                    new() { UUID = Guid.NewGuid(), ControlRoomId = "7b4c4fe6-2d9e-4276-949f-79cac408858c", Name = "Teleprompter", Vendor="AutoScript", Model="IP", CreatedDate = now},
                    new() { UUID = Guid.NewGuid(), ControlRoomId = "7b4c4fe6-2d9e-4276-949f-79cac408858c", Name = "Kamera", Vendor="Sony", Model="2500", CreatedDate = now}
                };

                await _controlroomCollection.InsertManyAsync(controlrooms);
                string message = JsonConvert.SerializeObject(new { TimeStamp = DateTime.Now, Action = "reset" });
                _kafkaService.SendMessage("controlroom", message);

                return Ok("ControlRoom data nulstillet og seedet.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"En fejl opstod: {ex.Message}");
            }
        }
    }
}
