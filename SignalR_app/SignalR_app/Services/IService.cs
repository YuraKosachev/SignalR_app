namespace SignalR_app.Services
{
    public interface IService
    {
        Task<string> Do(string message);
    }
}
