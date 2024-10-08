using AggregatorService.Factories;
using AggregatorService.Models;
using AggregatorService.Services;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AggregatorService.Managers
{
    public class TemplateManager(ServiceFactory serviceFactory, IOptions<ApiUrls> apiUrls)
    {
        private readonly ServiceFactory _serviceFactory = serviceFactory;
        private readonly ApiUrls _apiUrls = apiUrls.Value;

        public async Task<List<Rundown>> FetchRundownTemplate()
        {
            var templateService = _serviceFactory.GetService<TemplateService>(); 
            var templateData = await templateService.FetchData(_apiUrls.RundownTemplateApi);
            var templates = JsonSerializer.Deserialize<List<Rundown>>(templateData);

            return templates;
        }

    
    }
}
