using System.Text.Json.Serialization;

namespace RundownEditorCore.DTO
{
    public class StoryDetailDTO
    {
        public class StoryDetail
        {
            public Guid UUID { get; set; }
            public Guid StoryId { get; set; }
            public string Title { get; set; }
            public string Duration { get; set; }
            public string Type { get; set; }
            public int Order { get; set; }
        }


        public class StoryDetailTeleprompter : StoryDetail
        {
            public string PrompterText { get; set; }
        }


        public class StoryDetailVideo : StoryDetail
        {           
            public string VideoPath { get; set; }            
        }


        public class StoryDetailGraphic : StoryDetail
        {
            public string GraphicId { get; set; }
        }


        public class StoryDetailComment : StoryDetail
        {
            public string Comment { get; set; }

        }
    }
}
