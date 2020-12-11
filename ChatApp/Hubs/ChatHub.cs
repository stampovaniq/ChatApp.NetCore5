using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Business.Common.Enums;
using ChatApp.Business.Core.Authentication;
using ChatApp.Business.Core.Services;
using ChatApp.DataAccess.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        
        public async Task SendMessage(string user, string _message)
        {
            //Send Message
            var timeMsgSent = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            await Clients.All.SendAsync("BroadcastMessage", user, _message, timeMsgSent);
        }

        public override async Task OnConnectedAsync()
        {
            var timeConnected = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"); ;
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId, timeConnected);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var timeDisconnected = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"); ;
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId, timeDisconnected);
            await base.OnDisconnectedAsync(ex);
        }

        public Task JoinGroup(string group)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

        public Task SendMessageToGroup(string groupname, string sender, string message)
        {
            var timeMsgSent = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return Clients.Group(groupname).SendAsync("BroadcastMessage", sender, message, timeMsgSent);
        }


    }
}
