using AggregatorService.Abstractions;
using Microsoft.Extensions.Options;

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


        public override async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T payload)
        {
           throw new NotImplementedException();
        }
    }

}
