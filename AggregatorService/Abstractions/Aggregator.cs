namespace AggregatorService.Abstractions
{
    public abstract class Aggregator(HttpClient httpClient)
    {
        protected readonly HttpClient _httpClient = httpClient;       
        public abstract Task<string> FetchData(string url);
        public abstract Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T payload);
        public abstract Task<HttpResponseMessage> PutAsJsonAsync<T>(string url, T payload);
        public abstract Task<HttpResponseMessage> DeleteAsync(string url);
        public virtual async Task<string> FetchData(string url, string id)
        {
            var response = await _httpClient.GetAsync($"{url}/{id}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

    }
}
