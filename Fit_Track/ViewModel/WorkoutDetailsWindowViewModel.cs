using Fit_Track.Model;
using Fit_Track.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fit_Track.ViewModel
{
    public class WorkoutDetailsWindowViewModel : ViewModelBase
    {
        //boolean som bestämmer om det går att redigera
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

        //EGENSKAPER
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
        public string Date
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
        public int Duration
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
            get => _workout is StrengthWorkout strength ? strength.Repetitions : 0;
            set
            {
                if (_workout is StrengthWorkout strengthWorkout)
                {
                    strengthWorkout.Repetitions = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Distance
        {
            get => _workout is CardioWorkout cardio ? cardio.Distance : 0;
            set
            {
                if (_workout is CardioWorkout cardioWorkout)
                {
                    cardioWorkout.Distance = value;
                    OnPropertyChanged();
                }
            }
        }

        //styr synligheten av repetitionsfältet
        public Visibility RepetitionsVisibility => _workout is StrengthWorkout ? Visibility.Visible : Visibility.Collapsed;

        //styr synligheten av distansfältet
        public Visibility DistanceVisibility => _workout is CardioWorkout ? Visibility.Visible : Visibility.Collapsed;

        //KONSTRUKTOR
        public WorkoutDetailsWindowViewModel(Workout workout)
        {
            Workout = workout;

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
            IsEditable = false;

            //uppdatera workoutList i WorkoutsWindowViewModel
            if (param is Window window && window.DataContext is WorkoutsWindowViewModel mainViewModel)
            {
                mainViewModel.UpdateWorkoutList(_workout);
            }
            WorkoutsWindow workoutsWindow = new WorkoutsWindow();
            var workoutdetailsWindow = param as Window;
            workoutsWindow.Show();
            Application.Current.Windows[0].Close();
        }
    }
}