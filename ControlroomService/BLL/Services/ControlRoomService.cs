using ControlRoomDbService.BLL.Interfaces;
using ControlRoomDbService.DAL.Interfaces;
using ControlRoomDbService.Models;

namespace ControlRoomDbService.BLL.Services
{
    public class ControlRoomService : IControlRoomService
    {
        private readonly IControlRoomRepository _controlRoomRepository;

        public ControlRoomService(IControlRoomRepository controlRoomRepository)
        {
            _controlRoomRepository = controlRoomRepository;
        }

        public async Task<List<ControlRoom>> GetControlRoomsAsync()
        {
            return await _controlRoomRepository.GetAsync();
        }

        public async Task<ControlRoom> GetControlRoomByIdAsync(string id)
        {
            return await _controlRoomRepository.GetByIdAsync(id);
        }

        public async Task CreateControlRoomAsync(ControlRoom controlRoom)
        {
            await _controlRoomRepository.CreateAsync(controlRoom);
        }

        public async Task UpdateControlRoomAsync(string id, ControlRoom updatedControlRoom)
        {
            await _controlRoomRepository.UpdateAsync(id, updatedControlRoom);
        }

        public async Task DeleteControlRoomAsync(string id)
        {
            await _controlRoomRepository.RemoveAsync(id);
        }

        public async Task DeleteAllControlRoomsAsync()
        {
            await _controlRoomRepository.RemoveAllAsync();
        }
    }

}
