namespace RundownEditorCore.DTO
{
    public class RundownDTO
    {
        public string Uuid { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Type{ get; set; }
        public DateTimeOffset BroadcastDate { get; set; }
        public DateTime? ArchivedDate { get; set; } = null;
        public string? ArchivedBy { get; set; } = null;
        public string ControlRoomId { get; set; }
        public string ControlRoomName { get; set;}
        public List<RundownItemDTO> Items { get; set; } = new List<RundownItemDTO>();
    }
}
