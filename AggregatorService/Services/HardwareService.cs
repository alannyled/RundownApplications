using AggregatorService.Abstractions;
using Microsoft.Extensions.Options;

namespace AggregatorService.Services
{
    public class HardwareService(HttpClient httpClient) : Aggregator
    {
        private readonly HttpClient _httpClient = httpClient;

        public override async Task<string> FetchData(string api)
        {            
            var response = await _httpClient.GetStringAsync(api);
            return response;
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
