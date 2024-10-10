using Microsoft.AspNetCore.Components;
using RundownEditorCore.Components.Forms;
using RundownEditorCore.DTO;

namespace RundownEditorCore.Services
{
    public class ModalRenderService
    {

        public RenderFragment RenderRundownItemForm(string ItemName, string Duration, string Category)
        {
            return builder =>
            {
                builder.OpenComponent(0, typeof(RundownItemForm));
                builder.AddAttribute(1, "ItemName", ItemName);
                builder.AddAttribute(2, "Duration", Duration);
                builder.AddAttribute(3, "Category", Category);
                builder.AddAttribute(5, "ItemNameChanged", EventCallback.Factory.Create<string>(this, value => ItemName = value));
                builder.AddAttribute(6, "DurationChanged", EventCallback.Factory.Create<string>(this, value => Duration = value));
                builder.AddAttribute(7, "CategoryChanged", EventCallback.Factory.Create<string>(this, value => Category = value));
                builder.CloseComponent();
            };
        }

        public RenderFragment RenderNewRundownForm(List<TemplateDTO> AllTemplates, List<ControlRoomDTO> controlRooms, DateTime BroadcastDate)
        {
            return builder =>
            {
                builder.OpenComponent(0, typeof(CreateNewRundownForm));
                builder.AddAttribute(1, "Templates", AllTemplates);
                builder.AddAttribute(2, "ControlRooms", controlRooms);
                builder.AddAttribute(3, "BroadcastDate", BroadcastDate);
                builder.CloseComponent();
            };
        }





    }
}
