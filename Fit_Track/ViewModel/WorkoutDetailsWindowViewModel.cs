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
        private Workout _workout;
        private bool _isEditable; // New field to track edit mode

        public WorkoutDetailsWindowViewModel(Workout workout)
        {
            Workout = workout ?? throw new ArgumentNullException(nameof(workout));
            EditCommand = new RelayCommand(ExecuteEdit);
            SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
            _isEditable = false; // Initially, textboxes are disabled
        }

        public Workout Workout
        {
            get => _workout;
            set
            {
                _workout = value;
                OnPropertyChanged();
                // No need to notify every property individually here
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

        public Visibility RepetitionsVisibility => _workout is StrengthWorkout ? Visibility.Visible : Visibility.Collapsed;
        public Visibility DistanceVisibility => _workout is CardioWorkout ? Visibility.Visible : Visibility.Collapsed;

        // New property to control textbox editability
        public bool IsEditable
        {
            get => _isEditable;
            set
            {
                _isEditable = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand EditCommand { get; }
        public RelayCommand SaveCommand { get; }

        private void ExecuteEdit(object param)
        {
            // Toggle the IsEditable property
            IsEditable = !IsEditable;
        }

        private bool CanExecuteSave(object param)
        {
            return IsEditable; // Enable save only if in edit mode
        }
        private void ExecuteSave(object param)
        {
            MessageBox.Show("Changes has been saved");
            // Save logic is already handled by the property setters
            IsEditable = false; // Disable editing after saving
        }
    }
}