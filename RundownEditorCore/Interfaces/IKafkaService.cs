namespace RundownEditorCore.Interfaces
{
    public interface IKafkaService
    {
        void SendMessage(string topic, string message);
    }
}
