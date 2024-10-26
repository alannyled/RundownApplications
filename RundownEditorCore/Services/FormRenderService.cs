using Microsoft.AspNetCore.Components;
using RundownEditorCore.Components.Forms;
using CommonClassLibrary.DTO;
using RundownEditorCore.DTO;

namespace RundownEditorCore.Services
{
    public class FormRenderService
    {
        public RenderFragment RenderItemDetailForm(
            string DetailType
            )
        {
            return builder =>
            {
                builder.OpenComponent(0, typeof(ItemDetailForm));
                builder.AddAttribute(1, "DetailType", DetailType);
                builder.CloseComponent();
            };
        }

        public RenderFragment RenderRundownItemForm(
            List<string> Templates
            )
        {
            return builder =>
            {
                builder.OpenComponent(0, typeof(RundownItemForm));
                builder.AddAttribute(1, "Templates", Templates);
                builder.CloseComponent();
            };
        }

        public RenderFragment RenderNewRundownForm(
            List<TemplateDTO> AllTemplates, 
            List<ControlRoomDTO> ControlRooms, 
            DateTime BroadcastDate
            )
        {
            return builder =>
            {
                builder.OpenComponent(0, typeof(CreateNewRundownForm));
                builder.AddAttribute(1, "Templates", AllTemplates);
                builder.AddAttribute(2, "ControlRooms", ControlRooms);
                builder.AddAttribute(3, "BroadcastDate", BroadcastDate);
                builder.CloseComponent();
            };
        }





    }
}
