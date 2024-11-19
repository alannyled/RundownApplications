using Microsoft.AspNetCore.Components;
using RundownEditorCore.Components.Forms;
using CommonClassLibrary.DTO;
using RundownEditorCore.DTO;
using Microsoft.Identity.Client;

namespace RundownEditorCore.Services
{
    public class FormRenderService
    {
        public RenderFragment RenderDeleteItemForm(
            RundownItemDTO Item
                       )
        {
            return builder =>
            {
                builder.OpenComponent(0, typeof(DeleteItemForm));
                builder.AddAttribute(1, "Item", Item);
                builder.CloseComponent();
            };
        }
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
            DateTime BroadcastDate
            )
        {
            return builder =>
            {
                builder.OpenComponent(0, typeof(CreateNewRundownForm));
                builder.AddAttribute(1, "BroadcastDate", BroadcastDate);
                builder.CloseComponent();
            };
        }





    }
}
