using System.Text.Json.Serialization;

namespace RundownEditorCore.DTO
{
    public class RundownItemDTO
    {       
        public Guid UUID { get; set; }
        public string RundownId { get; set; }
        public string Name { get; set; }
        public string Duration { get; set; }
        public int Order { get; set; }
        public List<ItemDetailDTO> ItemDetails { get; set; } = new List<ItemDetailDTO>();
    }
}
