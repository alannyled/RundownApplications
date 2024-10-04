using AggregatorService.Abstractions;
using AggregatorService.Models;
using System.Net.Http.Json;

namespace AggregatorService.Services
{
    public class ControlRoomService : Aggregator
    {
        private readonly HttpClient _httpClient;

        public ControlRoomService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<string> FetchData()
        {
            var response = await _httpClient.GetAsync("https://localhost:3020/api/ControlRoom");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return content;
        }

        public override async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T payload)
        {          
            var response = await _httpClient.PostAsJsonAsync(url, payload);
            response.EnsureSuccessStatusCode(); 

            return response;
        }
    }


}
