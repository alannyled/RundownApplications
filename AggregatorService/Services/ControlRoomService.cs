using AggregatorService.Abstractions;
using Microsoft.Extensions.Options;

namespace AggregatorService.Services
{
    public class ControlRoomService(HttpClient httpClient, IOptions<ApiUrls> apiUrls) : Aggregator
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ApiUrls _apiUrls = apiUrls.Value;

        public override async Task<string> FetchData()
        {
            var response = await _httpClient.GetAsync(_apiUrls.ControlRoomApi);
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
