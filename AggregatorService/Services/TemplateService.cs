using AggregatorService.Abstractions;
using System.Net.Http;

namespace AggregatorService.Services
{
    public class TemplateService(HttpClient httpClient) : Aggregator
    {
        private readonly HttpClient _httpClient = httpClient;

        public override async Task<string> FetchData(string api)
        {
            var response = await _httpClient.GetAsync(api);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return content;
        }

        public async Task<string> GetByIdAsync(string api)
        {
            var response = await _httpClient.GetAsync(api);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return content;
        }

        public override async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T payload)
        {
            throw new NotImplementedException();
        }

        public override async Task<HttpResponseMessage> PutAsJsonAsync<T>(string url, T payload)
        {
            throw new NotImplementedException();
        }

        public override async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            throw new NotImplementedException();
        }
    }
}
