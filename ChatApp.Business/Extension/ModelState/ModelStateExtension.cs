using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ChatApp.Business.Common.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ChatApp.Business.Extension.ModelState
{
    public static class ModelStateExtension
    {
        private const string _key = "exception";
        public static void ErrorMessage<T>(this ModelStateDictionary modelState, string key, T instance)
        {
            ModelStateEntry entry = modelState.GetValueOrDefault(key);
            if (entry != null)
            {
                string message = entry.Errors[0].Exception.Message;
                Type modelType = typeof(T);
                PropertyInfo propertyInfoMessage = modelType.GetProperty("NotificationMessage");
                propertyInfoMessage.SetValue(instance, message);
                PropertyInfo propertyInfoMessageType = modelType.GetProperty("MessageType");
                propertyInfoMessageType.SetValue(instance, EnumMessageType.Error);
            }
        }
        public static void CustomErrorMessage<T>(this ModelStateDictionary modelState, string message, T instance)
        {
            ModelStateEntry entry = modelState.GetValueOrDefault(_key);
            if (entry != null)
            {
                Type modelType = typeof(T);
                PropertyInfo propertyInfoMessage = modelType.GetProperty("NotificationMessage");
                propertyInfoMessage.SetValue(instance, message);
                PropertyInfo propertyInfoMessageType = modelType.GetProperty("MessageType");
                propertyInfoMessageType.SetValue(instance, EnumMessageType.Error);
            }
        }
        public static void LogException(this ModelStateDictionary modelState) { throw new NotImplementedException(); }
    }
}
