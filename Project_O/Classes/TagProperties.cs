using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

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
        /////
        ///OnHoveredBGBrush
        /////
        public static readonly DependencyProperty OnHoverBGBrushProperty =
                DependencyProperty.RegisterAttached(
                    "OnHoverBGBrush",
                    typeof(Brush),
                    typeof(TagProperties),
                    new PropertyMetadata(Brushes.LightGray));

        public static Brush GetOnHoverBGBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(OnHoverBGBrushProperty);
        }
        public static void SetOnHoverBGBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(OnHoverBGBrushProperty, value);
        }
    }
}
