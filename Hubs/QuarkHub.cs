using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Quark_Backend.DAL;
using Quark_Backend.Entities;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Query;

namespace Quark_Backend.Hubs
{
    public class QuarkHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            using (var db = new QuarkDbContext())
            {
                var user = await db.Users
                    .Include(u => u.Connections)
                    .FirstAsync(u => u.Username == Context.User.Identity.Name);//what is the value of Identity.Name? (is properly set in token generation - ClaimType.Name?)
                if (user == null)
                {
                    return;
                }
                foreach (var conversation in user.Conversations)
                {
                    Groups.AddToGroupAsync(Context.ConnectionId, conversation.Name);
                }

            }
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            User user;
            int connectionId = int.Parse(Context.ConnectionId);
            using(var db = new QuarkDbContext())
            {
                var connection = await db.Connections.Include(c => c.User).FirstAsync(c => c.Id == connectionId);
                user =  connection.User;
            }
            foreach (var conversation in user.Conversations)//because connectionId is different everytime user connects to application
            {
                Groups.RemoveFromGroupAsync(Context.ConnectionId, conversation.Name);
            }
            await base.OnDisconnectedAsync(exception);
        }
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
