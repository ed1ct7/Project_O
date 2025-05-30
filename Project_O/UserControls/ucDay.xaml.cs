using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Project_O;
using Project_O.Classes;
using TaskManagerLogic.Classes;
using System.Windows.Diagnostics;
namespace Project_O.UserControls
{
    public partial class ucDay : UserControl
    {
        public ObservableCollection<LessonModel> lessons { get; set; }
        public DateTime date {  get; set; }
        public ucDay()
        {
            InitializeComponent();
            this.Loaded += UcDay_Loaded;
        }
        
        private void scheduleShift_Check(object sender, RoutedEventArgs e)

        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            var dayModel = DataContext as DayModel;
            if (scheduleShift.IsChecked == true)
            {
                BorderU.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#01f8bd"));
                BorderB.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#01f8bd"));
                dayModel.scheduleShift = mainWindow.user.Groups.Keys.ToArray()[0].Timetable[(int)dayModel.Date.DayOfWeek + 7 * dayModel.DenNum];
                mainWindow.user.Groups.Keys.ToArray()[0].addScheduleShiftAtDate(dayModel.Date.Date, dayModel.scheduleShift);
            }
            else
            {
                dayModel.scheduleShift = null;
                this.date = dayModel.Date;
                if (dayModel != null)
                {
                    if (date == DateTime.Today)
                    {
                        BorderU.Fill = Classes.Properties.Instance.ProperBlue;
                        BorderB.Fill = Classes.Properties.Instance.ProperBlue;
                        //BorderB.Height = 2;
                        //BorderU.Height = 2;
                    }
                    else if (date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        BorderU.Fill = Classes.Properties.Instance.ProperRed;
                        BorderB.Fill = Classes.Properties.Instance.ProperRed;
                    }
                    else
                    {
                        BorderU.Fill = Classes.Properties.Instance.BorderBrushS;
                        BorderB.Fill = Classes.Properties.Instance.BorderBrushS;
                    }
                }
            }
            GenerateLessons();
            
        }

        private void UcDay_Loaded(object sender, RoutedEventArgs e)
        {
            var dayModel = DataContext as DayModel;
            this.date = dayModel.Date;
            
            if (dayModel != null)
            {
                
                if (date == DateTime.Today)
                {
                    BorderU.Fill = Classes.Properties.Instance.ProperBlue;
                    BorderB.Fill = Classes.Properties.Instance.ProperBlue;
                   //BorderB.Height = 2;
                   //BorderU.Height = 2;
                }
                if (date.DayOfWeek == DayOfWeek.Sunday) {
                    BorderU.Fill = Classes.Properties.Instance.ProperRed;
                    BorderB.Fill = Classes.Properties.Instance.ProperRed;
                }
                if (dayModel.scheduleShift != null)
                {
                    scheduleShift.IsChecked = true;
                }

                GenerateLessons();
            }
        }

        public void GenerateLessons()
        {
            var dayModel = DataContext as DayModel;
            int dayIndex = (int)dayModel.Date.DayOfWeek + 7 * dayModel.DenNum;
            dayModel.Lessons.Clear();
            var mainWindow = Window.GetWindow(this) as MainWindow;
            string[] thisdaytimetable = mainWindow.user.Groups.Keys.ToArray()[0].Timetable[dayIndex];
            if (dayModel.scheduleShift != null) thisdaytimetable = dayModel.scheduleShift;
            foreach (var lesson in thisdaytimetable)
            {
            
                var lessonModel = new LessonModel
                {
                    Name = lesson,
                    CurrentTask = mainWindow.user.Groups.Keys.ToArray()[0].GetTaskCreatedAtDate(lesson, date),
                    Day = dayModel
                };
                dayModel.Lessons.Add(lessonModel);
            }
        }

        private void AddLessonButton_Click(object sender, RoutedEventArgs e)
        {
            var dayModel = DataContext as DayModel;
            int dayIndex = (int)dayModel.Date.DayOfWeek + 7 * dayModel.DenNum;
            var mainWindow = Window.GetWindow(this) as MainWindow;
            var comboBox = new ComboBox
            {
                IsEditable = true,
                IsTextSearchEnabled = true,
                Style = (Style)Application.Current.FindResource("S_AddLesson")
            };
            comboBox.ItemsSource = mainWindow.user.Groups.Keys.ToArray()[0].UniqueLessons;

            // Удаляем кнопку добавления и добавляем ComboBox
            if (StackPanelLessons.Children.Contains(AddLessonButton))
                StackPanelLessons.Children.Remove(AddLessonButton);

            if (!StackPanelLessons.Children.Contains(comboBox))
                StackPanelLessons.Children.Add(comboBox);


            // Обработчик события выбора элемента или ввода текста
            void AddNewLesson()
            {
                string newLesson = comboBox.SelectedItem != null
                    ? comboBox.SelectedItem.ToString()
                    : comboBox.Text;

                if (!string.IsNullOrWhiteSpace(newLesson))
                {
                    var mainWindow = Window.GetWindow(this) as MainWindow;
                    if (dayModel.scheduleShift != null)
                    {
                        mainWindow.user.Groups.Keys.ToArray()[0].Timetable[dayIndex] = mainWindow.user.Groups.Keys.ToArray()[0].Timetable[dayIndex].Concat(new[] { newLesson }).ToArray();
                        dayModel.Lessons.Add(new LessonModel { Name = newLesson });
                    }
                    else
                    {
                        var temp = dayModel.scheduleShift.ToList();
                        temp.Add(newLesson);
                        dayModel.scheduleShift = temp.ToArray();
                        dayModel.Lessons.Add(new LessonModel { Name = newLesson });
                        mainWindow.user.Groups.Keys.ToArray()[0].scheduleShifts[dayModel.Date.Date] = dayModel.scheduleShift;
                    }


                    if (StackPanelLessons.Children.Contains(comboBox))
                        StackPanelLessons.Children.Remove(comboBox);

                    if (!StackPanelLessons.Children.Contains(AddLessonButton))
                        StackPanelLessons.Children.Add(AddLessonButton);
                }
            }
            comboBox.KeyDown += (s, args) =>
            {
                if (args.Key == Key.Enter)
                {
                    AddNewLesson();
                }
            };

            //comboBox.LostFocus += (s, args) =>
            //{
            //    AddNewLesson();
            //};

            comboBox.SelectionChanged += (s, args) =>
            {
                if (comboBox.SelectedItem != null)
                {
                    AddNewLesson();
                }
            };
        }
    }
}
