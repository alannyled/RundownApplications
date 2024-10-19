using System.Text.Json.Serialization;

namespace RundownEditorCore.DTO
{
    public class ItemDetailDTO
    {
        public class ItemDetail
        {
            public Guid UUID { get; set; }
            public Guid ItemId { get; set; }
            public string Title { get; set; }
            public string Duration { get; set; }
            public string Type { get; set; }
            public int Order { get; set; }
        }


        public class ItemDetailTeleprompter : ItemDetail
        {
            public string PrompterText { get; set; }
        }


        public class ItemDetailVideo : ItemDetail
        {           
            public string VideoPath { get; set; }            
        }


        public class ItemDetailGraphic : ItemDetail
        {
            public string GraphicId { get; set; }
        }


        public class ItemDetailComment : ItemDetail
        {
            public string Comment { get; set; }

        }
    }
}
