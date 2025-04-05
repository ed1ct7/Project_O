using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Project_O.Classes
{
    public class DayModel : INotifyPropertyChanged
    {
        private string _dayOfWeek;
        private int _dayNumber;
        private ObservableCollection<string> _lessons;

        public string DayOfWeek
        {
            get => _dayOfWeek;
            set { _dayOfWeek = value; OnPropertyChanged(); }
        }

        public int DayNumber
        {
            get => _dayNumber;
            set { _dayNumber = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> Lessons
        {
            get => _lessons;
            set { _lessons = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
