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

        public WorkoutDetailsWindowViewModel(Workout workout)
        {
            Workout = workout ?? throw new ArgumentNullException(nameof(workout));
        }

        public Workout Workout
        {
            get => _workout;
            set
            {
                _workout = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Date));
                OnPropertyChanged(nameof(Type));
                OnPropertyChanged(nameof(Duration));
                OnPropertyChanged(nameof(Repetitions));
                OnPropertyChanged(nameof(Distance));
                OnPropertyChanged(nameof(CaloriesBurned));
                OnPropertyChanged(nameof(Notes));
                OnPropertyChanged(nameof(RepetitionsVisibility));
                OnPropertyChanged(nameof(DistanceVisibility));
            }
        }

        public string Date => _workout.Date;
        public string Type => _workout.Type;
        public int Duration => _workout.Duration;
        public int CaloriesBurned => _workout.CaloriesBurned;
        public string Notes => _workout.Notes;

        public int Repetitions => _workout is StrengthWorkout strength ? strength.Repetitions : 0;
        public double Distance => _workout is CardioWorkout cardio ? cardio.Distance : 0;

        //kontrollera synslighet
        public Visibility RepetitionsVisibility => _workout is StrengthWorkout ? Visibility.Visible : Visibility.Collapsed;
        public Visibility DistanceVisibility => _workout is CardioWorkout ? Visibility.Visible : Visibility.Collapsed;

        public RelayCommand EditCommand { get; }
        public RelayCommand SaveCommand { get; }

        public WorkoutDetailsWindowViewModel()
        {
            EditCommand = new RelayCommand(ExecuteEdit);
            SaveCommand = new RelayCommand(ExecuteSave);
        }

        private void ExecuteEdit(object param)
        {

        }
        private void ExecuteSave(object param)
        {

        }
    }
}