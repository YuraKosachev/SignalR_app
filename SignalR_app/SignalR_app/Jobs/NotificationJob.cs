using Coravel.Invocable;
using Microsoft.AspNetCore.SignalR;
using SignalR_app.Hubs;
using SignalR_app.Interfaces;
using System.Diagnostics;

namespace SignalR_app.Jobs
{
    public class NotificationJob : IInvocable
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IServiceScopeFactory _serviceFactory;
        public NotificationJob(IHttpClientFactory httpClientFactory,
            IServiceScopeFactory serviceFactory,
            IHubContext<NotificationHub> hubContext)
        {
            _httpClientFactory = httpClientFactory;
            _hubContext = hubContext;
            _serviceFactory = serviceFactory;
        }

        public async Task Invoke()
        {
            //Progress bar section 
            var list = Enumerable.Range(1, 55385).ToList();


            for (int i = 0, j = 1; i < list.Count; i++, j++)
            {
                if (j % 5 == 0)
                    await _hubContext.Clients.All.SendAsync($"Count {j}/{list.Count}");
            }

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://spystationapi.onrender.com/spy/pulse/ping");
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(httpRequestMessage);
            if (response.IsSuccessStatusCode)
            {
                await _hubContext.Clients.All.SendAsync($"Pulse checked! Heart is beating status code: {response.StatusCode}");
                // _logger.LogInformation("Pulse checked! Heart is beating status code: {0}", response.StatusCode);
            }
        }
    }
}
