namespace RundownDbService.Data
{
    public class Hardware
    {
        public int HardwareId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string DnsName { get; set; }
        public string IpAddress { get; set; }
        public string SubnetMask { get; set; }
        public string Gateway { get; set; }
        public string Port { get; set; }
        public string MacAddress { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }

        public List<ControlRoom> ControlRooms { get; set; } = new List<ControlRoom>();

    }
}
