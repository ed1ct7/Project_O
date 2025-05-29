using Project_O.UserControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TaskManagerLogic.Classes;

namespace Project_O.Windows
{
    /// <summary>
    /// Interaction logic for Start.xaml
    /// </summary>
    public partial class Start : Window
    {
        public User user;
        public Start()
        {
            InitializeComponent();
            Loaded += Start_Loaded;
        }
        private void Start_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = user;
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
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
                if (UserNameLoginTextBox.Text == "")
                {
                    UserNameLoginTextBox.Tag = "Имя не может быть пустым";
                }
                else { 
                    this.IsEnabled = false;
                    user = await User.Login(UserNameLoginTextBox.Text, PasswordLoginTextBox._realText);
                    LogIn.Visibility = Visibility.Collapsed;
                    GroupEntry.Visibility = Visibility.Visible;
                    DataContext = user;
                    this.IsEnabled = true;
                }
            }
            catch (UserException ex) when (ex.ErrorCode == 1)
            {
                MessageBox.Show($"Пользователь с ником {UserNameLoginTextBox.Text} не существует");
                this.IsEnabled = true;
            }
            catch (UserException ex) when (ex.ErrorCode == 2)
            {
                MessageBox.Show($"Введён неверный пароль для пользователя {UserNameLoginTextBox.Text}");
                this.IsEnabled = true;
            }
        }
        private async void Button_Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UserNameRegisterTextBox.Text == "")
                {
                    UserNameRegisterTextBox.Tag = "Имя не может быть пустым";
                }
                else
                {
                    this.IsEnabled = false;
                    user = await User.Register(UserNameRegisterTextBox.Text, PasswordRegisterTextBox._realText);
                    Register.Visibility = Visibility.Collapsed;
                    GroupEntry.Visibility = Visibility.Visible;
                    DataContext = user;
                    this.IsEnabled = true;
                }
                
            }
            catch (UserException ex) when (ex.ErrorCode == 3)
            {
                MessageBox.Show($"Пользователь с ником {UserNameRegisterTextBox.Text} уже существует");
                this.IsEnabled = true;
            }
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                // Check if character is Latin, Cyrillic, or number
                bool isAllowed = (c >= 'A' && c <= 'Z') ||  // Latin uppercase
                                 (c >= 'a' && c <= 'z') ||  // Latin lowercase
                                 (c >= 'А' && c <= 'Я') ||  // Cyrillic uppercase
                                 (c >= 'а' && c <= 'я') ||  // Cyrillic lowercase
                                 (c == 'Ё' || c == 'ё') ||  // Cyrillic Yo/yo
                                 (c >= '0' && c <= '9');    // Numbers

                if (!isAllowed)
                {
                    e.Handled = true;  // Block the character
                    return;
                }
            }
        }
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Allow navigation and control keys
            if (e.Key == Key.Back || e.Key == Key.Delete ||
                e.Key == Key.Left || e.Key == Key.Right ||
                e.Key == Key.Home || e.Key == Key.End)
            {
                return;
            }

            // Block space key explicitly
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
        private void SwitchWinButton_Click(object sender, RoutedEventArgs e)
        {
            if (GroupCreation.Visibility == Visibility.Visible)
            {
                GroupCreation.Visibility = Visibility.Collapsed;
                GroupEntry.Visibility = Visibility.Visible;
            }
            else
            {
                GroupCreation.Visibility = Visibility.Visible;
                GroupEntry.Visibility = Visibility.Collapsed;
            }
        }
        private void SwitchAuthButton_Click(object sender, RoutedEventArgs e)
        {
            GroupCreation.Visibility = Visibility.Collapsed;
            GroupEntry.Visibility = Visibility.Collapsed;
            LogIn.Visibility = Visibility.Visible;
            user = null;
        }

        private async void Button_ConnectToGroup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                if (await Group.CheckUserInGroup(GroupNameEntryTextBox.Text, user.UserName)) throw new GroupException("Пользователь уже в группе", 3);
                await user.ConnectToGroup(GroupNameEntryTextBox.Text, GroupPasswordEntryTextBox.Text);
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
                this.IsEnabled = true;

                
            }
            catch (GroupException ex) when (ex.ErrorCode == 3)
            {
                MessageBox.Show($"Пользователь \"{user.UserName}\" уже подключён к группе \"{GroupNameEntryTextBox.Text}\"");
                this.IsEnabled = true;
            }
            catch (GroupException ex) when (ex.ErrorCode == 5)
            {
                MessageBox.Show($"Группа с названием \"{GroupNameEntryTextBox.Text}\" не существует");
                this.IsEnabled = true;
            }
            catch (GroupException ex) when (ex.ErrorCode == 6)
            {
                MessageBox.Show($"Неверный пароль для группы \"{GroupNameEntryTextBox.Text}\" ");
                this.IsEnabled = true;
            }
        }

        private async void Button_CreateGroup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                await Group.CreateGroup(GroupNameCreateGroupTextBox.Text, GroupPasswordCreateGroupTextBox.Text, user.UserName, VerKeyCreateGroupTextBox.Text);
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
                this.IsEnabled = true;


            }
            catch (GroupException ex) when (ex.ErrorCode == 1)
            {
                MessageBox.Show("Введён несуществующий ключ");
                this.IsEnabled = true;
            }
            catch (GroupException ex) when (ex.ErrorCode == 2)
            {
                MessageBox.Show($"Группа с названием \"{GroupNameCreateGroupTextBox.Text}\" уже существует");
                this.IsEnabled = true;
            }
        }
    }
}
