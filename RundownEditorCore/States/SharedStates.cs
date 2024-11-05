using CommonClassLibrary.DTO;

namespace RundownEditorCore.States
{
    /// <summary>
    /// Modtager beskeder fra KafkaConsumerClient og opdaterer SharedStates
    /// Begge er singletons,
    /// For at have et sted at gemme data der skal deles mellem flere scoped services
    /// </summary>
    public class SharedStates
    {

        public enum StateAction
        {
            ItemUpdated,
            RundownUpdated,
            NewRundownAdded,
            ControlRoomsUpdated,
            TemplatesUpdated,
            OnlineStatusUpdated
        }

        public event Action<StateAction>? OnChange;
        public RundownItemDTO ItemUpdated { get; private set; } = new();
        public RundownDTO RundownUpdated { get; private set; } = new();
        public RundownDTO NewRundown { get; private set; } = new();
        public List<ControlRoomDTO> ControlRooms { get; private set; } = [];
        public List<TemplateDTO> Templates { get; private set; } = [];
        public Dictionary<string, bool> OnlineStatus { get; private set; } = [];

        internal void NotifyStateChanged(StateAction action)
        {
            OnChange?.Invoke(action);
        }

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

        public void SharedControlRoom(List<ControlRoomDTO> controlRooms)
        {
            ControlRooms = controlRooms;
            NotifyStateChanged(StateAction.ControlRoomsUpdated);
        }

        public void SharedTemplates(List<TemplateDTO> templates)
        {
            Templates = templates;
            NotifyStateChanged(StateAction.TemplatesUpdated);
        }

        public void SharedOnlineStatus(Dictionary<string, bool> onlineStatus)
        {
            OnlineStatus = onlineStatus;
            NotifyStateChanged(StateAction.OnlineStatusUpdated);
        }

       
    }
}
