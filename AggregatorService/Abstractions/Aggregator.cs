namespace AggregatorService.Abstractions
{
    public abstract class Aggregator
    {
        public abstract Task<string> FetchData(string api);
        public abstract Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T payload);
        public abstract Task<HttpResponseMessage> PutAsJsonAsync<T>(string url, T payload);
        public abstract Task<HttpResponseMessage> DeleteAsync(string url);
    }
}
