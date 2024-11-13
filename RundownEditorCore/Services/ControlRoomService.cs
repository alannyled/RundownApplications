using Microsoft.AspNetCore.Mvc;
using CommonClassLibrary.DTO;
using RundownEditorCore.Interfaces;

namespace RundownEditorCore.Services
{
    public class ControlRoomService(HttpClient httpClient, ILogger<ControlRoomService> logger) : IControlRoomService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<ControlRoomService> _logger = logger;

        public async Task<List<ControlRoomDTO>> GetControlRoomsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<ControlRoomDTO>>("fetch-controlroom-with-hardware");
               // _logger.LogInformation($"FETCHED Controlrooms");
                return response;
            }
            catch (Exception ex)
            {
               // _logger.LogInformation($"Error fetching control rooms: {ex.Message}");               
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
                   // _logger.LogInformation($"CREATED Controlroom {createdControlRoom.Name}");
                    return createdControlRoom;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                  //  _logger.LogInformation($"ERROR creating control room: {response.StatusCode}, {errorContent}");
                }
            }
            catch (Exception ex)
            {
               // _logger.LogInformation($"ERROR creating control room: {ex.Message}");
                
            }
            return null;
        }

        public async Task<ControlRoomDTO> UpdateControlRoomAsync(string controlRoomId, ControlRoomDTO updatedControlRoom)
        {
            try
            {

                var response = await _httpClient.PutAsJsonAsync($"update-controlroom/{controlRoomId}", updatedControlRoom);

                if (response.IsSuccessStatusCode)
                {
                    var updatedControlRoomResponse = await response.Content.ReadFromJsonAsync<ControlRoomDTO>();
                  //  _logger.LogInformation($"UPDATED Controlroom {updatedControlRoomResponse.Name}");
                    return updatedControlRoomResponse;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                   // _logger.LogInformation($"ERROR updating control room: {response.StatusCode}, {errorContent}");
                }
            }
            catch (Exception ex)
            {
               // _logger.LogInformation($"ERROR during control room update: {ex.Message}");
            
            }
            return null;
        }

        public async Task<ControlRoomDTO> DeleteControlRoomAsync(string controlRoomId)
        {
            try
            {

                var response = await _httpClient.DeleteAsync($"delete-controlroom/{controlRoomId}");

                if (response.IsSuccessStatusCode)
                {
                    var deletedControlRoom = await response.Content.ReadFromJsonAsync<ControlRoomDTO>();
                  //  _logger.LogInformation($"DELETED Controlroom {deletedControlRoom.Name}");
                    return deletedControlRoom;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                   // _logger.LogInformation($"ERROR deleting control room: {response.StatusCode}, {errorContent}");
                }
            }
            catch (Exception ex)
            {
              //  _logger.LogInformation($"ERROR during control room deletion: {ex.Message}");

            }
            return null;
        }
    }
}