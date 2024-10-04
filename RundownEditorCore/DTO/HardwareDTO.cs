namespace RundownEditorCore.DTO
{
    public class HardwareDTO
    {
        public string Uuid { get; set; }
        public string ControlRoomId { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string MacAddress { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
    }
}
