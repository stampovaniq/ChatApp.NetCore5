using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatApp.Areas.Chat.Data;
using ChatApp.Areas.Chat.Models;
using ChatApp.Business.Common.Enums;
using ChatApp.Business.Core.Authentication;
using ChatApp.Business.Core.Services;
using ChatApp.Business.Extension.Claims;
using ChatApp.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Areas.Chat.Controllers
{
    [Authorize]
    [Area("Chat")]
    [Route("[area]/[controller]/{auction=index}")]
    public class ChatController : Controller
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ApplicationUser _applicationUser;
        private readonly IMessageService _messageService;

        public ChatController(IHubContext<ChatHub> hubContext, ApplicationUser applicationUser, IMessageService messageService)
        {
            _hubContext = hubContext;
            _applicationUser = applicationUser;
            _messageService = messageService;
        }

       public IActionResult Index()
       {
           var model = new ChatViewModel() {UserName = User.GetLoggedInUserName()};
            return View(model);
       }

       [HttpPost]
       public async Task<IActionResult> Index(ChatViewModel model)
       {
           if (!ModelState.IsValid)
           {
               return PartialView("_ChatBox", model);
           }

           var currentUser = await _applicationUser.GetUser(User.Identity.Name);
           
           var message = new MessageItem
           {
               MessageBody = model.ChatMessage,
               StatusTypeId = (int)EnumMessageStatusType.Sent,
               CreatedBy = currentUser.Id
           };

           await _hubContext.Clients.All.SendAsync("BroadcastMessage", User.Identity.Name, model.ChatMessage, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

           await _messageService.AddMessageAsync(message);
           
           ModelState.Clear();
           model.ChatMessage = "";
           model.UserName = User.Identity.Name;
           return PartialView("_ChatBox", model);
       }
    }
}
