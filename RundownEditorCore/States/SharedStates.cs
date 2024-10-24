using CommonClassLibrary.DTO;
using Microsoft.Extensions.Logging.Console;

namespace RundownEditorCore.States
{
    public class SharedStates
    {
        public event Action OnChange;
        public RundownItemDTO Item { get; private set; } = new();
        public RundownDTO Rundown { get; private set; } = new();

        public void SharedItem(RundownItemDTO item)
        {
            Item = item;
            NotifyStateChanged();
        }
        public void SharedRundown(RundownDTO rundown)
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
