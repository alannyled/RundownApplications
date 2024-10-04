namespace AggregatorService.Abstractions
{
    public abstract class Aggregator
    {
        public abstract Task<string> FetchData();
        public abstract Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T payload);

    }

}
