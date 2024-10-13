using AggregatorService.Abstractions;
using AggregatorService.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace AggregatorService.Services
{
    public class HardwareService(HttpClient httpClient, IOptions<ApiUrls> apiUrls, ICacheService cacheService) : Aggregator(httpClient)
    {
        private readonly ApiUrls _apiUrls = apiUrls.Value;
        private readonly ICacheService _cacheService = cacheService;

        private const string HardwareCacheKey = "HardwareCacheKey";

        public override async Task<string> FetchData(string url)
        {
            var cachedData = await _cacheService.GetDataAsync<string>(HardwareCacheKey);
            if (cachedData != null) return cachedData;
            
            var response = await _httpClient.GetStringAsync(url);
            _cacheService.SetData(HardwareCacheKey, response, TimeSpan.FromHours(3));
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
            var updatedData = await _httpClient.GetStringAsync(_apiUrls.HardwareApi);
            _cacheService.SetData(HardwareCacheKey, updatedData, TimeSpan.FromHours(3));
        }
    }
}
