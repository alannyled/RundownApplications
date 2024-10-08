using AggregatorService.Factories;
using AggregatorService.DTO;
using AggregatorService.Services;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AggregatorService.Managers
{
    public class TemplateManager(ServiceFactory serviceFactory, IOptions<ApiUrls> apiUrls)
    {
        private readonly ServiceFactory _serviceFactory = serviceFactory;
        private readonly ApiUrls _apiUrls = apiUrls.Value;

        public async Task<List<TemplateDTO>> FetchRundownTemplate()
        {
            try
            {
                var templateService = _serviceFactory.GetService<TemplateService>();
                var templateData = await templateService.FetchData(_apiUrls.RundownTemplateApi);
                var templates = JsonSerializer.Deserialize<List<TemplateDTO>>(templateData);
                return templates ?? new List<TemplateDTO>(); 
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP fejl ved hentning af rundown template: {httpEx.Message}");
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON deserialiseringsfejl: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"En fejl opstod ved hentning af rundown template: {ex.Message}");
            }
            return new List<TemplateDTO>(); 
        }


    }
}
