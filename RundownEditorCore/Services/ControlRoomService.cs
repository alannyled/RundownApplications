﻿using RundownEditorCore.DTO;

namespace RundownEditorCore.Services
{
    public class ControlRoomService(HttpClient httpClient)
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
                Console.WriteLine($"Creating control room: {newControlRoom.Name}");

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
            throw new NotImplementedException();
        }
        public async Task<ControlRoomDTO> DeleteControlRoomAsync(string controlRoomId)
        {
            throw new NotImplementedException();
        }
    }

}