using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatApp.Business.Common.NavigationPages
{
    public class NavigationPage
    {
        private string _titlePage;

        private string GetControllerName<T>() where T : class
        {
            return typeof(T).Name.Substring(0, typeof(T).Name.IndexOf(typeof(T).BaseType.Name));
        }

        private string GetAreaName<T>() where T : class
        {
            string areaName = string.Empty;
            dynamic attribute = typeof(T).GetCustomAttributesData().SingleOrDefault(item => item.AttributeType.Name == "AreaAttribute");
            if (attribute != null)
            {
                dynamic arguments = attribute.ConstructorArguments;
                dynamic argument = arguments[0];
                areaName = argument.Value;
            }

            return areaName;
        }

        public string Controller<T>() where T : class
        {
            return this.GetControllerName<T>();
        }

        public string Area<T>() where T : class
        {
            return this.GetAreaName<T>();
        }

        public string Title
        {
            get { return _titlePage; }
            set { this._titlePage = value; }
        }
    }
}
