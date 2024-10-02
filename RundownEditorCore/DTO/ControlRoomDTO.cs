namespace RundownEditorCore.DTO
{
    public class ControlRoomDTO
    {
        public string Uuid { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public List<HardwareDTO> HardwareItems { get; set; }
    }    

}
