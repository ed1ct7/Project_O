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
        User user;
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
            //    LogIn.Visibility = Visibility.Collapsed;
            //    Register.Visibility = Visibility.Visible;
            }
            else
            {
                //LogIn.Visibility = Visibility.Visible;
                //Register.Visibility = Visibility.Collapsed;
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
                user = await User.Login(UserNameLoginTextBox.Text, PasswordLoginTextBox._realText);
            }
            catch (UserException ex) when (ex.ErrorCode == 1)
            {
                MessageBox.Show($"Пользователь с ником {UserNameLoginTextBox.Text} не существует");
            }
            catch (UserException ex) when (ex.ErrorCode == 2)
            {
                MessageBox.Show($"Введён неверный пароль для пользователя {UserNameLoginTextBox.Text}");
            }
        }

        private async void Button_Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                user = await User.Register(UserNameRegisterTextBox.Text, PasswordRegisterTextBox._realText);
            }
            catch (UserException ex) when (ex.ErrorCode == 3)
            {
                MessageBox.Show($"Пользователь с ником {UserNameRegisterTextBox.Text} уже существует");
            }
        }
    }
}
