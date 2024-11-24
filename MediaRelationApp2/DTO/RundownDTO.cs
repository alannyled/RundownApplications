
namespace MediaRelationApp2.DTO
{
    public class RundownDTO
    {
        public string UUID{ get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = null;
        public string? Type{ get; set; } = null;
        public DateTimeOffset BroadcastDate { get; set; } = new();
        public DateTime? ArchivedDate { get; set; } = null;
        public string? ArchivedBy { get; set; } = null;
        public string ControlRoomId { get; set; } = string.Empty;
        public string? ControlRoomName { get; set;} = null;
        public List<RundownStoryDTO> Stories { get; set; } = [];
    }
}
