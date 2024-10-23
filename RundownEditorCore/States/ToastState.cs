namespace RundownEditorCore.States
{
    public class ToastState
    {
        public event Action OnChange;
        public bool Show { get; private set; } = false;
        public string Message { get; private set; } = string.Empty;
        public string Color { get; private set; } = "text-bg-success";

        public void FireToast(string message, string color)
        {
            Message = message;
            Color = color;
            Show = true;            
            NotifyStateChanged();
            _ = AutoClose();
        }

        public void Close()
        {
            Show = false;
            Message = string.Empty;
            Color = "text-bg-success";
            NotifyStateChanged();
        }

        public async Task AutoClose()
        {
            await Task.Delay(5000);
            Close();
        }

        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }
    }
}
