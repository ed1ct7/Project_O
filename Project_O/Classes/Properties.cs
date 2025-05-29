using System.Windows.Media;
using System.Windows;

namespace Project_O.Classes
{
    public class Properties : DependencyObject
    {
        public static Brush tempBgBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3B1E78"));
        public static Color tempBgColor = (Color)ColorConverter.ConvertFromString("#1C1C1C");
        public static Color tempBgColorOnHov = (Color)ColorConverter.ConvertFromString("#3B1E78");

        // Background color property for Window
        public static Properties Instance { get; } = new Properties();
        //
        // BGP_Default
        //
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
        // BGP_Control
        //
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
        //
        // FGP_Control
        //
        public static readonly DependencyProperty FGP_Control =
            DependencyProperty.Register(
                "FGP_Control",
                typeof(Brush),
                typeof(Properties),
                new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fffbfc")))); //Almost white
        public Brush FG_Control
        {
            get { return (Brush)GetValue(FGP_Control); }
            set { SetValue(FGP_Control, value); }
        }
        //
        // BG_SControl
        //
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
        //
        // FG_SControl
        //
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
        // P_BorderBrushS
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
        ///"#ff9bb4" 
        // P_DefaultBackS
        ///
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
        /// 
        // P_ProperBlue
        ///
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
        ///
        // P_ProperRed
        ///
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
        /// 
        // ControlBorderColor
        ///
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
        /// 
        // FG_ControlOnHovered
        ///
        public static readonly DependencyProperty FGP_ControlOnHovered =
            DependencyProperty.Register(
                "FGP_ControlOnHovered",
                typeof(Color),
                typeof(Properties),
                 new PropertyMetadata((Color)ColorConverter.ConvertFromString("#ff1359")));
        public Color FG_ControlOnHovered
        {
            get { return (Color)GetValue(FGP_ControlOnHovered); }
            set { SetValue(FGP_ControlOnHovered, value); }
        }
        /// 
        // BG_ControlOnHovered
        ///
        public static readonly DependencyProperty BGP_ControlOnHoveredProperty =
            DependencyProperty.Register(
                "BGP_ControlOnHoveredProperty",
                typeof(Color),
                typeof(Properties),
                new PropertyMetadata(tempBgColorOnHov));

        public Color BG_ControlOnHovered
        {
            get => (Color)GetValue(BGP_ControlOnHoveredProperty);
            set => SetValue(BGP_ControlOnHoveredProperty, value);
        }
        /// 
        // FG_ControlOnPressed
        ///
        public static readonly DependencyProperty FGP_ControlOnPressed =
            DependencyProperty.Register(
                "FGP_ControlOnPressed",
                typeof(Color),
                typeof(Properties),
                new PropertyMetadata((Color)ColorConverter.ConvertFromString("#ff1359")));
        public Color FG_ControlOnPressed
        {
            get { return (Color)GetValue(FGP_ControlOnPressed); }
            set { SetValue(FGP_ControlOnPressed, value); }
        }
        /// 
        // BG_ControlOnPressed
        ///
        public static readonly DependencyProperty BGP_ControlOnPressed =
            DependencyProperty.Register(
                "BGP_ControlOnPressed",
                typeof(Color),
                typeof(Properties),
                new PropertyMetadata((Color)ColorConverter.ConvertFromString("#311963")));
        public Color BG_ControlOnPressed
        {
            get { return (Color)GetValue(BGP_ControlOnPressed); }
            set { SetValue(BGP_ControlOnPressed, value); }
        }
        /// 
        // FontSizeOnHovered
        ///
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
        /// 
        // DefaultCornerRadius
        ///
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
