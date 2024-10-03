using AggregatorService.Factories;
using AggregatorService.Models;
using AggregatorService.Services;
using System.Text.Json;
namespace AggregatorService.Managers
{
    public class AggregatorManager
    {
        private readonly ServiceFactory _serviceFactory;

        public AggregatorManager(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public async Task<List<ControlRoom>> FetchControlRoomWithHardwareData()
        {
            var controlRoomService = _serviceFactory.GetService<ControlRoomService>();
            var hardwareService = _serviceFactory.GetService<HardwareService>();

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
