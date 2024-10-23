namespace RundownEditorCore.Services
{

    public class HealthCheckService : IDisposable
    {
        private readonly Dictionary<string, string> _services; 
        public Dictionary<string, bool> OnlineStatus { get; private set; } 
        private readonly HttpClient _httpClient;

        public HealthCheckService(Dictionary<string, string> services)
        {
            _services = services;
            OnlineStatus = new Dictionary<string, bool>();
            _httpClient = new HttpClient();
            foreach (var s in services)
            {
                OnlineStatus[s.Key] = false; // Initialize all as offline
            }
        }

        public async Task CheckDatabaseHealthAsync()
        {
            foreach (var s in _services)
            {
                try
                {
                    var response = await _httpClient.GetAsync(s.Value);
                    OnlineStatus[s.Key] = response.IsSuccessStatusCode;
                }
                catch (HttpRequestException)
                {
                    OnlineStatus[s.Key] = false;
                }
                catch (Exception)
                {
                    OnlineStatus[s.Key] = false;
                }
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
