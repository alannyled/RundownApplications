namespace AggregatorService.Services
{
    public class ControlRoomService : Aggregator
    {
        private readonly HttpClient _httpClient;

        public ControlRoomService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<string> FetchData()
        {
            Console.WriteLine("Calling ControlRoomService...");
            var response = await _httpClient.GetAsync("https://localhost:7100/api/ControlRoom");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            // Log or print the response content
            Console.WriteLine($"Response from ControlRoomService: {content}");

            return content;
        }
    }


}
