using System.ComponentModel;

namespace ChatApp.Business.Common.Enums
{
    public enum EnumMessageStatusType : byte
    {
        [Description("Sent")]
        Sent = 1,
        [Description("Received")]
        Received
    }
}
