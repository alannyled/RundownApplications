using Microsoft.AspNetCore.Components;
using RundownEditorCore.Components.Forms;
using CommonClassLibrary.DTO;
using RundownEditorCore.DTO;
using Microsoft.Identity.Client;

namespace RundownEditorCore.Services
{
    public class FormRenderService
    {
        public RenderFragment RenderDeleteStoryForm(
            RundownStoryDTO Story
                       )
        {
            return builder =>
            {
                builder.OpenComponent(0, typeof(DeleteStoryForm));
                builder.AddAttribute(1, "Story", Story);
                builder.CloseComponent();
            };
        }
        public RenderFragment RenderStoryDetailForm(
            string DetailType
            )
        {
            return builder =>
            {
                builder.OpenComponent(0, typeof(StoryDetailForm));
                builder.AddAttribute(1, "DetailType", DetailType);
                builder.CloseComponent();
            };
        }

        public RenderFragment RenderRundownStoryForm(
            List<string> Templates
            )
        {
            return builder =>
            {
                builder.OpenComponent(0, typeof(RundownStoryForm));
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
