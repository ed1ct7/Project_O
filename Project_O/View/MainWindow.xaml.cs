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

namespace Project_O
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<DayModel> NumeratorDays { get; } = new ObservableCollection<DayModel>();
        public ObservableCollection<DayModel> DenominatorDays { get; } = new ObservableCollection<DayModel>();
        public DateTime CurrentDate { get; private set; } = DateTime.Today;

        public MainWindow()
        {
            InitializeComponent();
            GenerateWeeks();
        }

        private void GenerateWeeks()
        {
            NumeratorDays.Clear();
            DenominatorDays.Clear();

            // Morning of the next week
            DateTime monday = CurrentDate.AddDays(-(int)CurrentDate.DayOfWeek + (int)DayOfWeek.Monday);
            if (monday > CurrentDate) monday = monday.AddDays(-7);

            // First week
            for (int i = 0; i < 7; i++)
            {
                DateTime day = monday.AddDays(i);
                NumeratorDays.Add(new DayModel
                {
                    DayOfWeek = day.ToString("dddd"),
                    DayNumber = day.Day,
                    Lessons = new ObservableCollection<ucLesson>(),
                    Date = day
                });
            }

            // Second week
            for (int i = 0; i < 7; i++)
            {
                DateTime day = monday.AddDays(i + 7);
                DenominatorDays.Add(new DayModel
                {
                    DayOfWeek = day.ToString("dddd"),
                    DayNumber = day.Day,
                    Lessons = new ObservableCollection<ucLesson>(),
                    Date = day
                });
            }

            // Date update
            MonthYearText.Text = CurrentDate.ToString("MMMM yyyy");
        }

        private void PreviousWeek_Click(object sender, RoutedEventArgs e)
        {
            CurrentDate = CurrentDate.AddDays(-14);
            GenerateWeeks();
        }

        private void NextWeek_Click(object sender, RoutedEventArgs e)
        {
            CurrentDate = CurrentDate.AddDays(14);
            GenerateWeeks();
        }

        // Остальные методы управления окном
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
    }

    public class DayModel
    {
        public string DayOfWeek { get; set; }
        public int DayNumber { get; set; }
        public ObservableCollection<ucLesson> Lessons { get; set; }
        public DateTime Date { get; set; }
    }
}