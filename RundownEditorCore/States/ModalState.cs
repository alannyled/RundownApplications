using Microsoft.AspNetCore.Components;
using System;
using System.Threading;

namespace RundownEditorCore.States
{
    public class ModalState
    {    

        public event Action OnChange;
        public bool Show { get; private set; } = false;
        public string Title { get; private set; } = string.Empty;
        public RenderFragment Content { get; private set; }
        public void SetContent(RenderFragment content, string title)
        {
            Content = content;
            Title = title;
            Show = true;
            NotifyStateChanged();
        }
        public void Close()
        {
            Show = false;
            Content = null;
            NotifyStateChanged();
        }
        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }
    }
}
