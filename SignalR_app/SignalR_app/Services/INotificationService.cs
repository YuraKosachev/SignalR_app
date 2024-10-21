namespace SignalR_app.Services
{
    public interface INotificationService
    {
        Task SendErrorMessage(string message);
        Task SendInformationMessage(string message);
    }
}
