using Quark_Backend.DAL;
using Quark_Backend.Entities;
using Quark_Backend.Utilities;
using Quark_Backend.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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
                    .FirstAsync(u => u.Username == Context.User.Identity.Name);//does token generation (ClaimType.Name) set Identity.Name value properly?
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
        /*
            potential methods:
            - StartPrivateConversation (check if not started). Probably AddToConversation will be enough.

        */


        // public async Task<Conversation> OpenPrivateConversation(string requestingUser, string otherUser)
        // {
        //     using(var db = new QuarkDbContext())
        //     {
        //         db.Conversations.Include(c => c.Messages).Include(c => c.Users)
        //             .Where(c => c.Users.Count == 2)
        //             .Where(c => c.Users.)
        //     }
        // }

        //should initiating new conversation be implemented in seperated method?
        public async Task AddToConversation(string conversationName, string username)
        {
            using(var db = new QuarkDbContext())
            {
                var conversation = await db.Conversations.Include(c => c.Users).FirstAsync(c => c.
                Name == conversationName);
                if(conversation.Users.First(u => u.Username == username) != null)
                {
                    return;//user is already in conversation
                }
                var userToAdd = await db.Users.FirstAsync(u => u.Username == username);
                var usersCount = conversation.Users.Count;
                if(usersCount == 1) //initiating private conversation - is this case possible? how can user have conversation with only him in first place?
                {

                }
                else if(usersCount <= 2) //create new room to prevent adding new member to existing private conversation
                {
                    //generate roomHash/roomName that will be possible to change into custom name
                    string randomName;
                    while(true)
                    {
                        randomName = NameGenerator.GenerateRandomConversationName();
                        if(db.Conversations.First(c => c.Name == randomName) == null)
                        {
                            break;
                        }
                    }
                    List<User> newConversationMembers = new List<User>(conversation.Users);
                    newConversationMembers.Add(userToAdd);
                    var newConversation = new Conversation
                    {
                        Name = randomName,
                        Users = newConversationMembers
                    };
                    db.Conversations.Add(newConversation);
                    await db.SaveChangesAsync();
                    foreach(var user in newConversation.Users)
                    {
                        foreach(var connection in user.Connections)
                        {
                            var connectionId = connection.Id.ToString();
                            await Groups.AddToGroupAsync(connectionId, conversation.Name);
                        }
                    }
                }
                else //it means it's already a group conversation
                {
                    foreach(var connection in userToAdd.Connections)
                    {
                        var connectionId = connection.Id.ToString();
                        await Groups.AddToGroupAsync(connectionId, conversation.Name);
                    }
                    conversation.Users.Add(userToAdd);
                    await db.SaveChangesAsync();
                }
            }
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
