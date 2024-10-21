namespace SignalR_app.Interfaces
{
    public interface IChatClient
    {
        Task ReceiveMessage(string message);
    }
}
