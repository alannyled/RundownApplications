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
                var response = await _httpClient.PostAsJsonAsync(string.Empty, newHardware);

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
            throw new NotImplementedException();
        }
        public async Task<HardwareDTO> DeleteHardwareAsync(string hardwareId)
        {
            throw new NotImplementedException();
        }
  
    }
}
