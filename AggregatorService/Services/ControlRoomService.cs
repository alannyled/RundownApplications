using AggregatorService.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace AggregatorService.Services
{
    public class ControlRoomService(HttpClient httpClient, IOptions<ApiUrls> apiUrls, ICacheService cacheService) : Aggregator(httpClient)
    {
        private readonly ApiUrls _apiUrls = apiUrls.Value;
        private readonly ICacheService _cacheService = cacheService;

        private const string ControlRoomCacheKey = "ControlRoomCacheKey";
        public override async Task<string> FetchData(string url)
        {
            var cachedData = await _cacheService.GetDataAsync<string>(ControlRoomCacheKey);
            if (cachedData != null) return cachedData;
       
            var response = await _httpClient.GetStringAsync(url);
            _cacheService.SetData(ControlRoomCacheKey, response, TimeSpan.FromHours(3));
            return response;
        }

        public override async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T payload)
        {
            var response = await _httpClient.PostAsJsonAsync(url, payload);
            response.EnsureSuccessStatusCode();

            await UpdateCacheAfterChange();

            return response;
        }

        public override async Task<HttpResponseMessage> PutAsJsonAsync<T>(string url, T payload)
        {
            var response = await _httpClient.PutAsJsonAsync(url, payload);
            response.EnsureSuccessStatusCode();

            await UpdateCacheAfterChange();

            return response;
        }

        public override async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();

            await UpdateCacheAfterChange();

            return response;
        }

        private async Task UpdateCacheAfterChange()
        {
            var updatedData = await _httpClient.GetStringAsync(_apiUrls.ControlRoomApi);
            _cacheService.SetData(ControlRoomCacheKey, updatedData, TimeSpan.FromHours(3));
        }
    }
}
