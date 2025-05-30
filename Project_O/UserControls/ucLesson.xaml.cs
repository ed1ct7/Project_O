using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            Loaded += UcLesson_Loaded;
        }
        private void UcLesson_Loaded(object sender, RoutedEventArgs e)
        {
            var lessonModel = DataContext as LessonModel;
            if (lessonModel.CurrentTask != null)
            {
                Lesson.Background = new LinearGradientBrush
                {
                    //StartPoint = new Point(0.5, 1),  // Top-center
                    //EndPoint = new Point(0.5, 0),    // Bottom-center
                    StartPoint = new Point(1, 1),  // Top-center
                    EndPoint = new Point(0, 0),    // Bottom-center
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop((Color)ColorConverter.ConvertFromString("#242326"), 0.0), //Start //242326
                        new GradientStop((Color)ColorConverter.ConvertFromString("#466d57"), 1.0)
                    }
                };
                //Lesson.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#26C491"));
            }
        }
        public SubjectTask Task;
        private void LessonButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null && mainWindow.ucNInformation.Visibility == Visibility.Collapsed) 
                mainWindow.ucNInformation.Visibility = Visibility.Visible;

            var lessonModel = DataContext as LessonModel;
            
            mainWindow.ucNInformation.DataContext = lessonModel;
            mainWindow.ucNInformation.UpdateTextBoxInfo();
            //Debug.WriteLine((int)lessonModel.Day.Date.DayOfWeek);
        }
        private void LessonRemove_Click(object sender, RoutedEventArgs e) {
            var mainWindow = Window.GetWindow(this) as MainWindow;

            var lessonModel = DataContext as LessonModel;
            if (lessonModel.Day.scheduleShift == null)
            {
                var old = mainWindow.user.Groups.Keys.ToArray()[0].Timetable[(int)lessonModel.Day.Date.DayOfWeek + 7 * lessonModel.Day.DenNum].ToList();
                old.Remove(lessonModel.Name);
                mainWindow.user.Groups.Keys.ToArray()[0].Timetable[(int)lessonModel.Day.Date.DayOfWeek + 7 * lessonModel.Day.DenNum] = old.ToArray();
                
                
            }
            else
            {

            }
            if (mainWindow.ucNInformation.DataContext as LessonModel == lessonModel) mainWindow.ucNInformation.Visibility = Visibility.Collapsed;
            mainWindow.GenerateWeeks();
        }
    }
}
