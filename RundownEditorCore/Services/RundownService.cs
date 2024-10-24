using Confluent.Kafka;
using Newtonsoft.Json;
using RundownEditorCore.DTO;
using CommonClassLibrary.DTO;
using RundownEditorCore.Interfaces;

namespace RundownEditorCore.Services
{
    public class RundownService(HttpClient httpClient, ILogger<RundownService> logger) : IRundownService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<RundownService> _logger = logger;

        public async Task<List<RundownDTO>> GetRundownsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<RundownDTO>>("fetch-rundowns-with-controlrooms");
            _logger.LogInformation($"FETCHED All Rundowns");
            return response;
        }

       

        public async Task<RundownDTO> GetRundownAsync(string uuid)
        {
            var response = await _httpClient.GetFromJsonAsync<RundownDTO>($"fetch-rundown/{uuid}");
            //var json = JsonConvert.SerializeObject(response, Formatting.Indented);
            //Console.WriteLine("Fetching rundown: " + json);
            _logger.LogInformation($"FETCHED Rundown {response.Name}");
            return response;
        }

        public async Task<RundownDTO> CreateRundownFromTemplate(string templateId, string controlroomId, DateTimeOffset date)
        {
            var request = new RundownDTO
            {
                ControlRoomId = controlroomId,
                BroadcastDate = date
            };
            var createdRundown = await _httpClient.PostAsJsonAsync($"create-rundown-from-template/{templateId}", request);
            var response = await createdRundown.Content.ReadFromJsonAsync<RundownDTO>();
            _logger.LogInformation($"CREATED Rundown {response.Name}");
            return response;
        }

        public async Task<RundownDTO> UpdateRundownControlRoomAsync(string rundownId, string controlRoomId)
        {
            var updateRequest = new RundownDTO
            {
                ControlRoomId = controlRoomId
            };

            var response = await _httpClient.PutAsJsonAsync($"update-rundown-controlroom/{rundownId}", updateRequest);

            if (response.IsSuccessStatusCode)
            {
                var rundown = await response.Content.ReadFromJsonAsync<RundownDTO>();
                _logger.LogInformation($"UPDATED Rundown {rundown.Name}");
                return rundown;
            }
            _logger.LogInformation($"ERROR updating control room: {response.ReasonPhrase}");
            return null;
        }

        public async Task<RundownDTO> AddItemToRundownAsync(string rundownId, RundownItemDTO item)
        {
            var response = await _httpClient.PutAsJsonAsync($"add-item-to-rundown/{rundownId}", item);
       
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"ADDED Item to Rundown");
                return await response.Content.ReadFromJsonAsync<RundownDTO>();
            }
            _logger.LogInformation($"ERROR adding item to rundown: {response.ReasonPhrase}");
            return null;
        }
        public async Task<RundownDTO> AddDetailToItemAsync(string rundownId, ItemDetailDTO.ItemDetail itemDetail)
        {
            var json = JsonConvert.SerializeObject(itemDetail);
            var detail = JsonConvert.DeserializeObject<DetailDTO>(json);

            var response = await _httpClient.PutAsJsonAsync($"add-detail-to-item/{rundownId}", detail);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"ADDED Detail to item");
                return await response.Content.ReadFromJsonAsync<RundownDTO>();
            }
            _logger.LogInformation($"ERROR adding detail to item: {response.ReasonPhrase}");
            return null;
        }

        public async Task<RundownDTO> UpdateDetailAsync(string rundownId, DetailDTO itemDetail)
        {
            var response = await _httpClient.PutAsJsonAsync($"update-detail-in-item/{rundownId}", itemDetail);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"UPDATED Detail in item");
                return await response.Content.ReadFromJsonAsync<RundownDTO>();
            }
            _logger.LogInformation($"ERROR updating detail in item: {response.ReasonPhrase}");
            return null;
        }

        public async Task<RundownDTO> UpdateRundownAsync(string rundownId, RundownDTO rundown)
        {
            var response = await _httpClient.PutAsJsonAsync($"update-rundown/{rundownId}", rundown);            

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"UPDATED {rundown.Name}");
                return await response.Content.ReadFromJsonAsync<RundownDTO>();
            }
            _logger.LogInformation($"ERROR updating rundown: {response.ReasonPhrase}");
            return null;
        }
    }

}

