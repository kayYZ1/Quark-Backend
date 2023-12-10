using Microsoft.AspNetCore.SignalR;
using Quark_Backend.Entities;
namespace Quark_Backend.Hubs
{
    public class QuarkHub : Hub
    {
        public void BroadcastUser(User user)
        {
            Clients.All.SendAsync("ReceiveUser", user);
        }
        public void BroadcastMessage(string message)
        {
            Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
