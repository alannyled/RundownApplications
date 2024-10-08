using Microsoft.Extensions.Logging.Console;
using RundownEditorCore.DTO;
using RundownEditorCore.Interfaces;

namespace RundownEditorCore.Services
{
    public class TemplateService(HttpClient httpClient) : ITemplateService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<TemplateDTO>> GetAllTemplatesAsync()
        {
            Console.WriteLine("Fetching all templates ");
            var response = await _httpClient.GetFromJsonAsync<List<TemplateDTO>>("fetch-all-rundown-templates");
            return response;
        }
    }
}
