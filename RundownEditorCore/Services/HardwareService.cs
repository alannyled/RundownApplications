using RundownEditorCore.DTO;

namespace RundownEditorCore.Services
{
    public class HardwareService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<HardwareDTO>> GetHardwareAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<HardwareDTO> AddHardwareAsync(HardwareDTO newHardware)
        {
            throw new NotImplementedException();
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
