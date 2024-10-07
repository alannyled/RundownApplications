namespace RundownDbService.DTO
{
    public class RundownDTO
    {
        public string? Uuid { get; set; }
        public string? ControlRoomId { get; set; }
        public List<RundownItemDTO> Items { get; set; } = new List<RundownItemDTO>();
    }
}
