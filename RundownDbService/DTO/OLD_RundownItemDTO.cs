﻿using CommonClassLibrary.DTO;

namespace RundownDbService.DTO
{
    public class OLD_RundownItemDTO
    {
        public Guid UUID { get; set; }
        public Guid RundownId { get; set; }
        public string Name { get; set; }
        public string Duration { get; set; }
        public int Order { get; set; }
        public List<DetailDTO> Details { get; set; } = new List<DetailDTO>();
    }
}