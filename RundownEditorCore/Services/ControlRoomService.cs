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
    }

}
