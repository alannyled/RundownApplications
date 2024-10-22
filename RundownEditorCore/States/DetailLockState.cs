namespace RundownEditorCore.States
{
    public class DetailLockState
    {
        private readonly HashSet<string> _lockedDetails = [];

        public event Action<string, bool> OnLockStateChanged;
    
        public bool IsLocked(string detailId)
        {
            return _lockedDetails.Contains(detailId);
        }
        public void SetLockState(string detailId, bool isLocked)
        {
            if (isLocked)
            {
                if (_lockedDetails.Add(detailId)) 
                {
                    OnLockStateChanged?.Invoke(detailId, true);
                }
            }
            else
            {
                if (_lockedDetails.Remove(detailId)) 
                {
                    OnLockStateChanged?.Invoke(detailId, false);
                }
            }
        }
    }

}
