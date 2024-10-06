using AggregatorService.Factories;
using AggregatorService.Models;
using AggregatorService.Services;
using Microsoft.Extensions.Options;
using System.Text.Json;

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

            var controlRoomData = await controlRoomService.FetchData();

            var rundownData = await rundownService.FetchData();

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
    }
}
