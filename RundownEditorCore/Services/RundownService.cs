using RundownEditorCore.DTO;

namespace RundownEditorCore.Services
{
    public class RundownService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<RundownDTO>> GetActiveRundowsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<RundownDTO>>("fetch-active-rundowns-with-controlrooms");
            return response;
        }

    }
}

// skal hente rundows fra rundownsdb
// der ligger pt noget i shared templatestable.