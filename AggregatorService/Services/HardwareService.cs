namespace AggregatorService.Services
{
    public class HardwareService : Aggregator
    {
        private readonly HttpClient _httpClient;

        public HardwareService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<string> FetchData()
        {
            // Call the HardwareServiceAPI to fetch data
            var response = await _httpClient.GetStringAsync("https://localhost:7100/api/Hardware");
            return response;
        }
    }

}
