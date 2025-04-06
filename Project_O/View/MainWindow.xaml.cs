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

            // Находим понедельник текущей недели
            DateTime monday = CurrentDate.AddDays(-(int)CurrentDate.DayOfWeek + (int)DayOfWeek.Monday);
            if (monday > CurrentDate) monday = monday.AddDays(-7);

            // Заполняем числитель (текущая неделя)
            for (int i = 0; i < 7; i++)
            {
                DateTime day = monday.AddDays(i);
                NumeratorDays.Add(new DayModel
                {
                    DayOfWeek = day.ToString("dddd"),
                    DayNumber = day.Day,
                    Lessons = new ObservableCollection<string>()
                });
            }

            // Заполняем знаменатель (следующая неделя)
            for (int i = 0; i < 7; i++)
            {
                DateTime day = monday.AddDays(i + 7);
                DenominatorDays.Add(new DayModel
                {
                    DayOfWeek = day.ToString("dddd"),
                    DayNumber = day.Day,
                    Lessons = new ObservableCollection<string>()
                });
            }

            // Обновляем отображение даты
            MonthYearText.Text = CurrentDate.ToString("MMMM yyyy");
        }

        private void PreviousWeek_Click(object sender, RoutedEventArgs e)
        {
            CurrentDate = CurrentDate.AddDays(-7);
            GenerateWeeks();
        }

        private void NextWeek_Click(object sender, RoutedEventArgs e)
        {
            CurrentDate = CurrentDate.AddDays(7);
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
        public ObservableCollection<string> Lessons { get; set; }
    }
}