using Microsoft.Extensions.Caching.Memory;

namespace AggregatorService.Services
{
    public interface ICacheService
    {
        Task<T> GetDataAsync<T>(string cacheKey);
        void SetData<T>(string cacheKey, T data, TimeSpan time);
        void DeleteData(string cacheKey);
    }

    public class CacheService(IMemoryCache memoryCache) : ICacheService
    {
        private readonly IMemoryCache _memoryCache = memoryCache;

        public Task<T> GetDataAsync<T>(string cacheKey)
        {
            _memoryCache.TryGetValue(cacheKey, out T? data);
            return Task.FromResult(data ?? default!);
        }

        public void SetData<T>(string cacheKey, T data, TimeSpan time)
        {
            _memoryCache.Set(cacheKey, data, time);
        }
        public void DeleteData(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
        }
    }
}