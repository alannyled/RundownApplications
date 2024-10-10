using RundownEditorCore.DTO;
using RundownEditorCore.Interfaces;

namespace RundownEditorCore.Services
{
    public class ControlRoomService(HttpClient httpClient) : IControlRoomService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<ControlRoomDTO>> GetControlRoomsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<ControlRoomDTO>>("fetch-controlroom-with-hardware");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching control rooms: {ex.Message}");
                return null;
            }
        }
        public async Task<ControlRoomDTO> CreateControlRoomAsync(ControlRoomDTO newControlRoom)
        {
            try
            {            

                var response = await _httpClient.PostAsJsonAsync("create-controlroom", newControlRoom);

                if (response.IsSuccessStatusCode)
                {
                    var createdControlRoom = await response.Content.ReadFromJsonAsync<ControlRoomDTO>();
                    Console.WriteLine($"Control room created successfully: {createdControlRoom.Name}");
                    return createdControlRoom;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error creating control room: {response.StatusCode}, {errorContent}");
                    throw new Exception($"Error creating control room: {response.StatusCode}, {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during control room creation: {ex.Message}");
                throw;
            }
        }

        public async Task<ControlRoomDTO> UpdateControlRoomAsync(string controlRoomId, ControlRoomDTO updatedControlRoom)
        {
            try
            {
                Console.WriteLine($"Updating control room: {updatedControlRoom.Name}");

                var response = await _httpClient.PutAsJsonAsync($"update-controlroom/{controlRoomId}", updatedControlRoom);

                if (response.IsSuccessStatusCode)
                {
                    var updatedControlRoomResponse = await response.Content.ReadFromJsonAsync<ControlRoomDTO>();
                    Console.WriteLine($"Control room updated successfully: {updatedControlRoomResponse.Name}");
                    return updatedControlRoomResponse;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error updating control room: {response.StatusCode}, {errorContent}");
                    throw new Exception($"Error updating control room: {response.StatusCode}, {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during control room update: {ex.Message}");
                throw;
            }
        }

        public async Task<ControlRoomDTO> DeleteControlRoomAsync(string controlRoomId)
        {
            try
            {
                Console.WriteLine($"Deleting control room with ID: {controlRoomId}");

                var response = await _httpClient.DeleteAsync($"delete-controlroom/{controlRoomId}");

                if (response.IsSuccessStatusCode)
                {
                    var deletedControlRoom = await response.Content.ReadFromJsonAsync<ControlRoomDTO>();
                    Console.WriteLine($"Control room deleted successfully: {deletedControlRoom.Name}");
                    return deletedControlRoom;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error deleting control room: {response.StatusCode}, {errorContent}");
                    throw new Exception($"Error deleting control room: {response.StatusCode}, {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during control room deletion: {ex.Message}");
                throw;
            }
        }
    }
}