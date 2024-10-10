using AggregatorService.DTO;
using AggregatorService.Factories;
using AggregatorService.Models;
using AggregatorService.Services;
using Microsoft.Extensions.Options;
using System.Text.Json;
namespace AggregatorService.Managers
{
    public class ControlRoomManager(ServiceFactory serviceFactory, IOptions<ApiUrls> apiUrls)
    {
        private readonly ServiceFactory _serviceFactory = serviceFactory;
        private readonly ApiUrls _apiUrls = apiUrls.Value;

        public async Task<List<ControlRoom>> FetchControlRoomWithHardwareData()
        {
            var controlRoomService = _serviceFactory.GetService<ControlRoomService>();
            var hardwareService = _serviceFactory.GetService<HardwareService>();

            var controlRoomData = await controlRoomService.FetchData(_apiUrls.ControlRoomApi);

            var hardwareData = await hardwareService.FetchData(_apiUrls.HardwareApi);


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

        public async Task<ControlRoom> CreateControlRoomAsync(ControlRoomDTO newControlRoom)
        {
            Console.WriteLine("Creating control room in the database API: " + newControlRoom);
            var controlRoomService = _serviceFactory.GetService<ControlRoomService>();

            var response = await controlRoomService.PostAsJsonAsync(_apiUrls.ControlRoomApi, newControlRoom);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ControlRoom>();
            }
            else
            {
                throw new Exception("Failed to create control room in the database API.");
            }
        }

        public async Task<ControlRoom> UpdateControlRoomAsync(string controlRoomId, ControlRoom updatedControlRoom)
        {
            var controlRoomService = _serviceFactory.GetService<ControlRoomService>();

            var response = await controlRoomService.PutAsJsonAsync($"{_apiUrls.ControlRoomApi}/{controlRoomId}", updatedControlRoom);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ControlRoom>();
            }
            else
            {
                throw new Exception("Failed to update control room in the database API.");
            }
        }

        public async Task<ControlRoom> DeleteControlRoomAsync(string controlRoomId)
        {
            var controlRoomService = _serviceFactory.GetService<ControlRoomService>();

            var response = await controlRoomService.DeleteAsync($"{_apiUrls.ControlRoomApi}/{controlRoomId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ControlRoom>();
            }
            else
            {
                throw new Exception("Failed to delete control room in the database API.");
            }
        }

    }

}
