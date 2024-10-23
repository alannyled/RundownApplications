using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;

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
                OnlineStatus[s.Key] = false; // Init alle som offline
            }
        }
        /// <summary>
        /// Tjekker sundheden af både HTTP og TCP-baserede tjenester i listen.
        /// </summary>
        public async Task CheckHealthAsync()
        {
            foreach (var s in _services)
            {
                try
                {
                    var uri = new Uri(s.Value);

                    if (uri.Scheme == "http" || uri.Scheme == "https")
                    {
                        var response = await _httpClient.GetAsync(uri);
                        OnlineStatus[s.Key] = response.IsSuccessStatusCode;
                    }
                    else
                    {   // TCP eller andre ikke-HTTP tjenester
                        OnlineStatus[s.Key] = await PingHostAsync(uri.Host, uri.Port);
                    }
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

        /// <summary>
        /// Metode til at tjekke om en TCP port er åben (Feks. Kafka/ZooKeeper)
        /// </summary>
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

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
