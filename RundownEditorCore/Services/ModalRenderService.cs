using Microsoft.AspNetCore.Components;
using RundownEditorCore.Components.Forms;

namespace RundownEditorCore.Services
{
    public class ModalRenderService
    {

        // modal variabler til form
        private string ItemName { get; set; } = string.Empty;
        private string Duration { get; set; } = "00:00:00";
        private string Category { get; set; } = string.Empty;

        public RenderFragment RundownItemFormContent => builder =>
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
}
