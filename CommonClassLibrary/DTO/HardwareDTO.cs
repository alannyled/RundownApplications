using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClassLibrary.DTO
{
    public class HardwareDTO
    {
        public string Uuid { get; set; }
        public string? ControlRoomId { get; set; }
        public string? Name { get; set; }
        public string? Vendor { get; set; }
        public string? Model { get; set; }
        public string? MacAddress { get; set; }
        public string? IpAddress { get; set; }
        public int Port { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ArchivedDate { get; set; } = null;
        public string? ArchivedBy { get; set; } = null;
    }
}
