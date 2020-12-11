using System;
using System.Collections.Generic;
using System.Text;
using ChatApp.Business.Common.Enums;

namespace ChatApp.Business.Common.Interface
{
    public interface INotification
    {
        EnumMessageType MessageType { get; set; }

        string NotificationMessage { get; set; }
    }
}
