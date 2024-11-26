using Confluent.Kafka;
using Newtonsoft.Json;
using RundownEditorCore.DTO;
using CommonClassLibrary.DTO;
using RundownEditorCore.Interfaces;
using RundownEditorCore.States;

namespace RundownEditorCore.Services
{
    public class RundownService(HttpClient httpClient, ToastState toastState, SharedStates sharedStates, RundownState rundownState, ILogger<RundownService> logger) : IRundownService
    {
        private readonly HttpClient _httpClient = httpClient;
           private readonly ToastState _toastState = toastState;
        private readonly SharedStates _sharedStates = sharedStates;
        private readonly RundownState _rundownState = rundownState;
        private readonly ILogger<RundownService> _logger = logger;

        public async Task<List<RundownDTO>> GetRundownsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<RundownDTO>>("fetch-rundowns-with-controlrooms");
                return response ?? [];
            }
            catch (Exception)
            {
                _toastState.FireToast("Der skete en fejl under hentning af alle rundowns", "text-bg-warning");
                return [];
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
                var stories = new List<RundownStoryDTO>(_rundownState.Rundown.Stories);
                var updateRequest = new RundownDTO
                {
                    ControlRoomId = controlRoomId,
                    Stories = stories
                };
                var response = await _httpClient.PutAsJsonAsync($"update-rundown-controlroom/{rundownId}", updateRequest);

                if (response.IsSuccessStatusCode)
                {
                    var rundown = await response.Content.ReadFromJsonAsync<RundownDTO>();
                     var allUpdatedRundowns = await GetRundownsAsync();
                     _sharedStates.SharedAllRundowns(allUpdatedRundowns);
                    return rundown;
                }
                var msg = $"Error updating controlroom: {response.ReasonPhrase}";
                _logger.LogWarning(msg);
                return null;
            }
            catch (Exception)
            {
                _toastState.FireToast("Der skete en fejl under opdatering af controlroom til rundown", "text-bg-warning");
                return null;
            }
        }

        public async Task<RundownDTO?> AddStoryToRundownAsync(string rundownId, RundownStoryDTO story)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"add-story-to-rundown/{rundownId}", story);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<RundownDTO>();
                }
                _logger.LogWarning($"Eroor adding story to rundown: {response.ReasonPhrase}");
                return null;
            }
            catch (Exception)
            {
                _toastState.FireToast("Der skete en fejl under tilføjelse af story til rundown", "text-bg-warning");
                return null;
            }
        }

        public async Task<RundownDTO?> AddDetailToStoryAsync(string rundownId, StoryDetailDTO.StoryDetail storyDetail)
        {
            try
            {
                var json = JsonConvert.SerializeObject(storyDetail);
                var detail = JsonConvert.DeserializeObject<DetailDTO>(json);

                var response = await _httpClient.PutAsJsonAsync($"add-detail-to-story/{rundownId}", detail);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Oprettet {detail?.Title} i historie");
                    return await response.Content.ReadFromJsonAsync<RundownDTO>();
                }
                _logger.LogWarning($"ERROR adding detail to story: {response.ReasonPhrase}");
                return null;
            }
            catch (Exception)
            {
                _toastState.FireToast("Der skete en fejl under tilføjelse af detail til story i rundown", "text-bg-warning");
                return null;
            }
        }

        public async Task<RundownDTO?> UpdateDetailAsync(string rundownId, DetailDTO storyDetail)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"update-detail-in-story/{rundownId}", storyDetail);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<RundownDTO>();
                }
              //  _logger.LogWarning($"ERROR updating detail in story: {response.ReasonPhrase}");
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

