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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Project_O.UserControls
{
    /// <summary>
    /// Interaction logic for ucLesson.xaml
    /// </summary>
    public partial class ucInformation : UserControl
    {
        public ucInformation()
        {
            InitializeComponent();

            this.Loaded += UcInformation_Loaded;
            
        }
        private void UcInformation_Loaded(object sender, RoutedEventArgs e)
        {
            var lessonModel = DataContext as LessonModel;
            if (lessonModel != null && lessonModel.CurrentTask != null)
            {
                TaskName.Text = lessonModel.CurrentTask.Name;
                TaskDesc.Text = lessonModel.CurrentTask.Description;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var lessonModel = DataContext as LessonModel;
            lessonModel.Name = "Помогите";
            this.DataContext = lessonModel;
        }
    }
}
