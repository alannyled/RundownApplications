﻿using System.Text.Json.Serialization;

namespace RundownEditorCore.DTO
{
    public class RundownItemDTO
    {       
        public Guid UUID { get; set; }
        public Guid RundownId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public List<ItemDetailDTO> ItemDetails { get; set; } = new List<ItemDetailDTO>();
    }
}