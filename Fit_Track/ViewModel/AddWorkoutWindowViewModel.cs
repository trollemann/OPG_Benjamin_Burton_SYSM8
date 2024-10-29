using Fit_Track.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;


namespace Fit_Track.ViewModel
{
    public class AddWorkoutWindowViewModel : ViewModelBase
    {
        //ObservableCollection för att lagra träningspass
        public ObservableCollection<Workout> Workouts { get; set; } = new ObservableCollection<Workout>();

        //referens till parentfönstrets ViewModel
        private WorkoutsWindowViewModel _workoutsWindowViewModel;

        //EGENSKAPER
        private bool _strengthWorkoutEnabled = true;
        public bool StrengthWorkoutEnabled
        {
            get => _strengthWorkoutEnabled;
            set
            {
                _strengthWorkoutEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _cardioWorkoutEnabled = true;
        public bool CardioWorkoutEnabled
        {
            get => _cardioWorkoutEnabled;
            set
            {
                _cardioWorkoutEnabled = value;
                OnPropertyChanged();
            }
        }

        private Visibility _repetitionsVisibility = Visibility.Visible;
        public Visibility RepetitionsVisibility
        {
            get => _repetitionsVisibility;
            set
            {
                _repetitionsVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _distanceVisibility = Visibility.Collapsed;
        public Visibility DistanceVisibility
        {
            get => _distanceVisibility;
            set
            {
                _distanceVisibility = value;
                OnPropertyChanged();
            }
        }

        private string _date;
        public string Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }

        private string _type;
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        private string _duration;
        public string Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnPropertyChanged();
                UpdateCaloriesBurned();
            }
        }

        private string _caloriesBurned;
        public string CaloriesBurned
        {
            get => _caloriesBurned;
            private set
            {
                _caloriesBurned = value;
                OnPropertyChanged();
            }
        }

        private string _notes;
        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }

        private string _repetitions;
        public string Repetitions
        {
            get => _repetitions;
            set
            {
                _repetitions = value;
                OnPropertyChanged();
                UpdateCaloriesBurned();
            }
        }

        private string _distance;
        public string Distance
        {
            get => _distance;
            set
            {
                _distance = value;
                OnPropertyChanged();
                UpdateCaloriesBurned();
            }
        }

        private bool _strengthWorkout;
        public bool StrengthWorkout
        {
            get => _strengthWorkout;
            set
            {
                _strengthWorkout = value;
                OnPropertyChanged();
                UpdateVisibility();
            }
        }

        private bool _cardioWorkout;
        public bool CardioWorkout
        {
            get => _cardioWorkout;
            set
            {
                _cardioWorkout = value;
                OnPropertyChanged();
                UpdateVisibility();
            }
        }

        //KONSTRUKTOR
        public AddWorkoutWindowViewModel()
        {
            SetWorkout(true);

            StrengthWorkout = true;
            CardioWorkout = false;

            StrengthWorkoutCommand = new RelayCommand(_ => SetWorkout(true));
            CardioWorkoutCommand = new RelayCommand(_ => SetWorkout(false));
            SaveWorkoutCommand = new RelayCommand(ExecuteSaveWorkout, CanExecuteSaveWorkout);

            UpdateVisibility();
        }

        //KOMMANDON
        public RelayCommand SaveWorkoutCommand { get; }
        public RelayCommand StrengthWorkoutCommand { get; }
        public RelayCommand CardioWorkoutCommand { get; }

        //METODER
        private void SetWorkout(bool Strength)
        {
            StrengthWorkout = Strength;
            CardioWorkout = !Strength;

            StrengthWorkoutEnabled = !Strength;
            CardioWorkoutEnabled = Strength;

            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            RepetitionsVisibility = StrengthWorkout ? Visibility.Visible : Visibility.Collapsed;
            DistanceVisibility = CardioWorkout ? Visibility.Visible : Visibility.Collapsed;
        }

        private bool CanExecuteSaveWorkout(object param)
        {
            if (StrengthWorkout)
            {
                return !string.IsNullOrWhiteSpace(Date) &&
                       !string.IsNullOrWhiteSpace(Type) &&
                       !string.IsNullOrWhiteSpace(Duration) &&
                       !string.IsNullOrWhiteSpace(Repetitions) &&
                       !string.IsNullOrWhiteSpace(Notes);
            }
            else
            {
                return !string.IsNullOrWhiteSpace(Date) &&
                       !string.IsNullOrWhiteSpace(Type) &&
                       !string.IsNullOrWhiteSpace(Duration) &&
                       !string.IsNullOrWhiteSpace(Distance) &&
                       !string.IsNullOrWhiteSpace(Notes);
            }
        }

        private void ExecuteSaveWorkout(object param)
        {
            var addWorkoutWindow = param as Window;

            if (!int.TryParse(Duration, out int workoutDuration))
            {
                MessageBox.Show("Please enter a valid number for duration");
                return;
            }

            int caloriesBurned = int.TryParse(CaloriesBurned, out int result) ? result : 0;

            if (StrengthWorkout)
            {
                if (int.TryParse(Repetitions, out int repetitions))
                {
                    var newStrengthWorkout = new StrengthWorkout(Date, Type, workoutDuration, caloriesBurned, Notes, repetitions);

                    _workoutsWindowViewModel.CurrentUser.AddWorkout(newStrengthWorkout);
                    _workoutsWindowViewModel.StrengthWorkouts.Add(newStrengthWorkout);

                    MessageBox.Show("New workout has been added");
                    addWorkoutWindow.Close();
                }
                else
                {
                    MessageBox.Show("Please enter a valid number for repetitions");
                }
            }
            else if (CardioWorkout)
            {
                if (int.TryParse(Distance, out int distance))
                {
                    var newCardioWorkout = new CardioWorkout(Date, Type, workoutDuration, caloriesBurned, Notes, distance);

                    _workoutsWindowViewModel.CurrentUser.AddWorkout(newCardioWorkout);
                    _workoutsWindowViewModel.CardioWorkouts.Add(newCardioWorkout);

                    addWorkoutWindow.Close();
                }
                else
                {
                    MessageBox.Show("Please enter a valid number for distance");
                }
            }
        }

        public AddWorkoutWindowViewModel(WorkoutsWindowViewModel workoutsWindowViewModel)
        {
            _workoutsWindowViewModel = workoutsWindowViewModel;

            SetWorkout(true);
            StrengthWorkout = true;
            CardioWorkout = false;

            //initiera kommandon
            StrengthWorkoutCommand = new RelayCommand(_ => SetWorkout(true));
            CardioWorkoutCommand = new RelayCommand(_ => SetWorkout(false));
            SaveWorkoutCommand = new RelayCommand(ExecuteSaveWorkout, CanExecuteSaveWorkout);

            UpdateVisibility();
        }

        private void UpdateCaloriesBurned()
        {
            if (int.TryParse(Duration, out int workoutDuration))
            {
                if (StrengthWorkout && int.TryParse(Repetitions, out int repetitions))
                {
                    var tempWorkout = new StrengthWorkout(Date, Type, workoutDuration, 0, Notes, repetitions);
                    CaloriesBurned = tempWorkout.CalculateCaloriesBurned().ToString();
                }
                else if (CardioWorkout && int.TryParse(Distance, out int distance))
                {
                    var tempWorkout = new CardioWorkout(Date, Type, workoutDuration, 0, Notes, distance);
                    CaloriesBurned = tempWorkout.CalculateCaloriesBurned().ToString();
                }
                else
                {
                    CaloriesBurned = "0";
                }
            }
            else
            {
                CaloriesBurned = "0";
            }
        }
    }
}