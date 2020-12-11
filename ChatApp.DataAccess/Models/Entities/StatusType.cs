using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.DataAccess.Models.Entities
{
    public class StatusType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusTypeId { get; set; }
        [Required]
        [Column("StatusTypeName", TypeName = "nvarchar(50)")]
        public string StatusTypeName { get; set; }
        [Required]
        [Column("StatusTypeDescription", TypeName = "nvarchar(50)")]
        public string StatusTypeDescription { get; set; }

        public virtual MessageReceivers MessageReceivers { get; set; }

        public virtual Message Message { get; set; }
    }
}
