using Microsoft.AspNetCore.SignalR;
using Quark_Backend.Entities;

namespace Quark_Backend.Hubs
{
    public class QuarkHub : Hub
    {
        public async Task BroadcastUser(User user)
        {
            await Clients.All.SendAsync("ReceiveUser", user);
        }

        public async Task PushMessage(string message, string username)
        {
            await Clients.All.SendAsync("ReceiveMessage", message, username);
        }
    }
}
