using AggregatorService.Factories;
using AggregatorService.Models;
using AggregatorService.Services;
using System.Text.Json;
namespace AggregatorService.Managers
{
    public class ControlRoomManager
    {
        private readonly ServiceFactory _serviceFactory;

        public ControlRoomManager(ServiceFactory serviceFactory)
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

        public async Task<ControlRoom> CreateControlRoomAsync(ControlRoom newControlRoom)
        {
            var controlRoomService = _serviceFactory.GetService<ControlRoomService>();

            var response = await controlRoomService.PostAsJsonAsync("https://localhost:3020/api/ControlRoom", newControlRoom);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ControlRoom>();
            }
            else
            {
                throw new Exception("Failed to create control room in the database API.");
            }
        }

    }

}
