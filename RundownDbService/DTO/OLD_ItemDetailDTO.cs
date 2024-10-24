namespace RundownDbService.DTO
{
    public class OLD_ItemDetailDTO
    {
        public Guid UUID { get; set; }
        public Guid ItemId { get; set; }
        public string Type { get; set; }
        public int Order { get; set; }
        public string? PrompterText { get; set; }
        public string Title { get; set; }
        public string? VideoPath { get; set; }
        public string Duration { get; set; }
        public string? GraphicId { get; set; }
        public string? Comment { get; set; }
    }
}
