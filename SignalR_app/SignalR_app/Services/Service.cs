
using Microsoft.AspNetCore.SignalR;
using SignalR_app.Hubs;
using SignalR_app.Interfaces;

namespace SignalR_app.Services
{
    public class Service : IService
    {
        IHubContext<ChatHub, IChatClient> _context;
        public Service(IHubContext<ChatHub, IChatClient> context)
        {
            _context = context;
        }
        public async Task<string> Do(string message)
        {
            await Task.Delay(1000);

            await _context.Clients.All.ReceiveMessage($"[Error]-> {message}");

            return message;
        }
    }
}
