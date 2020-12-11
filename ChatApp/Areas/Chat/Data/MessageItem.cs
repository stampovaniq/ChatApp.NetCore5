using System;

namespace ChatApp.Areas.Chat.Data
{
    public class MessageItem
    {
        public string MessageBody { get; set; }
        public int StatusTypeId { get; set; }
        public Guid CreatedBy { get; set; }
    }
}