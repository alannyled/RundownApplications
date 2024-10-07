using AggregatorService.DTO;
using AggregatorService.Factories;
using AggregatorService.Models;
using AggregatorService.Services;
using Microsoft.Extensions.Options;
using System.Text.Json;
using static AggregatorService.Controllers.RundownController;

namespace AggregatorService.Managers
{
    public class RundownManager(ServiceFactory serviceFactory, IOptions<ApiUrls> apiUrls)
    {
        private readonly ServiceFactory _serviceFactory = serviceFactory;
        private readonly ApiUrls _apiUrls = apiUrls.Value;

        public async Task<List<Rundown>> FetchRundownsWithControlRoomData()
        {
            var controlRoomService = _serviceFactory.GetService<ControlRoomService>();
            var rundownService = _serviceFactory.GetService<RundownService>();

            var controlRoomData = await controlRoomService.FetchData(_apiUrls.ControlRoomApi);

            var rundownData = await rundownService.FetchData(_apiUrls.RundownApi);

            var controlRooms = JsonSerializer.Deserialize<List<ControlRoom>>(controlRoomData);
            var rundowns = JsonSerializer.Deserialize<List<Rundown>>(rundownData);

            foreach (var rundown in rundowns)
            {
                var controlRoom = controlRooms
                    .FirstOrDefault(c => c.Uuid == rundown.ControlRoomId);
                if (controlRoom != null)
                {
                    rundown.ControlRoomName = controlRoom.Name; 
                }
            }

            return rundowns;
        }

        public async Task<Rundown> FetchSelectedRundown(string rundownId)
        {
            var rundownService = _serviceFactory.GetService<RundownService>();
            var controlRoomService = _serviceFactory.GetService<ControlRoomService>();

            var rundownData = await rundownService.FetchData($"{_apiUrls.RundownApi}/{rundownId}");
            var rundown = JsonSerializer.Deserialize<Rundown>(rundownData);

            var controlRoomData = await controlRoomService.FetchData($"{_apiUrls.ControlRoomApi}/{rundown.ControlRoomId}");
            var controlRoom = JsonSerializer.Deserialize<ControlRoom>(controlRoomData);
            
            rundown.ControlRoomName = controlRoom.Name;
            

            return rundown;
        }

        public async Task<Rundown> UpdateControlRoomAsync(string rundownId, RundownDTO controlRoom)
        {
            var rundownService = _serviceFactory.GetService<RundownService>();

            // Konstruer DTO med både Uuid og ControlRoomId
            var dto = new RundownDTO
            {
                Uuid = rundownId,
                ControlRoomId = controlRoom.ControlRoomId
            };

            var response = await rundownService.PutAsJsonAsync($"{_apiUrls.RundownApi}/{rundownId}", dto);
            response.EnsureSuccessStatusCode();

            var updatedRundown = await response.Content.ReadFromJsonAsync<Rundown>();
            return updatedRundown;
        }


    }
}
