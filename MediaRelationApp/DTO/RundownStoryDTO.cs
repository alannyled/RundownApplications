
namespace MediaRelationApp.DTO
{
    public class RundownStoryDTO
    {       
        public Guid UUID { get; set; }
        public string? RundownId { get; set; }
        public string? Name { get; set; }
        public string? Duration { get; set; }
        public int Order { get; set; }
        public List<DetailDTO> Details { get; set; } = [];
    }
}
