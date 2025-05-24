using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace Project_O.Classes
{
    public class Properties : DependencyObject
    {
        // Background color property for Window
        public static Properties Instance { get; } = new Properties();

        public static readonly DependencyProperty BGP_Default =
            DependencyProperty.Register(
                "BGP_Default",
                typeof(Brush),
                typeof(Properties),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(28, 28, 28))));
        public Brush BG_Default
        {
            get { return (Brush)GetValue(BGP_Default); }
            set { SetValue(BGP_Default, value); }
        }
        //

        //Control properties
        // Background color property for Control
        public static readonly DependencyProperty BGP_Control =
            DependencyProperty.Register(
                "BGP_Control",
                typeof(Brush),
                typeof(Properties),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(28, 28, 28))));
        public Brush BG_Control
        {
            get { return (Brush)GetValue(BGP_Control); }
            set { SetValue(BGP_Control, value); }
        }
        // Foreground color property for Control
        public static readonly DependencyProperty FGP_Control =
            DependencyProperty.Register(
                "FGP_Control",
                typeof(Brush),
                typeof(Properties),
                new PropertyMetadata(Brushes.White));
        public Brush FG_Control
        {
            get { return (Brush)GetValue(FGP_Control); }
            set { SetValue(FGP_Control, value); }
        }
        //

        public static readonly DependencyProperty P_DefaultCornerRadius =
            DependencyProperty.Register(
                "P_DefaultCornerRadius",
                typeof(int),
                typeof(Properties),
                new PropertyMetadata(9));
        public int DefaultCornerRadius
        {
            get { return (int)GetValue(P_DefaultCornerRadius); }
            set { SetValue(P_DefaultCornerRadius, value); }
        }
        //

        //Example How to Use//
        /*
         * Button
            Background="{Binding ButtonBG, Source={x:Static local:Properties.Instance}}" 
            Foreground="{Binding ButtonFG, Source={x:Static local:Properties.Instance}}" 
            BorderBrush="{Binding ButtonBorderBrush, Source={x:Static local:Properties.Instance}}"
        */
    }
}
