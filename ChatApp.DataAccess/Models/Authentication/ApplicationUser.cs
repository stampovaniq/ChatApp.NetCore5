using System;
using System.Collections.Generic;
using System.Text;
using ChatApp.DataAccess.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.DataAccess.Models.Authentication
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Message> MessageCreatedBy { get; set; }

        public virtual ICollection<MessageReceivers> MessageReceiversReceiverId { get; set; }
    }
}
