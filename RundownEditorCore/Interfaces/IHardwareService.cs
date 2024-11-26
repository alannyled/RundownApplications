using CommonClassLibrary.DTO;

namespace RundownEditorCore.Interfaces
{
    public interface IHardwareService
    {
        Task<List<HardwareDTO>> GetHardwareAsync();
        Task<HardwareDTO?> AddHardwareAsync(HardwareDTO newHardware);
        Task<HardwareDTO?> UpdateHardwareAsync(string hardwareId, HardwareDTO updatedHardware);
        Task DeleteHardwareAsync(string hardwareId);
    }
}
