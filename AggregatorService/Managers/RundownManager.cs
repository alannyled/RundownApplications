using AggregatorService.Controllers;
using AggregatorService.DTO;
using AggregatorService.Factories;
using AggregatorService.Models;
using AggregatorService.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AggregatorService.Managers
{
    public class RundownManager(ServiceFactory serviceFactory, IOptions<ApiUrls> apiUrls)
    {
        private readonly ServiceFactory _serviceFactory = serviceFactory;
        private readonly ApiUrls _apiUrls = apiUrls.Value;

        public async Task<Rundown> CreateRundownFromTemplate(string templateId, string controlroomId, DateTime date)
        {
            var rundownService = _serviceFactory.GetService<RundownService>();
            var templateService = _serviceFactory.GetService<TemplateService>();
            var controlRoomService = _serviceFactory.GetService<ControlRoomService>();

            var controlRoomData = await controlRoomService.FetchData(_apiUrls.ControlRoomApi, controlroomId);
            var controlRoom = JsonConvert.DeserializeObject<ControlRoom>(controlRoomData);

            var templateData = await templateService.GetByIdAsync($"{_apiUrls.RundownTemplateApi}/{templateId}");
            var template = JsonConvert.DeserializeObject<Rundown>(templateData);

            var newRundown = new Rundown
            {
                Uuid = Guid.NewGuid(),
                BroadcastDate = date,
                ControlRoomId = controlRoom.Uuid,
                ControlRoomName = controlRoom.Name,
                Name = template.Name,
                Items = template.Items
            };
          
            var response = await rundownService.PostAsJsonAsync(_apiUrls.RundownApi, newRundown);
            response.EnsureSuccessStatusCode();
            
            var createdRundown = await response.Content.ReadFromJsonAsync<Rundown>();
            createdRundown.ControlRoomName = controlRoom.Name;
            return createdRundown;
        }
        
        
        public async Task<List<Rundown>> FetchRundownsWithControlRoomData()
        {
            var controlRoomService = _serviceFactory.GetService<ControlRoomService>();
            var rundownService = _serviceFactory.GetService<RundownService>();

            var controlRoomData = await controlRoomService.FetchData(_apiUrls.ControlRoomApi);

            var rundownData = await rundownService.FetchData(_apiUrls.RundownApi);

            var controlRooms = JsonConvert.DeserializeObject<List<ControlRoom>>(controlRoomData);
            var rundowns = JsonConvert.DeserializeObject<List<Rundown>>(rundownData);

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
            var rundown = JsonConvert.DeserializeObject<Rundown>(rundownData);
            //var json = JsonConvert.SerializeObject(rundown, Formatting.Indented);
            //Console.WriteLine("Fetching rundown: " + json);

            var controlRoomData = await controlRoomService.FetchData(_apiUrls.ControlRoomApi, rundown.ControlRoomId.ToString());
            var controlRoom = JsonConvert.DeserializeObject<ControlRoom>(controlRoomData);
            
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

        public async Task<Rundown> AddItemToRundownAsync(Guid rundownId, RundownItemDTO itemDto)
        {
            // Hent det eksisterende rundown
            var rundownService = _serviceFactory.GetService<RundownService>();
            var existingRundownResponse = await rundownService.FetchData($"{_apiUrls.RundownApi}/{rundownId}");
            var rundown = JsonConvert.DeserializeObject<RundownDTO>(existingRundownResponse);
            if (existingRundownResponse is null)
            {
                throw new Exception("Rundown not found.");
            }
            itemDto.UUID = Guid.NewGuid();
            // Tilføj det nye item til eksisterende liste
            rundown.Items.Add(itemDto);
       
            // Send opdateringen tilbage til service
            var response = await rundownService.PutAsJsonAsync($"{_apiUrls.RundownApi}/add-item-to-rundown/{rundownId}", itemDto);
            response.EnsureSuccessStatusCode();
            var updatedRundown = await response.Content.ReadFromJsonAsync<Rundown>();
            return updatedRundown;
        }

        public async Task<Rundown> AddDetailToItemAsync(Guid rundownId, RundownItemDTO itemDto)
        {
            // Hent det eksisterende rundown
            var rundownService = _serviceFactory.GetService<RundownService>();
            var existingRundownResponse = await rundownService.GetByIdAsync($"{_apiUrls.RundownApi}/{rundownId}");
            var rundown = JsonConvert.DeserializeObject<RundownDTO>(existingRundownResponse);
          
            if (existingRundownResponse is null)
            {
                throw new Exception("Rundown not found.");
            }
            var item = rundown.Items.FirstOrDefault(i => i.UUID == itemDto.UUID);
            if (item is null)
            {
                throw new Exception("Item not found.");
            }
            item.UUID = itemDto.UUID;
            var detail = itemDto.Details.First();
            detail.UUID = Guid.NewGuid();
            item.Details = [detail];
          
            // Send opdateringen tilbage til service
            var response = await rundownService.PutAsJsonAsync($"{_apiUrls.RundownApi}/add-item-detail-to-rundown/{rundownId}", item);
            response.EnsureSuccessStatusCode();
            var updatedRundown = await response.Content.ReadFromJsonAsync<Rundown>();
            return updatedRundown;
        }



    public async Task<Rundown> UpdateItemDetailAsync(Guid rundownId, ItemDetailDTO itemDetailDto)
    {
        // Hent det eksisterende rundown
        var rundownService = _serviceFactory.GetService<RundownService>();
        var existingRundownResponse = await rundownService.GetByIdAsync($"{_apiUrls.RundownApi}/{rundownId}");
        var rundown = JsonConvert.DeserializeObject<RundownDTO>(existingRundownResponse);
      
        if (existingRundownResponse is null)
        {
            throw new Exception("Rundown not found.");
        }
        var item = rundown.Items.FirstOrDefault(i => i.UUID == itemDetailDto.ItemId);
        if (item is null)
        {
            throw new Exception("Item not found.");
        }
        var detail = item.Details.FirstOrDefault(d => d.UUID == itemDetailDto.UUID);
        if (detail is null)
        {
            throw new Exception("Detail not found.");
        }
        detail.UUID = itemDetailDto.UUID;
        detail.ItemId = itemDetailDto.ItemId;
        detail.Title = itemDetailDto.Title;
        detail.Type = itemDetailDto.Type;
        detail.Order = itemDetailDto.Order;
        detail.Duration = itemDetailDto.Duration;
        detail.PrompterText = itemDetailDto.PrompterText ?? null;
        detail.VideoPath = itemDetailDto.VideoPath ?? null;
        detail.GraphicId = itemDetailDto.GraphicId ?? null;
        detail.Comment = itemDetailDto.Comment ?? null;
        // Send opdateringen tilbage til service
        var response = await rundownService.PutAsJsonAsync($"{_apiUrls.RundownApi}/edit-item-detail-in-rundown/{rundownId}", detail);
        response.EnsureSuccessStatusCode();
        var updatedRundown = await response.Content.ReadFromJsonAsync<Rundown>();
        return updatedRundown;
    }
}
}
