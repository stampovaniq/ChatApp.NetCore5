using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChatApp.DataAccess.Models.Authentication;

namespace ChatApp.DataAccess.Models.Entities
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }
        
        [Column("MessageBody", TypeName = "nvarchar(1024)")]
        public string MessageBody { get; set; }
        [Required]
        [Column("StatusTypeId",TypeName = "int")]
        public int StatusTypeId { get; set; }
        [Required]
        public Guid CreatedBy { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }

        public virtual StatusType StatusType { get; set; }

        public virtual ApplicationUser CreatedByUser { get; set; }

        public ICollection<MessageReceivers> MessageReceiversMessageId { get; set; }

    }
}
