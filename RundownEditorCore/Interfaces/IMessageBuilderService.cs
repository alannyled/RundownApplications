using CommonClassLibrary.DTO;

namespace RundownEditorCore.Interfaces
{
    public interface IMessageBuilderService
    {
        //string BuildRundownUpdateMessage(RundownDTO rundown);
        string BuildDetailLockMessage(DetailDTO detail, bool locked, string userName);
        string BuildLogMessage(string message, LogLevel logLevel);
        //string BuildItemMessage(RundownItemDTO item);
    }
}
