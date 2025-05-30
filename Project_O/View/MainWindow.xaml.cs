using Project_O.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Collections.ObjectModel;
using Project_O.UserControls;
using static Project_O.UserControls.ucDay;
using TaskManagerLogic.Classes;
using System.IO;

namespace Project_O
{
    public partial class MainWindow : Window
    {
        public User user;
        
        public ObservableCollection<DayModel> NumeratorDays { get; } = new ObservableCollection<DayModel>();
        public ObservableCollection<DayModel> DenominatorDays { get; } = new ObservableCollection<DayModel>();
        public DateTime CurrentDate { get; private set; } = DateTime.Today;

        public MainWindow(User user)
        {
            
            InitializeComponent();
            this.user = user;
            DataContext = this;
            GenerateWeeks();
            if (user.Groups[user.Groups.Keys.ToArray()[0]]) AccStatus.Fill = new SolidColorBrush(Colors.Green);
            user.Groups.Keys.ToArray()[0].UpdateTimeTable();
            
        }
        public static async Task<MainWindow> CreateMainWindow(User user)
        {
            foreach (var item in user.Groups.Keys) { 
                await item.ActualizeGroupFiles();
            }
            return new MainWindow(user);
        }
        
        public void GenerateWeeks()
        {
            NumeratorDays.Clear();
            DenominatorDays.Clear();

            // Morning of the next week
            DateTime monday = CurrentDate.AddDays(-(int)CurrentDate.DayOfWeek + (int)DayOfWeek.Monday);
            if (monday > CurrentDate) monday = monday.AddDays(-7);

            // First week (Numerator)
            for (int i = 0; i < 7; i++)
            {
                DateTime day = monday.AddDays(i);
                var dayModel = new DayModel
                {
                    DenNum = 0,
                    DayOfWeek = day.ToString("dddd"),
                    DayNumber = day.Day,
                    Date = day,
                    Lessons = new ObservableCollection<LessonModel>()
                };
                dayModel.scheduleShift = user.Groups.Keys.ToArray()[0].GetScheduleShiftAtDate(day);
                NumeratorDays.Add(dayModel);
            }

            // Second week (Denominator)
            for (int i = 0; i < 7; i++)
            {
                DateTime day = monday.AddDays(i + 7);
                DenominatorDays.Add(new DayModel
                {
                    DenNum = 1,
                    DayOfWeek = day.ToString("dddd"),
                    DayNumber = day.Day,
                    Date = day,
                    Lessons = new ObservableCollection<LessonModel>()
                });
                DenominatorDays[i].scheduleShift = user.Groups.Keys.ToArray()[0].GetScheduleShiftAtDate(day);
            }
            // Date update
            MonthYearText.Text = CurrentDate.ToString("MMMM yyyy");
        }

        private async void PreviousWeek_Click(object sender, RoutedEventArgs e)
        {
            CurrentDate = CurrentDate.AddDays(-14);

            
            GenerateWeeks();
        }

        private void NextWeek_Click(object sender, RoutedEventArgs e)
        {
            CurrentDate = CurrentDate.AddDays(14);
            GenerateWeeks();
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await user.Groups.Keys.ToArray()[0].UploadTimeTable();
            user.Groups.Keys.ToArray()[0].UpdateTimeTable();
            await user.Groups.Keys.ToArray()[0].UploadScheduleShifts();
            user.Groups.Keys.ToArray()[0].UpdateScheduleShifts();
        }
    }

    public class DayModel
    {
        public short DenNum {  get; set; } //0 - Numerator; 1 - Denumerator
        public string DayOfWeek { get; set; }
        public int DayNumber { get; set; }
        public DateTime Date { get; set; }
        public string[] scheduleShift { get; set; }
        public ObservableCollection<LessonModel> Lessons { get; set; }
    }
    public class LessonModel
    {
        public string Name { get; set; }
        
        public SubjectTask CurrentTask;
        public DayModel Day { init; get; }
    }
}