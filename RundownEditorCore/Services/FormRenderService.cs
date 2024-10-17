using Microsoft.AspNetCore.Components;
using RundownEditorCore.Components.Forms;
using RundownEditorCore.DTO;

namespace RundownEditorCore.Services
{
    public class FormRenderService
    {
        public RenderFragment RenderItemDetailForm(
            string DetailType,
            EventCallback<RundownDTO> onCreateCallback
            )
        {
            return builder =>
            {
                builder.OpenComponent(0, typeof(ItemDetailForm));
                builder.AddAttribute(2, "DetailType", DetailType);
                builder.AddAttribute(3, "OnItemDetailCreated", EventCallback.Factory.Create<RundownDTO>(this, onCreateCallback));
                builder.CloseComponent();
            };
        }

        public RenderFragment RenderRundownItemForm(
            List<string> Templates,
            RundownDTO SelectedRundown,
            EventCallback<RundownDTO> onCreateCallback
            )
        {
            return builder =>
            {
                builder.OpenComponent(0, typeof(RundownItemForm));
                builder.AddAttribute(1, "SelectedRundown", SelectedRundown);
                builder.AddAttribute(2, "Templates", Templates);
                builder.AddAttribute(3, "OnItemCreated", EventCallback.Factory.Create<RundownDTO>(this, onCreateCallback));
                builder.CloseComponent();
            };
        }

        public RenderFragment RenderNewRundownForm(
            List<TemplateDTO> AllTemplates, 
            List<ControlRoomDTO> ControlRooms, 
            DateTime BroadcastDate, 
            EventCallback<CreateRundownResultDTO> onCreateCallback
            )
        {
            return builder =>
            {
                builder.OpenComponent(0, typeof(CreateNewRundownForm));
                builder.AddAttribute(1, "Templates", AllTemplates);
                builder.AddAttribute(2, "ControlRooms", ControlRooms);
                builder.AddAttribute(3, "BroadcastDate", BroadcastDate);
                builder.AddAttribute(4, "OnRundownCreated", EventCallback.Factory.Create<CreateRundownResultDTO>(this, onCreateCallback));
                builder.CloseComponent();
            };
        }





    }
}
