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
         public ucDay()
            {
                InitializeComponent();
                this.Loaded += UcDay_Loaded;  // Subscribe to the Loaded event
            }

        private void UcDay_Loaded(object sender, RoutedEventArgs e)
        {
            // Now DataContext should be available
            var dayModel = DataContext as DayModel;
            if (dayModel != null)
            {
                DateTime date = dayModel.Date;
                if (date == DateTime.Today)
                {
                    Border.BorderBrush = Brushes.Blue;
                }
            
                // Call AddLessonButton here if it depends on DataContext
                AddLessonButton(dayModel);
            }
        }

        private void AddLessonButton(DayModel dayModel)
        {
            Button btt = new Button();
            var lessons = dayModel.Lessons;
            if (lessons.Count < 8 && GroupSettings.isMaster == true)
            {
                //ItemPanel.Items.Add(btt);
            }
        }
    }
}
