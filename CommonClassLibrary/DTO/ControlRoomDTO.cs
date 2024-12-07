﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClassLibrary.DTO
{
    public class ControlRoomDTO
    {
        public Guid Uuid { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ArchivedDate { get; set; }
        public string? ArchivedBy { get; set; } = null;
        public List<HardwareDTO> HardwareItems { get; set; } = [];
    }
}
