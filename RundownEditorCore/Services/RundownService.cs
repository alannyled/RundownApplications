using RundownEditorCore.DTO;
using RundownEditorCore.Interfaces;

namespace RundownEditorCore.Services
{
    public class RundownService(HttpClient httpClient) : IRundownService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<RundownDTO>> GetActiveRundowsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<RundownDTO>>("fetch-active-rundowns-with-controlrooms");
            return response;
        }

        public async Task<RundownDTO> GetRundownAsync(string uuid)
        {
            var response = await _httpClient.GetFromJsonAsync<RundownDTO>($"fetch-rundown/{uuid}");
            return response;
        }

        public async Task<RundownDTO> UpdateRundownControlRoomAsync(string uuid, string controlRoomId)
        {
           // test senere med send fra body og ikke params?
            var response = await _httpClient.PutAsync($"update-rundown-controlroom/{uuid}?controlRoomId={controlRoomId}", null);
            return await response.Content.ReadFromJsonAsync<RundownDTO>();
        }

    }

}

