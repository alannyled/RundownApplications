using CommonClassLibrary.DTO;
using Microsoft.Extensions.Logging.Console;

namespace RundownEditorCore.States
{
    /// <summary>
    /// Modtager beskeder fra KafkaConsumerClient og opdaterer SharedStates
    /// Begge er singletons,
    /// så det er for at have et sted at gemme data der skal deles mellem flere scoped services
    /// </summary>
    public class SharedStates
    {

        public enum StateAction
        {
            ItemUpdated,
            RundownUpdated,
            NewRundownAdded
        }

        public event Action<StateAction> OnChange;
        public RundownItemDTO ItemUpdated { get; private set; } = new();
        public RundownDTO RundownUpdated { get; private set; } = new();

        public RundownDTO NewRundown { get; private set; } = new();

        public void SharedItem(RundownItemDTO item)
        {
            ItemUpdated = item;
            NotifyStateChanged(StateAction.ItemUpdated);
        }
        public void SharedRundown(RundownDTO rundown)
        {
            RundownUpdated = rundown;
            NotifyStateChanged(StateAction.RundownUpdated);
        }

        public void SharedNewRundown(RundownDTO rundown)
        {
            NewRundown = rundown;
            NotifyStateChanged(StateAction.NewRundownAdded);
        }

        private void NotifyStateChanged(StateAction action)
        {
            OnChange?.Invoke(action);
        }
    }
}
