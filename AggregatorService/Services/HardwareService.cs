using AggregatorService.Abstractions;
using Microsoft.Extensions.Options;

namespace AggregatorService.Services
{
    public class HardwareService(HttpClient httpClient) : Aggregator
    {
        private readonly HttpClient _httpClient = httpClient;

        public override async Task<string> FetchData(string url)
        {            
            var response = await _httpClient.GetStringAsync(url);
            return response;
        }

        public override async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T payload)
        {
            var response = await _httpClient.PostAsJsonAsync(url, payload);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public override async Task<HttpResponseMessage> PutAsJsonAsync<T>(string url, T payload)
        {
            var response = await _httpClient.PutAsJsonAsync(url, payload);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public override async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();

            return response;
        }
    }

}
