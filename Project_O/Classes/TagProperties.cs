using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project_O.Classes
{
    public static class TagProperties
    {
        public static readonly DependencyProperty CustomTagProperty =
                DependencyProperty.RegisterAttached(
                    "CustomTag",
                    typeof(string),
                    typeof(TagProperties),
                    new PropertyMetadata(""));

        public static string GetCustomTag(DependencyObject obj)
        {
            return (string)obj.GetValue(CustomTagProperty);
        }

        public static void SetCustomTag(DependencyObject obj, string value)
        {
            obj.SetValue(CustomTagProperty, value);
        }
    }
}
