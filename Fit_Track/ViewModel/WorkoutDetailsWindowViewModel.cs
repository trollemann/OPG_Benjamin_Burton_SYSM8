using Fit_Track.Model;
using Fit_Track.View;
using System.Windows;

namespace Fit_Track.ViewModel
{
    public class WorkoutDetailsWindowViewModel : ViewModelBase
    {
        private Workout _workout;
        private StrengthWorkout _strengthWorkout;
        private CardioWorkout _cardioWorkout;

        // Egenskaper
        private bool _isEditable;
        public bool IsEditable
        {
            get => _isEditable;
            set
            {
                _isEditable = value;
                OnPropertyChanged();
            }
        }

        public Workout Workout
        {
            get => _workout;
            set
            {
                _workout = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(RepetitionsVisibility)); // Ensure visibility is updated
                OnPropertyChanged(nameof(DistanceVisibility));
            }
        }

        public DateTime Date
        {
            get => _workout.Date;
            set
            {
                _workout.Date = value;
                OnPropertyChanged();
            }
        }

        public string Type
        {
            get => _workout.Type;
            set
            {
                _workout.Type = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan Duration
        {
            get => _workout.Duration;
            set
            {
                _workout.Duration = value;
                OnPropertyChanged();
            }
        }

        public int CaloriesBurned
        {
            get => _workout.CaloriesBurned;
            set
            {
                _workout.CaloriesBurned = value;
                OnPropertyChanged();
            }
        }

        public string Notes
        {
            get => _workout.Notes;
            set
            {
                _workout.Notes = value;
                OnPropertyChanged();
            }
        }

        public int Repetitions
        {
            get => _strengthWorkout.Repetitions;
            set
            {
                _strengthWorkout.Repetitions = value;
                OnPropertyChanged();
            }
        }

        public int Distance
        {
            get => _cardioWorkout.Distance;
            set
            {
                _cardioWorkout.Distance = value;
                OnPropertyChanged();
            }
        }

        public Visibility RepetitionsVisibility => _workout is StrengthWorkout ? Visibility.Collapsed : Visibility.Visible;
        public Visibility DistanceVisibility => _workout is CardioWorkout ? Visibility.Collapsed : Visibility.Visible;

        //Konstruktor
        public WorkoutDetailsWindowViewModel(Workout workout)
        {
            _isEditable = false;

            Workout = workout;
        
            Date = workout.Date;
            Type = workout.Type;
            Duration = workout.Duration;
            CaloriesBurned = workout.CaloriesBurned;
            Notes = workout.Notes;
            

            if (_workout is StrengthWorkout strengthWorkout)
            {
                strengthWorkout.Repetitions = Repetitions;
            }
            else if (_workout is CardioWorkout cardioWorkout)
            {
                cardioWorkout.Distance = Distance;
            }

            Edit = new RelayCommand(ExecuteEdit);
            Save = new RelayCommand(ExecuteSave, CanExecuteSave);
        }

        // Commands
        public RelayCommand Edit { get; }
        public RelayCommand Save { get; }

        // Methods
        private void ExecuteEdit(object param)
        {
            IsEditable = !IsEditable;
        }

        private bool CanExecuteSave(object param)
        {
            return IsEditable;
        }

        private void ExecuteSave(object param)
        {
            _workout.Date = Date;
            _workout.Type = Type;
            _workout.Duration = Duration;
            _workout.CaloriesBurned = CaloriesBurned;
            _workout.Notes = Notes;

            if (_workout is StrengthWorkout strengthWorkout)
            {
                strengthWorkout.Repetitions = Repetitions;
            }
            else if (_workout is CardioWorkout cardioWorkout)
            {
                cardioWorkout.Distance = Distance;
            }

            MessageBox.Show("Changes have been saved");

            // Close current window and open WorkoutsWindow
            var workoutsWindow = new WorkoutsWindow();
            workoutsWindow.Show();
            Application.Current.Windows[0].Close();
        }

        private void CalculateCaloriesBurned()
        {
            if (_workout is StrengthWorkout strengthWorkout)
            {
                CaloriesBurned = (int)(Duration.TotalMinutes * Repetitions);
            }
            else if (_workout is CardioWorkout cardioWorkout)
            {
                CaloriesBurned = (int)(Duration.TotalMinutes * Distance);
            }
        }
    }
}