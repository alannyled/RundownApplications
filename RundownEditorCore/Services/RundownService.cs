using Confluent.Kafka;
using RundownEditorCore.DTO;
using RundownEditorCore.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RundownEditorCore.Services
{
    public class RundownService(HttpClient httpClient) : IRundownService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<RundownDTO>> GetActiveRundowsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<RundownDTO>>("fetch-active-rundowns-with-controlrooms");
            return response;
        }

        public async Task<RundownDTO> GetRundownAsync(string uuid)
        {
            var response = await _httpClient.GetFromJsonAsync<RundownDTO>($"fetch-rundown/{uuid}");
            return response;
        }

        public async Task<RundownDTO> CreateRundownFromTemplate(string templateId, string controlroomId, DateTimeOffset date)
        {

            Console.WriteLine("Creating rundown from template on date: " + date.ToString());
            var request = new RundownDTO
            {
                ControlRoomId = controlroomId,
                BroadcastDate = date
            };
            var createdRundown = await _httpClient.PostAsJsonAsync($"create-rundown-from-template/{templateId}", request);
            var response = await createdRundown.Content.ReadFromJsonAsync<RundownDTO>();
            
            return response;
        }

        public async Task<RundownDTO> UpdateRundownControlRoomAsync(string rundownId, string controlRoomId)
        {
            var updateRequest = new RundownDTO
            {
                ControlRoomId = controlRoomId
            };

            var response = await _httpClient.PutAsJsonAsync($"update-rundown-controlroom/{rundownId}", updateRequest);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully updated control room.");
                return await response.Content.ReadFromJsonAsync<RundownDTO>();
            }
            else
            {
                Console.WriteLine($"Error updating control room: {response.ReasonPhrase}");
                return null;
            }
        }

        public async Task AddItemToRundownAsync(string rundownId, RundownItemDTO item)
        {
            var response = await _httpClient.PutAsJsonAsync($"add-item-to-rundown/{rundownId}", item);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully added item to rundown.");
            }
            else
            {
                Console.WriteLine($"Error adding item to rundown: {response.ReasonPhrase}");
            }
        }


    }

}

