using System.Text.Json.Serialization;

namespace CommonClassLibrary.DTO
{
    public class DetailDTO
    {
        public Guid UUID { get; set; } = Guid.NewGuid();
        public Guid ItemId { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
        public int Order { get; set; }
        public string? Duration { get; set; }
        public string? PrompterText { get; set; } = null;
        public string? VideoPath { get; set; } = null;
        public string? GraphicId { get; set; } = null;
        public string? Comment { get; set; } = null;
    }
}
