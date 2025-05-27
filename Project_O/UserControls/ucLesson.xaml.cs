using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskManagerLogic.Classes;

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
        public SubjectTask Task;
        private void LessonButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null && mainWindow.ucNInformation.Visibility == Visibility.Collapsed) 
                mainWindow.ucNInformation.Visibility = Visibility.Visible;

            var lessonModel = DataContext as LessonModel;
            
            mainWindow.ucNInformation.DataContext = lessonModel;
        }
        private void LessonRemove_Click(object sender, RoutedEventArgs e) { 
        
        }
    }
}
