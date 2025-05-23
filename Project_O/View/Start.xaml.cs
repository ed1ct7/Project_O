using System.Windows;
using System.Windows.Input;

namespace Project_O.Windows
{
    /// <summary>
    /// Interaction logic for Start.xaml
    /// </summary>
    public partial class Start : Window
    {
        public Start()
        {
            InitializeComponent();
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
        // Меню //
        private void btnMinimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void btnMaximize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState ==
            WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        private void btnClose_Click(object sender, RoutedEventArgs e) => Close();

        private void RegAuthButton_Click(object sender, RoutedEventArgs e)
        {
            if(LogIn.Visibility == Visibility.Visible)
            {
                LogIn.Visibility = Visibility.Collapsed;
                Register.Visibility = Visibility.Visible;
            }
            else
            {
                LogIn.Visibility = Visibility.Visible;
                Register.Visibility = Visibility.Collapsed;
            }
        }
    }
}
