using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Wpf.App.Helpers
{
    public static class ObjectHelper
    {
        public static object GetThePropertyValue(object instance, string propertyName)
        {
            Type type = instance.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(instance);
            }

            return double.NaN;
        }

        public static bool SetThePropertyValue(object instance, string propertyName, object value)
        {
            Type type = instance.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(instance, value);

                return true;
            }

            return false;
        }
    }
}
