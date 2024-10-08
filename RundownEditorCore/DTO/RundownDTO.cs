namespace RundownEditorCore.DTO
{
    public class RundownDTO
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Type{ get; set; }
        public DateTime BroadcastDate { get; set; }
        public DateTime? ArchivedDate { get; set; } = null;
        public string? ArchivedBy { get; set; } = null;
        public Guid ControlRoomId { get; set; }
        public string ControlRoomName { get; set;}
        public List<RundownItemDTO> Items { get; set; } = new List<RundownItemDTO>();
    }
}
