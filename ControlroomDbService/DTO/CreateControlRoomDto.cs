namespace ControlRoomDbService.DTO
{
    public class CreateControlRoomDto
    {
        public string? Name { get; set; }
        public string? Location { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
