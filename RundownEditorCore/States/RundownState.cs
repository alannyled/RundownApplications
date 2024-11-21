using CommonClassLibrary.DTO;
using System;

namespace RundownEditorCore.States
{
    public class RundownState
    {
        public enum StateAction
        {
            StoryUpdated,
            RundownUpdated
        }
        public event Action<StateAction>? OnChange;
        public RundownStoryDTO Story { get; private set; } = new();
        public RundownDTO Rundown { get; private set; } = new();
        
        public void SelectedStory(RundownStoryDTO story)
        {
            Story = story;
            NotifyStateChanged(StateAction.StoryUpdated);
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
