using System.Text.Json.Serialization;

namespace RundownEditorCore.DTO
{
    public class StoryDetailDTO
    {
        public class StoryDetail
        {
            public Guid UUID { get; set; }
            public Guid StoryId { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Duration { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public int Order { get; set; }
        }


        public class StoryDetailTeleprompter : StoryDetail
        {
            public string PrompterText { get; set; } = string.Empty;
        }


        public class StoryDetailVideo : StoryDetail
        {           
            public string VideoPath { get; set; } = string.Empty;            
        }


        public class StoryDetailGraphic : StoryDetail
        {
            public string GraphicId { get; set; } = string.Empty;
        }


        public class StoryDetailComment : StoryDetail
        {
            public string Comment { get; set; } = string.Empty;

        }
    }
}
