
using Microsoft.AspNetCore.SignalR;
using SignalR_app.Hubs;
using SignalR_app.Interfaces;

namespace SignalR_app.Services
{
    public class NotificationService : INotificationService
    {
        IHubContext<ChatHub, IChatClient> _context;
        public NotificationService(IHubContext<ChatHub, IChatClient> context)
        {
            _context = context;
        }
        public async Task SendErrorMessage(string message)
        {
            await _context.Clients.All.ReceiveMessage($"[Error]-> {message}");
        }

        public  async Task SendInformationMessage(string message)
        {
            await _context.Clients.All.ReceiveMessage($"[Information]-> {message}");
        }
    }
}
