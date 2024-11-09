using CommonClassLibrary.DTO;
using System;

namespace RundownEditorCore.States
{
    public class RundownState
    {
        public enum StateAction
        {
            ItemUpdated,
            RundownUpdated
        }
        public event Action<StateAction>? OnChange;
        public RundownItemDTO Item { get; private set; } = new();
        public RundownDTO Rundown { get; private set; } = new();
        
        public void SelectedItem(RundownItemDTO item)
        {
            Item = item;
            NotifyStateChanged(StateAction.ItemUpdated);
        }
        public void SelectedRundown(RundownDTO rundown)
        {
            Rundown = rundown;
            NotifyStateChanged(StateAction.RundownUpdated);
        }
        private void NotifyStateChanged(StateAction action)
        {
            OnChange?.Invoke(action);
        }
    }
}
