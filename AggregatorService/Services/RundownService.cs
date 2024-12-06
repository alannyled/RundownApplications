﻿using AggregatorService.Abstractions;
using Newtonsoft.Json;

namespace AggregatorService.Services
{
    public class RundownService(HttpClient httpClient) : Aggregator(httpClient)
    {
        public override async Task<string> FetchData(string api)
        {
            var response = await _httpClient.GetAsync(api);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return content;
        }

        // skal den også være en abstract metode?
        public async Task<string> GetByIdAsync(string api)
        {
            var response = await _httpClient.GetAsync(api);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return content;
        }       

        public override async Task<HttpResponseMessage> PutAsJsonAsync<T>(string url, T payload)
        {
            var json = JsonConvert.SerializeObject(payload);
            Console.WriteLine($"Updating rundown: {json}");
            var response = await _httpClient.PutAsJsonAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return response;
        }

        public override Task<HttpResponseMessage> DeleteAsync(string url)
        {
            throw new NotImplementedException();
        }

        public override async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T payload)
        {
            var response = await _httpClient.PostAsJsonAsync(url, payload);
            Console.WriteLine($"Rundown created: {JsonConvert.SerializeObject(response)}");
            response.EnsureSuccessStatusCode();
            return response;
        }
    }

}
