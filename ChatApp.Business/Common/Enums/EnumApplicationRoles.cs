using System.ComponentModel;

namespace ChatApp.Business.Common.Enums
{
    public enum EnumApplicationRoles : byte
    {
        [Description("Administrator")]
        Administrator,

        [Description("User")]
        User
    }
}