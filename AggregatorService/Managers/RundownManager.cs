using AggregatorService.Controllers;
using AggregatorService.DTO;
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

        public async Task<Rundown> CreateRundownFromTemplate(string templateId, DateTime date)
        {
            var rundownService = _serviceFactory.GetService<RundownService>();
            var templateService = _serviceFactory.GetService<TemplateService>();

            var template = await templateService.GetByIdAsync($"{_apiUrls.RundownTemplateApi}/{templateId}");
            var rundown = JsonSerializer.Deserialize<Rundown>(template);
            rundown.BroadcastDate = date;

            var response = await rundownService.PostAsJsonAsync(_apiUrls.RundownApi, date);
            response.EnsureSuccessStatusCode();

            var createdRundown = await response.Content.ReadFromJsonAsync<Rundown>();
            return createdRundown;
        }
        
        
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

        public async Task AddItemToRundownAsync(Guid rundownId, RundownItemDTO itemDto)
        {
            // Hent det eksisterende rundown
            var rundownService = _serviceFactory.GetService<RundownService>();
            var existingRundownResponse = await rundownService.GetByIdAsync($"{_apiUrls.RundownApi}/{rundownId}");
            var rundown = JsonSerializer.Deserialize<RundownDTO>(existingRundownResponse);
            
            if (existingRundownResponse is null)
            {
                throw new Exception("Rundown not found.");
            }

            // Tilføj det nye item til eksisterende liste
            rundown.Items.Add(itemDto);
            Console.WriteLine(JsonSerializer.Serialize(rundown));


            // Send opdateringen tilbage til service
            var response = await rundownService.PutAsJsonAsync($"{_apiUrls.RundownApi}/add-item-to-rundown/{rundownId}", rundown);
            response.EnsureSuccessStatusCode();
        }



    }
}
