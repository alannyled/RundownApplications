namespace CommonClassLibrary.Interfaces
{
    public interface ILogService
    {
        void SendMessage(string topic, string message);
    }
}
