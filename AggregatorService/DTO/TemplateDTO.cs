﻿using System.Text.Json.Serialization;

namespace AggregatorService.DTO
{
    public class TemplateDTO
    {
        [JsonPropertyName("uuid")]
        public Guid Uuid { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; }
    }
}
