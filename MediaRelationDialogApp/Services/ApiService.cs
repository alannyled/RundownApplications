﻿using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommonClassLibrary.DTO;
using Newtonsoft.Json;

namespace MediaRelationDialogApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
           // _httpClient.BaseAddress = new Uri("https://localhost:3010/api/Rundown"); 
        }

        // GET: Hent en liste af records
        public async Task<List<RundownDTO>> GetRecordsAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:3010/api/Rundown/fetch-rundowns-with-controlrooms");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var rundowns = JsonConvert.DeserializeObject<List<RundownDTO>>(json);
            return rundowns;
        }


        public async Task<RundownDTO> UpdateDetailAsync(string rundownId, DetailDTO itemDetail)
        {
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:3010/api/Rundown/update-detail-in-item/{rundownId}", itemDetail);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<RundownDTO>();
            }
            return null;
        }
    }
}