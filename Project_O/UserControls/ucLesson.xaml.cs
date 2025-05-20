using System.Windows;
using System.Windows.Controls;

namespace Project_O.UserControls
{
    /// <summary>
    /// Interaction logic for ucLesson.xaml
    /// </summary>
    public partial class ucLesson : UserControl
    {
        public ucLesson()
        {
            InitializeComponent();

        }
        private void LessonButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null && mainWindow.ucNInformation.Visibility == Visibility.Collapsed) 
                mainWindow.ucNInformation.Visibility = Visibility.Visible;

            var lessonModel = DataContext as LessonModel;
            mainWindow.ucNInformation.DataContext = lessonModel;
        }
    }
}
