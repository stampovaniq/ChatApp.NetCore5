using System.ComponentModel;

namespace ChatApp.Business.Common.Enums
{
    public enum EnumApplicationPolicy : byte
    {
        [Description("AdministratorRole")]
        AdministratorRoleGroup,

        [Description("UserRole")]
        UserRoleGroup
    }
}