
namespace CommonClassLibrary.DTO
{
    public class RundownDTO
    {
        public string Uuid { get; set; }
        public string? Name { get; set; } = null;
        public string? Description { get; set; } = null;
        public string? Type{ get; set; } = null;
        public DateTimeOffset? BroadcastDate { get; set; } = null;
        public DateTime? ArchivedDate { get; set; } = null;
        public string? ArchivedBy { get; set; } = null;
        public string? ControlRoomId { get; set; } = null;
        public string? ControlRoomName { get; set;} = null;
        public List<RundownItemDTO> Items { get; set; } = [];
    }
}
