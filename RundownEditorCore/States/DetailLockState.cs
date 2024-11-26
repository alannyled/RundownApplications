using CommonClassLibrary.DTO;
using RundownEditorCore.DTO;
using RundownEditorCore.Services;

namespace RundownEditorCore.States
{
    public class DetailLockState
    {
        public event Action<DetailDTO, bool, string>? OnLockStateChanged;

        private readonly Dictionary<string, string> _lockedDetails = [];

        public bool IsLocked(string detailId, out string lockedByUserId)
        {
            if (_lockedDetails.TryGetValue(detailId, out var user))
            {
                lockedByUserId = user ?? "Unkown"; 
                return true;
            }

            lockedByUserId = string.Empty; 
            return false;
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

        public void ClearLocks()
        {
            _lockedDetails.Clear();
        }
    }
}
