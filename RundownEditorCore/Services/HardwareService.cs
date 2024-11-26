using Microsoft.Extensions.Logging;
using CommonClassLibrary.DTO;
using RundownEditorCore.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RundownEditorCore.Services
{
    public class HardwareService(HttpClient httpClient) : IHardwareService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<HardwareDTO>> GetHardwareAsync()
        {
            return await Task.FromResult(new List<HardwareDTO>());
        }

        public async Task<HardwareDTO?> AddHardwareAsync(HardwareDTO newHardware)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("create-hardware", newHardware);

                if (response.IsSuccessStatusCode)
                {
                    var createdHardware = await response.Content.ReadFromJsonAsync<HardwareDTO>();
                    if(createdHardware != null ) return createdHardware;
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<HardwareDTO?> UpdateHardwareAsync(string hardwareId, HardwareDTO updatedHardware)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"update-hardware/{hardwareId}", updatedHardware);

                if (response.IsSuccessStatusCode)
                {
                    var updatedHardwareResponse = await response.Content.ReadFromJsonAsync<HardwareDTO>();
                    if (updatedHardwareResponse != null) return updatedHardwareResponse;
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                return null;
            }
            catch (Exception)
            {
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
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
