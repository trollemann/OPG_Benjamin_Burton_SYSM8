using Fit_Track.Model;
using Fit_Track.View;
using System.Windows;

namespace Fit_Track.ViewModel
{
    //EGENSKAPER
    public class CopyWorkoutWindowViewModel : ViewModelBase
    {

        private User _currentUser;

        public User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }

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

        private Workout _workout;
        public Workout Workout
        {
            get => _workout;
            set
            {
                _workout = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsEditable));
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
                CalculateCaloriesBurned();
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
            get => _workout is StrengthWorkout strengthWorkout ? strengthWorkout.Repetitions : 0;
            set
            {
                if (_workout is StrengthWorkout strengthWorkout)
                {
                    strengthWorkout.Repetitions = value;
                    OnPropertyChanged();
                    CalculateCaloriesBurned();
                }
            }
        }
        public int Distance
        {
            get => _workout is CardioWorkout cardioWorkout ? cardioWorkout.Distance : 0;
            set
            {
                if (_workout is CardioWorkout cardioWorkout)
                {
                    cardioWorkout.Distance = value;
                    OnPropertyChanged();
                    CalculateCaloriesBurned();
                }
            }
        }

        public Visibility RepetitionsVisibility => _workout is StrengthWorkout ? Visibility.Visible : Visibility.Collapsed;

        public Visibility DistanceVisibility => _workout is CardioWorkout ? Visibility.Visible : Visibility.Collapsed;

        public WorkoutsWindowViewModel _workoutsWindowViewModel;

       
        //KONSTRUKTOR
        public CopyWorkoutWindowViewModel(Workout workout, WorkoutsWindowViewModel workoutsWindowViewModel)
        {
            Workout = workout;
            _workoutsWindowViewModel = workoutsWindowViewModel;

            Edit = new RelayCommand(ExecuteEdit);
            Save = new RelayCommand(ExecuteSave, CanExecuteSave);
            _isEditable = false;
        }

        //KOMMANDON
        public RelayCommand Edit { get; }
        public RelayCommand Save { get; }

        //METODER
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
            MessageBox.Show("Changes have been saved");

            Workout workout;

            if (_workout is StrengthWorkout)
            {
                workout = new StrengthWorkout(Date, Type, Duration, CaloriesBurned, Notes, Repetitions);
            }
            else if (_workout is CardioWorkout)
            {
                workout = new CardioWorkout(Date, Type, Duration, CaloriesBurned, Notes, Distance);
            }
            else
            {
                return;
            }

            _workoutsWindowViewModel.CurrentUser.AddWorkout(workout);

            WorkoutsWindow workoutsWindow = new WorkoutsWindow();
            workoutsWindow.Show();
            Application.Current.Windows[0].Close();
        }

        private void CalculateCaloriesBurned()
        {
            if (_workout is StrengthWorkout strengthWorkout)
            {
                CaloriesBurned = (int)Duration.TotalMinutes * Repetitions;
            }
            else if (_workout is CardioWorkout cardioWorkout)
            {
                CaloriesBurned = (int)Duration.TotalMinutes * Distance;
            }
        }
    }
}