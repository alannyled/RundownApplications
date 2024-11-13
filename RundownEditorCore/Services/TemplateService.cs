using Microsoft.Extensions.Logging.Console;
using CommonClassLibrary.DTO;
using RundownEditorCore.Interfaces;

namespace RundownEditorCore.Services
{
    public class TemplateService(HttpClient httpClient, ILogger<TemplateService> logger) : ITemplateService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<TemplateService> _logger = logger;

        public async Task<List<TemplateDTO>> GetAllTemplatesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<TemplateDTO>>("fetch-all-rundown-templates");           
            return response;
        }
    }
}
