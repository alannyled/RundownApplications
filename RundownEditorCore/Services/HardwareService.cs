using Microsoft.Extensions.Logging;
using CommonClassLibrary.DTO;
using RundownEditorCore.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RundownEditorCore.Services
{
    public class HardwareService(HttpClient httpClient, ILogger<HardwareService> logger) : IHardwareService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<HardwareService> _logger = logger;

        public async Task<List<HardwareDTO>> GetHardwareAsync()
        {
            return await Task.FromResult(new List<HardwareDTO>());
        }


        public async Task<HardwareDTO> AddHardwareAsync(HardwareDTO newHardware)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("create-hardware", newHardware);

                if (response.IsSuccessStatusCode)
                {
                    var createdHardware = await response.Content.ReadFromJsonAsync<HardwareDTO>();
                   // _logger.LogInformation($"CREATED Hardware {createdHardware.Name}");
                    return createdHardware;
                }
                var errorContent = await response.Content.ReadAsStringAsync();
               // _logger.LogInformation($"ERROR creating Hardware: {response.StatusCode}, {errorContent}");
                return null;
            }
            catch (Exception ex)
            {
              //  _logger.LogInformation($"Exception during hardware creation: {ex.Message}");
                return null;
            }
        }
        public async Task<HardwareDTO> UpdateHardwareAsync(string hardwareId, HardwareDTO updatedHardware)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"update-hardware/{hardwareId}", updatedHardware);

                if (response.IsSuccessStatusCode)
                {
                    var updatedHardwareResponse = await response.Content.ReadFromJsonAsync<HardwareDTO>();
                  //  _logger.LogInformation($"UPDATED Hardware {updatedHardwareResponse.Name}");
                    return updatedHardwareResponse;
                }
                var errorContent = await response.Content.ReadAsStringAsync();
              //  _logger.LogInformation($"ERROR updating hardware: {response.StatusCode}, {errorContent}");
                return null;
            }
            catch (Exception ex)
            {
               // _logger.LogInformation($"Exception during hardware update: {ex.Message}");
                return null;
            }
        }
        public async Task DeleteHardwareAsync(string hardwareId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"delete-hardware/{hardwareId}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                  //  _logger.LogInformation($"Error deleting hardware: {response.StatusCode}, {errorContent}");   
                }
            }
            catch (Exception ex)
            {
               // _logger.LogInformation($"Exception during hardware deletion: {ex.Message}");
            }
        }


    }
}
