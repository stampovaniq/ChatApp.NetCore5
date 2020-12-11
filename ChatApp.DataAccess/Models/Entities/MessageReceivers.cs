using System;
using System.Collections.Generic;
using ChatApp.DataAccess.Models.Authentication;

namespace ChatApp.DataAccess.Models.Entities
{
    public class MessageReceivers
    {
        public int MessageId { get; set; }

        public Guid ReceiverId { get; set; }

        public int StatusTypeId { get; set; }

        public virtual Message MessageIdUser { get; set; }

        public virtual ApplicationUser ReceiverIdUser { get; set; }

        public virtual StatusType StatusType { get; set; }
    }
}
