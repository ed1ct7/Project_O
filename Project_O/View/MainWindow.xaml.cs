using System.Windows;
using System.Windows.Input;
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
            DataContext = this;
            GenerateWeeks();
        }

        private void GenerateWeeks()
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
        public short DenNum {  get; set; } //0 - Numerator; 1 - Denumerator
        public string DayOfWeek { get; set; }
        public int DayNumber { get; set; }
        public DateTime Date { get; set; }
        public ObservableCollection<LessonModel> Lessons { get; set; }
    }
    public class LessonModel
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}