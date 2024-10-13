using AggregatorService.Abstractions;
using System.Net.Http;

namespace AggregatorService.Services
{
    public class TemplateService(HttpClient httpClient) : Aggregator(httpClient)
    {
        public override async Task<string> FetchData(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return content;
        }

        public async Task<string> GetByIdAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);
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
