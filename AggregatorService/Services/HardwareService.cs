﻿using AggregatorService.Abstractions;
using Microsoft.Extensions.Options;

namespace AggregatorService.Services
{
    public class HardwareService(HttpClient httpClient, IOptions<ApiUrls> apiUrls) : Aggregator
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ApiUrls _apiUrls = apiUrls.Value;

        public override async Task<string> FetchData()
        {            
            var response = await _httpClient.GetStringAsync(_apiUrls.HardwareApi);
            return response;
        }

        public override async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T payload)
        {
            throw new NotImplementedException();
        }
    }

}