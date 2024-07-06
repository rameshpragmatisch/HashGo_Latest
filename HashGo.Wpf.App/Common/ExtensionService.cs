using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace HashGo.Wpf.App.Common
{
    public class ExtensionService
    {
        public static T GetVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            var child = default(T);
            if (parent == null) return null;
            var numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < numVisuals; i++)
            {
                var v = VisualTreeHelper.GetChild(parent, i);
                child = v as T ?? GetVisualChild<T>(v);
                if (child != null) break;
            }

            return child;
        }
    }
}
