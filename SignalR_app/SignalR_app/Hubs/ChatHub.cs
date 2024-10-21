using Microsoft.AspNetCore.SignalR;
using SignalR_app.Interfaces;

namespace SignalR_app.Hubs
{
    public sealed class ChatHub : Hub<IChatClient> //Hub
    {
        //message {"protocol":"json","version":1} - it needs to 1'st send to connect 
        public override async Task OnConnectedAsync()
        {
            // await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined");
            await Clients.All.ReceiveMessage($"{Context.ConnectionId} has joined");
        }

        //{"arguments":["Test message"], "invocationId":"0", "target":"SendMessageAsync","type":1}
        public async Task SendMessage(string message) {
            await Clients.Client(Context.ConnectionId).ReceiveMessage($"{Context.ConnectionId}:{message}");
            //await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId}:{message}");
        }
    }
}


//public static class HubProtocolConstants
//{
//    public const int InvocationMessageType = 1;
//    public const int StreamItemMessageType = 2;
//    public const int CompletionMessageType = 3;
//    public const int StreamInvocationMessageType = 4;
//    public const int CancelInvocationMessageType = 5;
//    public const int PingMessageType = 6;
//    public const int CloseMessageType = 7;
//}