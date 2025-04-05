using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project_O.Classes
{
    public class WeekViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<DayModel> _numeratorDays;
        private ObservableCollection<DayModel> _denominatorDays;
        private DateTime _currentDate;

        public ObservableCollection<DayModel> NumeratorDays
        {
            get => _numeratorDays;
            set { _numeratorDays = value; OnPropertyChanged(); }
        }

        public ObservableCollection<DayModel> DenominatorDays
        {
            get => _denominatorDays;
            set { _denominatorDays = value; OnPropertyChanged(); }
        }

        public DateTime CurrentDate
        {
            get => _currentDate;
            set
            {
                _currentDate = value;
                OnPropertyChanged();
                GenerateWeeks();
            }
        }

        public WeekViewModel()
        {
            NumeratorDays = new ObservableCollection<DayModel>();
            DenominatorDays = new ObservableCollection<DayModel>();
            CurrentDate = DateTime.Today;
            GenerateWeeks();
        }

        private void GenerateWeeks()
        {
            NumeratorDays.Clear();
            DenominatorDays.Clear();

            // Get Monday of current week
            DateTime monday = CurrentDate.AddDays(-(int)CurrentDate.DayOfWeek + (int)DayOfWeek.Monday);
            if (monday > CurrentDate) monday = monday.AddDays(-7);

            // Generate numerator week (odd week)
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

            // Generate denominator week (even week)
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
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private ICommand _previousWeekCommand;
        private ICommand _nextWeekCommand;

        public ICommand PreviousWeekCommand => _previousWeekCommand ?? (_previousWeekCommand = new RelayCommand(
    _ => { CurrentDate = CurrentDate.AddDays(-7); }));

        public ICommand NextWeekCommand => _nextWeekCommand ?? (_nextWeekCommand = new RelayCommand(
            _ => { CurrentDate = CurrentDate.AddDays(7); }));
    }
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
