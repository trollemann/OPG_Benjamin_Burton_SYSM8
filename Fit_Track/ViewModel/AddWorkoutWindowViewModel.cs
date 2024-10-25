using Fit_Track.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;


namespace Fit_Track.ViewModel
{
    public class AddWorkoutWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Workout> Workouts { get; set; } = new ObservableCollection<Workout>();

        private WorkoutsWindowViewModel _workoutsWindowViewModel;


        private bool _isStrengthWorkoutEnabled = true;
        public bool IsStrengthWorkoutEnabled
        {
            get { return _isStrengthWorkoutEnabled; }
            set
            {
                _isStrengthWorkoutEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _isCardioWorkoutEnabled = true;
        public bool IsCardioWorkoutEnabled
        {
            get { return _isCardioWorkoutEnabled; }
            set
            {
                _isCardioWorkoutEnabled = value;
                OnPropertyChanged();
            }
        }

        private Visibility _repetitionsVisibility = Visibility.Visible;
        public Visibility RepetitionsVisibility
        {
            get { return _repetitionsVisibility; }
            set
            {
                _repetitionsVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _distanceVisibility = Visibility.Collapsed;
        public Visibility DistanceVisibility
        {
            get { return _distanceVisibility; }
            set
            {
                _distanceVisibility = value;
                OnPropertyChanged();
            }
        }

        private string _date;
        public string Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }

        private string _type;
        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        private string _duration;
        public string Duration
        {
            get { return _duration; }
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
            get { return _caloriesBurned; }
            private set
            {
                _caloriesBurned = value;
                OnPropertyChanged();
            }
        }

        private string _notes;
        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }

        private string _repetitions;
        public string Repetitions
        {
            get { return _repetitions; }
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
            get { return _distance; }
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
            get { return _strengthWorkout; }
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
            get { return _cardioWorkout; }
            set
            {
                _cardioWorkout = value;
                OnPropertyChanged();
                UpdateVisibility();
            }
        }

        public ICommand SaveWorkoutCommand { get; }
        public ICommand StrengthWorkoutCommand { get; }
        public ICommand CardioWorkoutCommand { get; }

        public AddWorkoutWindowViewModel()
        {
            SetWorkout(true);

            StrengthWorkout = true; // Default inställning för Styrketräning
            CardioWorkout = false;

            // Initiera kommandon
            StrengthWorkoutCommand = new RelayCommand(_ => SetWorkout(true));
            CardioWorkoutCommand = new RelayCommand(_ => SetWorkout(false));
            SaveWorkoutCommand = new RelayCommand(ExecuteSaveWorkout, CanExecuteSaveWorkout);

            UpdateVisibility();
        }

        private void SetWorkout(bool isStrength)
        {
            StrengthWorkout = isStrength;
            CardioWorkout = !isStrength;

            // Uppdatera knappens tillstånd
            IsStrengthWorkoutEnabled = !isStrength;
            IsCardioWorkoutEnabled = isStrength;

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
                       !string.IsNullOrWhiteSpace(Repetitions) && // Kontrollera att Repetitions inte är tom
                       !string.IsNullOrWhiteSpace(Notes);
            }
            else
            {
                return !string.IsNullOrWhiteSpace(Date) &&
                       !string.IsNullOrWhiteSpace(Type) &&
                       !string.IsNullOrWhiteSpace(Duration) &&
                       !string.IsNullOrWhiteSpace(Distance) && // Kontrollera att Distance inte är tom
                       !string.IsNullOrWhiteSpace(Notes);
            }
        }

        private void ExecuteSaveWorkout(object param)
        {
            var addWorkoutWindow = param as Window;

            // Parse and validate duration and calories
            if (!int.TryParse(Duration, out int workoutDuration))
            {
                MessageBox.Show("Please enter a valid number for duration.");
                return;
            }

            int caloriesBurned = int.TryParse(CaloriesBurned, out int result) ? result : 0;

            if (StrengthWorkout)
            {
                if (int.TryParse(Repetitions, out int repetitions))
                {
                    var newStrengthWorkout = new StrengthWorkout(Date, Type, workoutDuration, caloriesBurned, Notes, repetitions);

                    // Add to both CurrentUser and the local StrengthWorkouts collection
                    _workoutsWindowViewModel.CurrentUser.AddWorkout(newStrengthWorkout);
                    _workoutsWindowViewModel.StrengthWorkouts.Add(newStrengthWorkout);

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

                    // Add to both CurrentUser and the local CardioWorkouts collection
                    _workoutsWindowViewModel.CurrentUser.AddWorkout(newCardioWorkout);
                    _workoutsWindowViewModel.CardioWorkouts.Add(newCardioWorkout);

                    addWorkoutWindow.Close();
                }
                else
                {
                    MessageBox.Show("Please enter a valid number for distance.");
                }
            }
        }

        public AddWorkoutWindowViewModel(WorkoutsWindowViewModel workoutsWindowViewModel)
        {
            _workoutsWindowViewModel = workoutsWindowViewModel; // Spara referensen

            SetWorkout(true);
            StrengthWorkout = true; // Default inställning för Styrketräning
            CardioWorkout = false;

            // Initiera kommandon
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