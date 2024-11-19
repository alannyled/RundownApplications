using AggregatorService.Factories;
using AggregatorService.Models;
using AggregatorService.Services;
using AggregatorService.DTO;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AggregatorService.Managers
{
    public class HardwareManager(ServiceFactory serviceFactory, IOptions<ApiUrls> apiUrls)
    {
        private readonly ServiceFactory _serviceFactory = serviceFactory;
        private readonly ApiUrls _apiUrls = apiUrls.Value;

        public async Task<List<Hardware>> FetchHardwareData()
        {
            var hardwareService = _serviceFactory.GetService<HardwareService>();

            var hardwareData = await hardwareService.FetchData(_apiUrls.HardwareApi);

            return JsonSerializer.Deserialize<List<Hardware>>(hardwareData) ?? [];
        }

        public async Task<Hardware?> CreateHardwareAsync(Hardware newHardware)
        {
            var hardwareService = _serviceFactory.GetService<HardwareService>();

            var response = await hardwareService.PostAsJsonAsync(_apiUrls.HardwareApi, newHardware);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Hardware>();
            }
            else
            {
                throw new Exception("Failed to create hardware in the database API.");
            }
        }

        public async Task<Hardware?> UpdateHardwareAsync(string hardwareId, Hardware updatedHardware)
        {
            var hardwareService = _serviceFactory.GetService<HardwareService>();

            var response = await hardwareService.PutAsJsonAsync($"{_apiUrls.HardwareApi}/{hardwareId}", updatedHardware);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Hardware>();
            }
            else
            {
                throw new Exception("Failed to update hardware in the database API.");
            }
        }

        public async Task DeleteHardwareAsync(string hardwareId)
        {
            var hardwareService = _serviceFactory.GetService<HardwareService>();

            var response = await hardwareService.DeleteAsync($"{_apiUrls.HardwareApi}/{hardwareId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to delete hardware in the database API.");
            }
        }
    }
}
