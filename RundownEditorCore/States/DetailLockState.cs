namespace RundownEditorCore.States
{
    public class DetailLockState
    {
        // Dictionary til at gemme detailId som nøgle og userId som værdi
        private readonly Dictionary<string, string> _lockedDetails = new Dictionary<string, string>();

        public event Action<string, bool, string> OnLockStateChanged;

        // Metode til at kontrollere om et detailId er låst, og hvem der har låst det
        public bool IsLocked(string detailId, out string lockedByUserId)
        {
            return _lockedDetails.TryGetValue(detailId, out lockedByUserId);
        }

        // Opdaterer lock state og gemmer, hvem der har låst/unlocked detaljen
        public bool SetLockState(string detailId, bool isLocked, string userId)
        {
            if (isLocked)
            {
                // Kontroller, om det allerede er låst af en anden bruger
                if (!_lockedDetails.ContainsKey(detailId))
                {
                    _lockedDetails[detailId] = userId; // Tilføj detailId og userId til dictionary
                    OnLockStateChanged?.Invoke(detailId, true, userId);
                    return true; // Låsning lykkedes
                }
                else
                {
                    return false; // Detalje er allerede låst
                }
            }
            else
            {
                // Kun brugeren, der har låst detaljen, kan låse den op
                if (_lockedDetails.TryGetValue(detailId, out var lockedBy) && lockedBy == userId)
                {
                    _lockedDetails.Remove(detailId);
                    OnLockStateChanged?.Invoke(detailId, false, userId);
                    return true; // Låsning fjernet
                }
                else
                {
                    return false; // Brugeren kan ikke låse denne detalje op, da de ikke er den, der låste den
                }
            }
        }
    }
}
