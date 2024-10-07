using AggregatorService.Abstractions;
using AggregatorService.DTO;
using AggregatorService.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AggregatorService.Services
{
    public class RundownService(HttpClient httpClient) : Aggregator
    {
        private readonly HttpClient _httpClient = httpClient;    

        public override async Task<string> FetchData(string api)
        {
            var response = await _httpClient.GetAsync(api);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return content;
        }

        // skal den også være en abstract metode?
        public async Task<string> GetByIdAsync(string api)
        {
            var response = await _httpClient.GetAsync(api);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return content;
        }

        public async Task<HttpResponseMessage> PutAsJsonAsync(string url, RundownDTO rundown)
        {
            var json = JsonSerializer.Serialize(rundown);
            Console.WriteLine("JSON Payload: " + json);

            var response = await _httpClient.PutAsJsonAsync(url, rundown);
            response.EnsureSuccessStatusCode();
            return response;
        }




        public override async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T payload)
        {
           throw new NotImplementedException();
        }
    }

}
