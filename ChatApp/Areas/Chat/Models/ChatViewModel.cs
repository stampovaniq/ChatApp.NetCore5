using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Areas.Chat.Data;
using ChatApp.Business.Common.Enums;
using ChatApp.Business.Common.Interface;
using ChatApp.Business.Core.Authentication;

namespace ChatApp.Areas.Chat.Models
{
    public class ChatViewModel : INotification
    {
        public string UserName { get; set; }

        public string ChatMessage { get; set; }

        public DateTime TimeSent { get; set; }

        public List<ApplicationUser> Users { get; set; }
        
        public List<MessageItem> Messages { get; set; }

        public EnumMessageType MessageType { get; set; }

        public string NotificationMessage { get; set; }
    }
}
