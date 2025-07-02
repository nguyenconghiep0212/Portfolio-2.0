using Microsoft.AspNetCore.SignalR;

namespace SignalRWebpack.Hubs;

public class IncidentHub : Hub
{
    public async Task SendMessage(string username, string message) =>
        await Clients.All.SendAsync("messageReceived", username, message);
}