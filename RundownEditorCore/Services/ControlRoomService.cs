using Microsoft.AspNetCore.Mvc;
using CommonClassLibrary.DTO;
using RundownEditorCore.Interfaces;

namespace RundownEditorCore.Services
{
    public class ControlRoomService(HttpClient httpClient, ILogger<ControlRoomService> logger) : IControlRoomService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<ControlRoomService> _logger = logger;

        public async Task<List<ControlRoomDTO>?> GetControlRoomsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<ControlRoomDTO>>("fetch-controlroom-with-hardware");
                return response ?? [];
            }
            catch (Exception)
            {             
                return null;
            }
        }
        public async Task<ControlRoomDTO?> CreateControlRoomAsync(ControlRoomDTO newControlRoom)
        {
            try
            {            

                var response = await _httpClient.PostAsJsonAsync("create-controlroom", newControlRoom);

                if (response.IsSuccessStatusCode)
                {
                    var createdControlRoom = await response.Content.ReadFromJsonAsync<ControlRoomDTO>();
                    return createdControlRoom;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception)
            {
                
            }
            return null;
        }

        public async Task<ControlRoomDTO?> UpdateControlRoomAsync(string controlRoomId, ControlRoomDTO updatedControlRoom)
        {
            try
            {

                var response = await _httpClient.PutAsJsonAsync($"update-controlroom/{controlRoomId}", updatedControlRoom);

                if (response.IsSuccessStatusCode)
                {
                    var updatedControlRoomResponse = await response.Content.ReadFromJsonAsync<ControlRoomDTO>();
                    return updatedControlRoomResponse;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception)
            {
            
            }
            return null;
        }

        public async Task<ControlRoomDTO?> DeleteControlRoomAsync(string controlRoomId)
        {
            try
            {

                var response = await _httpClient.DeleteAsync($"delete-controlroom/{controlRoomId}");

                if (response.IsSuccessStatusCode)
                {
                    var deletedControlRoom = await response.Content.ReadFromJsonAsync<ControlRoomDTO>();
                    return deletedControlRoom;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception)
            {
            }
            return null;
        }
    }
}