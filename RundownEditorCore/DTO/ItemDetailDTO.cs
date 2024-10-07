using System.Text.Json.Serialization;

namespace RundownEditorCore.DTO
{
    public class ItemDetailDTO
    {
        public class ItemDetail
        {
            public Guid UUID { get; set; }
            public Guid ItemId { get; set; }
            public string Type { get; set; }
            public int Order { get; set; }
        }


        public class ItemDetailTeleprompter : ItemDetail
        {
            public string PrompterText { get; set; }
            public string DetailType { get; set; } = "ItemDetailTeleprompter";
        }


        public class ItemDetailVideo : ItemDetail
        {
            public string title { get; set; }
            public string VideoPath { get; set; }
            public TimeSpan Duration { get; set; }
            public string DetailType { get; set; } = "ItemDetailVideo";
        }


        public class ItemDetailGraphic : ItemDetail
        {
            public string GraphicId { get; set; }
            public TimeSpan Duration { get; set; }
            public string DetailType { get; set; } = "ItemDetailGraphic";
        }


        public class ItemDetailComment : ItemDetail
        {
            public string Comment { get; set; }
            public string DetailType { get; set; } = "ItemDetailComment";

        }
    }
}
