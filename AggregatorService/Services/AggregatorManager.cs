using AggregatorService.Models;
using System.Text.Json;
namespace AggregatorService.Services
{
    public class AggregatorManager
    {
        private readonly IEnumerable<Aggregator> _services;

        public AggregatorManager(IEnumerable<Aggregator> services)
        {
            _services = services;
        }

        public async Task<List<ControlRoom>> FetchControlRoomWithHardwareData()
        {
            var controlRoomService = _services.First(s => s is ControlRoomService);
            var hardwareService = _services.First(s => s is HardwareService);

            var controlRoomData = await controlRoomService.FetchData();

            var hardwareData = await hardwareService.FetchData();

            var controlRooms = JsonSerializer.Deserialize<List<ControlRoom>>(controlRoomData);
            var hardwareItems = JsonSerializer.Deserialize<List<Hardware>>(hardwareData);

            foreach (var room in controlRooms)
            {
                room.HardwareItems = hardwareItems
                    .Where(h => h.ControlRoomId == room.Uuid)
                    .ToList();
            }

            return controlRooms;
        }

    }

}
