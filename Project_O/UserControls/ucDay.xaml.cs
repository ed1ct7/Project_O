using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
                    BorderU.Fill = Classes.Properties.Instance.ProperBlue; 
                    BorderB.Fill = Classes.Properties.Instance.ProperBlue;
                }
                GenerateLessons();
            }
        }

        public void GenerateLessons()
        {
            var dayModel = DataContext as DayModel;
            int dayIndex = (int)dayModel.Date.DayOfWeek + 7 * dayModel.DenNum;
            dayModel.Lessons.Clear();

            foreach (var lesson in GroupSettings.Lessons[dayIndex])
            {
                var lessonModel = new LessonModel
                {
                    Name = lesson,
                    Date = date
                };
                dayModel.Lessons.Add(lessonModel);
            }
        }

        private void AddLessonButton_Click(object sender, RoutedEventArgs e)
        {
            var dayModel = DataContext as DayModel;
            int dayIndex = (int)dayModel.Date.DayOfWeek + 7 * dayModel.DenNum;

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
                    dayModel.Lessons.Add(new LessonModel { Name = newLesson });

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
