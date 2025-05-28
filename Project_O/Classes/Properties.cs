using System.Windows.Media;
using System.Windows;

namespace Project_O.Classes
{
    public class Properties : DependencyObject
    {
        public static Brush tempBgBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#64229E"));
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
                new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fffbfc"))));
        public Brush FG_Control
        {
            get { return (Brush)GetValue(FGP_Control); }
            set { SetValue(FGP_Control, value); }
        }
        //

        // Background color property for Control
        public static readonly DependencyProperty BGP_SControl =
            DependencyProperty.Register(
                "BGP_SControl",
                typeof(Brush),
                typeof(Properties),
                new PropertyMetadata(tempBgBrush));
        public Brush BG_SControl
        {
            get { return (Brush)GetValue(BGP_SControl); }
            set { SetValue(BGP_SControl, value); }
        }
        // Foreground color property for Control
        public static readonly DependencyProperty FGP_SControl =
            DependencyProperty.Register(
                "FGP_SControl",
                typeof(Brush),
                typeof(Properties),
                new PropertyMetadata(Brushes.White));
        public Brush FG_SControl
        {
            get { return (Brush)GetValue(FGP_SControl); }
            set { SetValue(FGP_SControl, value); }
        }
        //

        //
        public static readonly DependencyProperty P_BorderBrushS =
            DependencyProperty.Register(
                "P_BorderBrushS",
                typeof(Brush),
                typeof(Properties),
                new PropertyMetadata(tempBgBrush));
        public Brush BorderBrushS
        {
            get { return (Brush)GetValue(P_BorderBrushS); }
            set { SetValue(P_BorderBrushS, value); }
        }
        //"#ff9bb4" 

        //
        public static readonly DependencyProperty P_DefaultBackS =
            DependencyProperty.Register(
                "P_DefaultBackS",
                typeof(Brush),
                typeof(Properties),
                new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#242326"))));
        public Brush DefaultBackS
        {
            get { return (Brush)GetValue(P_DefaultBackS); }
            set { SetValue(P_DefaultBackS, value); }
        }
        //



        public static readonly DependencyProperty P_ProperBlue =
            DependencyProperty.Register(
                "ProperBlue",
                typeof(Brush),
                typeof(Properties),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(61, 95, 199))));
        public Brush ProperBlue
        {
            get { return (Brush)GetValue(P_ProperBlue); }
            set { SetValue(P_ProperBlue, value); }
        }

        public static readonly DependencyProperty P_ProperRed =
            DependencyProperty.Register(
                "P_ProperRed",
                typeof(Brush),
                typeof(Properties),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(199, 29, 76))));
        public Brush ProperRed
        {
            get { return (Brush)GetValue(P_ProperRed); }
            set { SetValue(P_ProperRed, value); }
        }

        //
        public static readonly DependencyProperty ControlBorderColor =
            DependencyProperty.Register(
                "ControlBorderColorOnHovered",
                typeof(Brush),
                typeof(Properties),
                new PropertyMetadata(tempBgBrush));
        public Brush BorderColor
        {
            get { return (Brush)GetValue(ControlBorderColor); }
            set { SetValue(ControlBorderColor, value); }
        }
        //


        //
        public static readonly DependencyProperty FGP_ControlOnHovered =
            DependencyProperty.Register(
                "FGP_ControlOnHovered",
                typeof(Brush),
                typeof(Properties),
                 new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff1359"))));
        public Brush FG_ControlOnHovered
        {
            get { return (Brush)GetValue(FGP_ControlOnHovered); }
            set { SetValue(FGP_ControlOnHovered, value); }
        }
        //

        // 
        public static readonly DependencyProperty BGP_ControlOnHovered =
            DependencyProperty.Register(
                "BGP_ControlOnHovered",
                typeof(Brush),
                typeof(Properties),
                new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff1359"))));
        public Brush BG_ControlOnHovered
        {
            get { return (Brush)GetValue(BGP_ControlOnHovered); }
            set { SetValue(BGP_ControlOnHovered, value); }
        }
        //


        // 
        public static readonly DependencyProperty FGP_SControlOnHovered =
            DependencyProperty.Register(
                "FGP_SControlOnHovered",
                typeof(Brush),
                typeof(Properties),
                 new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff1359"))));
        public Brush FG_SControlOnHovered
        {
            get { return (Brush)GetValue(FGP_SControlOnHovered); }
            set { SetValue(FGP_SControlOnHovered, value); }
        }
        //

        // 
        public static readonly DependencyProperty BGP_SControlOnHovered =
            DependencyProperty.Register(
                "BGP_SControlOnHovered",
                typeof(Brush),
                typeof(Properties),
                new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff1359"))));
        public Brush BG_SControlOnHovered
        {
            get { return (Brush)GetValue(BGP_SControlOnHovered); }
            set { SetValue(BGP_SControlOnHovered, value); }
        }
        //



        //
        public static readonly DependencyProperty FGP_ControlOnPressed =
            DependencyProperty.Register(
                "FGP_ControlOnPressed",
                typeof(Brush),
                typeof(Properties),
                new PropertyMetadata(Brushes.White));
        public Brush FG_ControlOnPressed
        {
            get { return (Brush)GetValue(FGP_ControlOnPressed); }
            set { SetValue(FGP_ControlOnPressed, value); }
        }
        //

        //
        public static readonly DependencyProperty BGP_ControlOnPressed =
            DependencyProperty.Register(
                "BGP_ControlOnPressed",
                typeof(Brush),
                typeof(Properties),
                new PropertyMetadata(Brushes.White));
        public Brush BG_ControlOnPressed
        {
            get { return (Brush)GetValue(BGP_ControlOnPressed); }
            set { SetValue(BGP_ControlOnPressed, value); }
        }
        //



        public static readonly DependencyProperty P_FontSizeChangedOnHovered =
           DependencyProperty.Register(
               "P_FontSizeChangedOnHovered",
               typeof(int),
               typeof(Properties),
               new PropertyMetadata(15));
        public int FontSizeOnHovered
        {
            get { return (int)GetValue(P_FontSizeChangedOnHovered); }
            set { SetValue(P_FontSizeChangedOnHovered, value); }
        }



        public static readonly DependencyProperty P_DefaultCornerRadius =
            DependencyProperty.Register(
                "P_DefaultCornerRadius",
                typeof(int),
                typeof(Properties),
                new PropertyMetadata(15));
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
