using Microsoft.AspNetCore.Components;
using RundownEditorCore.DTO;

namespace RundownEditorCore.States
{
    public class RundownState
    {
        public event Action OnChange;
        public RundownItemDTO Item { get; private set; } = new();
        public RundownDTO Rundown { get; private set; } = new();

        public void SelectedItem(RundownItemDTO item)
        {
            Item = item;
            NotifyStateChanged();
        }
        public void SelectedRundown(RundownDTO rundown)
        {
            Rundown = rundown;
            NotifyStateChanged();
        }
        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }
    }
}
