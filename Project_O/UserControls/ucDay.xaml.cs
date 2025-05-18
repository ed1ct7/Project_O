using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Project_O;
using Project_O.Classes;

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

        private void UcDay_Loaded(object sender, RoutedEventArgs e)
        {
            var dayModel = DataContext as DayModel;
            this.date = dayModel.Date;
            
            if (dayModel != null)
            {
                if (date == DateTime.Today)
                {
                    Border.BorderBrush = Brushes.Blue;
                }
                GenerateLessons();
            }
        }

        public void GenerateLessons()
        {
            var dayModel = DataContext as DayModel;
            dayModel.Lessons.Clear(); // Clear existing lessons

            foreach (var lesson in GroupSettings.Lessons[(int)dayModel.Date.DayOfWeek])
            {
                var lessonModel = new LessonModel
                {
                    name = lesson
                };
                dayModel.Lessons.Add(lessonModel);
            }
        }

        private void AddLessonButton_Click(object sender, RoutedEventArgs e)
        {
            var dayModel = DataContext as DayModel;
            int dayIndex = (int)dayModel.Date.DayOfWeek;

            var comboBox = new ComboBox
            {
                IsEditable = true,
                IsTextSearchEnabled = true
            };

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
                    GroupSettings.Lessons[dayIndex] = GroupSettings.Lessons[dayIndex].Concat(new[] { newLesson }).ToArray();
                    dayModel.Lessons.Add(new LessonModel { name = newLesson });

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
