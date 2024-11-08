namespace ControlRoomDbService.DTO
{
    public class CreateHardwareDto
    {
        public string Uuid { get; set; } = string.Empty;
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
