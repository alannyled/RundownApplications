using Confluent.Kafka;
using Newtonsoft.Json;
using RundownEditorCore.DTO;
using CommonClassLibrary.DTO;
using RundownEditorCore.Interfaces;
using RundownEditorCore.States;

namespace RundownEditorCore.Services
{
    public class RundownService(HttpClient httpClient, ToastState toastState, SharedStates sharedStates, ILogger<RundownService> logger, IKafkaService kafkaService, IMessageBuilderService messageBuilderService) : IRundownService
    {
        private readonly HttpClient _httpClient = httpClient;
           private readonly ToastState _toastState = toastState;
        private readonly SharedStates _sharedStates = sharedStates;
        private readonly ILogger<RundownService> _logger = logger;
        private readonly IKafkaService _kafkaService = kafkaService;
        private readonly IMessageBuilderService _messageBuilderService = messageBuilderService;

        public async Task<List<RundownDTO>> GetRundownsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<RundownDTO>>("fetch-rundowns-with-controlrooms");
                return response ?? new List<RundownDTO>();
            }
            catch (Exception)
            {
                _toastState.FireToast("Der skete en fejl under hentning af alle rundowns", "text-bg-warning");
                return new List<RundownDTO>();
            }
        }

        public async Task<RundownDTO?> GetRundownAsync(string uuid)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<RundownDTO>($"fetch-rundown/{uuid}");
                return response;
            }
            catch (Exception)
            {
                _toastState.FireToast("Der skete en fejl under hentning af rundown med ID {uuid}", "text-bg-warning");
                return null;
            }
        }

        public async Task<RundownDTO?> CreateRundownFromTemplate(string templateId, string controlroomId, DateTimeOffset date)
        {
            try
            {
                var request = new RundownDTO
                {
                    ControlRoomId = controlroomId,
                    BroadcastDate = date
                };
                var createdRundown = await _httpClient.PostAsJsonAsync($"create-rundown-from-template/{templateId}", request);
                var response = await createdRundown.Content.ReadFromJsonAsync<RundownDTO>();
                return response;
            }
            catch (Exception)
            {
                _toastState.FireToast("Der skete en fejl under oprettelse af rundown fra template", "text-bg-warning");
                return null;
            }
        }

        public async Task<RundownDTO?> UpdateRundownControlRoomAsync(string rundownId, string controlRoomId)
        {
            try
            {
                var updateRequest = new RundownDTO
                {
                    ControlRoomId = controlRoomId
                };

                var response = await _httpClient.PutAsJsonAsync($"update-rundown-controlroom/{rundownId}", updateRequest);

                if (response.IsSuccessStatusCode)
                {
                    var rundown = await response.Content.ReadFromJsonAsync<RundownDTO>();
                     var allUpdatedRundowns = await GetRundownsAsync();
                     _sharedStates.SharedAllRundowns(allUpdatedRundowns);
                    return rundown;
                }
                _logger.LogWarning($"ERROR updating controlroom: {response.ReasonPhrase}");
                return null;
            }
            catch (Exception)
            {
                _toastState.FireToast("Der skete en fejl under opdatering af controlroom til rundown", "text-bg-warning");
                return null;
            }
        }

        public async Task<RundownDTO?> AddItemToRundownAsync(string rundownId, RundownItemDTO item)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"add-item-to-rundown/{rundownId}", item);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<RundownDTO>();
                }
                _logger.LogWarning($"ERROR adding item to rundown: {response.ReasonPhrase}");
                return null;
            }
            catch (Exception)
            {
                _toastState.FireToast("Der skete en fejl under tilføjelse af item til rundown", "text-bg-warning");
                return null;
            }
        }

        public async Task<RundownDTO?> AddDetailToItemAsync(string rundownId, ItemDetailDTO.ItemDetail itemDetail)
        {
            try
            {
                var json = JsonConvert.SerializeObject(itemDetail);
                var detail = JsonConvert.DeserializeObject<DetailDTO>(json);

                var response = await _httpClient.PutAsJsonAsync($"add-detail-to-item/{rundownId}", detail);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("ADDED Detail to item");
                    return await response.Content.ReadFromJsonAsync<RundownDTO>();
                }
                _logger.LogWarning($"ERROR adding detail to item: {response.ReasonPhrase}");
                return null;
            }
            catch (Exception)
            {
                _toastState.FireToast("Der skete en fejl under tilføjelse af detail til item i rundown", "text-bg-warning");
                return null;
            }
        }

        public async Task<RundownDTO?> UpdateDetailAsync(string rundownId, DetailDTO itemDetail)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"update-detail-in-item/{rundownId}", itemDetail);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<RundownDTO>();
                }
              //  _logger.LogWarning($"ERROR updating detail in item: {response.ReasonPhrase}");
                return null;
            }
            catch (Exception)
            {
                _toastState.FireToast("Der skete en fejl under opdatering af detail i rundown", "text-bg-warning");
                return null;
            }
        }

        public async Task<RundownDTO?> UpdateRundownAsync(string rundownId, RundownDTO rundown)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"update-rundown/{rundownId}", rundown);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<RundownDTO>();
                }
                _logger.LogWarning($"ERROR updating rundown: {response.ReasonPhrase}");
                return null;
            }
            catch (Exception)
            {
                _toastState.FireToast("Der skete en fejl under opdatering af rundown", "text-bg-warning");
                return null;
            }
        }
    }



}

