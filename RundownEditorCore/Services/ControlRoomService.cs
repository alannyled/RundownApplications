using RundownEditorCore.DTO;

namespace RundownEditorCore.Services
{
    public class ControlRoomService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<ControlRoomDTO>> GetControlRoomsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<ControlRoomDTO>>("fetch-controlroom-with-hardware");
            return response;
        }
        public async Task<ControlRoomDTO> CreateControlRoomAsync(ControlRoomDTO newControlRoom)
        {
            var response = await _httpClient.PostAsJsonAsync("create-controlroom", newControlRoom);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ControlRoomDTO>();
            }
            else
            {
                throw new Exception("Error creating control room");
            }
        }
        public async Task<ControlRoomDTO> UpdateControlRoomAsync(string controlRoomId, ControlRoomDTO updatedControlRoom)
        {
            throw new NotImplementedException();
        }
        public async Task<ControlRoomDTO> DeleteControlRoomAsync(string controlRoomId)
        {
            throw new NotImplementedException();
        }
    }

}
