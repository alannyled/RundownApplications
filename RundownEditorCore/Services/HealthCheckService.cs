using System.Net.Sockets;
using Microsoft.Extensions.Hosting;
using RundownEditorCore.States;
using System.Threading;
using System.Threading.Tasks;

namespace RundownEditorCore.Services
{
    public class HealthCheckService : BackgroundService, IDisposable
    {
        private readonly Dictionary<string, string> _services;
        private readonly SharedStates _sharedStates;
        private readonly HttpClient _httpClient;

        public HealthCheckService(Dictionary<string, string> services, SharedStates sharedStates)
        {
            _services = services;
            _sharedStates = sharedStates;
            _httpClient = new HttpClient();
            _sharedStates.SharedOnlineStatus(_services.ToDictionary(s => s.Key, _ => false)); // Init alle som offline
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var updatedStatus = await CheckHealthAsync();
                _sharedStates.SharedOnlineStatus(updatedStatus);
                _sharedStates.NotifyStateChanged(SharedStates.StateAction.OnlineStatusUpdated);

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); // Kør hvert 30. sekund
            }
        }

        public async Task<Dictionary<string, bool>> CheckHealthAsync()
        {
            var updatedStatus = new Dictionary<string, bool>();

            foreach (var s in _services)
            {
                try
                {
                    var uri = new Uri(s.Value);

                    if (uri.Scheme == "http" || uri.Scheme == "https")
                    {
                        var response = await _httpClient.GetAsync(uri);
                        updatedStatus[s.Key] = response.IsSuccessStatusCode;
                    }
                    else
                    {
                        updatedStatus[s.Key] = await PingHostAsync(uri.Host, uri.Port);
                    }
                }
                catch
                {
                    updatedStatus[s.Key] = false;
                }
            }
            return updatedStatus;
        }

        private async Task<bool> PingHostAsync(string host, int port)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    await client.ConnectAsync(host, port);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public override void Dispose()
        {
            _httpClient.Dispose();
            base.Dispose();
        }
    }
}
