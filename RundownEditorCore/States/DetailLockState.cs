using RundownEditorCore.DTO;
using RundownEditorCore.Services;

namespace RundownEditorCore.States
{
    public class DetailLockState
    {
        private readonly Dictionary<string, string> _lockedDetails = new Dictionary<string, string>();

        public event Action<DetailDTO, bool, string> OnLockStateChanged;
        public bool IsLocked(string detailId, out string lockedByUserId)
        {
            return _lockedDetails.TryGetValue(detailId, out lockedByUserId);
        }

        public bool SetLockState(DetailDTO detail, bool isLocked, string user)
        {
            string detailId = detail.UUID.ToString();

            if (isLocked)
            {
                if (!_lockedDetails.ContainsKey(detailId))
                {
                    _lockedDetails[detailId] = user; 
                    OnLockStateChanged?.Invoke(detail, true, user);
                    return true; 
                }
                return false;
            }
            else
            {
                // Kun brugeren, der har låst detail, kan låse den op
                if (_lockedDetails.TryGetValue(detailId, out var lockedBy) && lockedBy == user)
                {
                    _lockedDetails.Remove(detailId);
                    OnLockStateChanged?.Invoke(detail, false, user);
                    return true; 
                }
                return false; 
            }
        }
    }
}
