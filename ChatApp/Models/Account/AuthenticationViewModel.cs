using System.ComponentModel.DataAnnotations;
using ChatApp.Business.Common.Enums;
using ChatApp.Business.Common.Interface;

namespace ChatApp.Models.Account
{
    public class AuthenticationViewModel : INotification
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public EnumMessageType MessageType { get; set; }

        public string NotificationMessage { get; set; }
    }
}
