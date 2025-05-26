using System.Windows;
using System.Windows.Input;
using TaskManagerLogic.Classes;

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
            if (LogIn.Visibility == Visibility.Visible)
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

        // UserException с кодом ошибки 1 - такого пользователя нет
        // UserException с кодом ошибки 2 - неверный пароль 
        // UserException с кодом ошибки 3 - пользователь с таким ником уже существует
        // Для обработки catch (UserException ex) when (ex.ErrorCode == N)
        private async void Button_Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await User.Login(UserNameLoginTextBox.Text, PasswordLoginTextBox.maskedTextBox.Text);
            }
            catch (UserException ex) when (ex.ErrorCode == 1)
            {

            }
            catch (UserException ex) when (ex.ErrorCode == 2)
            {

            }
        }

        private async void Button_Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await User.Register(UserNameRegisterTextBox.Text, PasswordRegisterTextBox.maskedTextBox.Text);
            }
            catch (UserException ex) when (ex.ErrorCode == 3)
            {

            }
        }
    }
}
