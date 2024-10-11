using RundownEditorCore.DTO;
using RundownEditorCore.Interfaces;

namespace RundownEditorCore.Services
{
    public class HardwareService(HttpClient httpClient) : IHardwareService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<HardwareDTO>> GetHardwareAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<HardwareDTO> AddHardwareAsync(HardwareDTO newHardware)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("create-hardware", newHardware);

                if (response.IsSuccessStatusCode)
                {
                    var createdHardware = await response.Content.ReadFromJsonAsync<HardwareDTO>();
                    return createdHardware;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error creating Hardware: {response.StatusCode}, {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during hardware creation: {ex.Message}");
                throw;
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
                    return updatedHardwareResponse;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error updating hardware: {response.StatusCode}, {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during hardware update: {ex.Message}");
                throw;
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
                    throw new Exception($"Error deleting hardware: {response.StatusCode}, {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during hardware deletion: {ex.Message}");
                throw;
            }
        }


    }
}
