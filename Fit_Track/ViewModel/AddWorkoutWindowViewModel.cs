using Fit_Track.Model;
using Fit_Track.View;
using System.Collections.ObjectModel;
using System.Windows;


namespace Fit_Track.ViewModel
{
    public class AddWorkoutWindowViewModel : ViewModelBase
    {
        //EGENSKAPER
        public User CurrentUser { get; }

        private Workout _selectedWorkout;
        public Workout SelectedWorkout
        {
            get => _selectedWorkout;
            set
            {
                _selectedWorkout = value;
                OnPropertyChanged();
            }
        }

        //samling av användarens träningspass
        private ObservableCollection<Workout> _workouts;
        public ObservableCollection<Workout> WorkoutsList
        {
            get => _workouts;
            set
            {
                _workouts = value;
                OnPropertyChanged();
            }
        }

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

        // Ändrat till DateTime
        private DateTime _date;
        public DateTime Date
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

        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                CalculateCaloriesBurned();
                OnPropertyChanged();
            }
        }

        private int _caloriesBurned;
        public int CaloriesBurned
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

        private int _repetitions;
        public int Repetitions
        {
            get => _repetitions;
            set
            {
                _repetitions = value;
                CalculateCaloriesBurned();
                OnPropertyChanged();
            }
        }

        private int _distance;
        public int Distance
        {
            get => _distance;
            set
            {
                _distance = value;
                CalculateCaloriesBurned();
                OnPropertyChanged();
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

        private WorkoutsWindowViewModel _workoutsWindowViewModel;
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

            //initialisera UserWorkouts med en ny ObservableCollection (uppdateras direkt)
            WorkoutsList = new ObservableCollection<Workout>();
            workoutsWindowViewModel.InitializeWorkouts();

            UpdateVisibility();
        }

        //KOMMANDON
        public RelayCommand SaveWorkoutCommand { get; }
        public RelayCommand StrengthWorkoutCommand { get; }
        public RelayCommand CardioWorkoutCommand { get; }

        //METODER
        private bool CanExecuteSaveWorkout(object param)
        {
            if (StrengthWorkout)
            {
                return !string.IsNullOrWhiteSpace(Date.ToString()) &&
                       !string.IsNullOrWhiteSpace(Type) &&
                       Repetitions >= 0 &&
                       CaloriesBurned >= 0 &&
                       !string.IsNullOrWhiteSpace(Notes);
            }
            else
            {
                return !string.IsNullOrWhiteSpace(Date.ToString()) &&
                       !string.IsNullOrWhiteSpace(Type) &&
                       Distance >= 0 &&
                       CaloriesBurned >= 0 &&
                       !string.IsNullOrWhiteSpace(Notes);
            }
        }

        private void ExecuteSaveWorkout(object param)
        {
            Workout workout;

            if (StrengthWorkout)
            {
                workout = new StrengthWorkout(Date, Type, Duration, CaloriesBurned, Notes, Repetitions);
            }
            else
            {
                workout = new CardioWorkout(Date, Type, Duration, CaloriesBurned, Notes, Distance);
            }

            // Lägg till workout i listan
            _workoutsWindowViewModel.CurrentUser.AddWorkout(workout);

            MessageBox.Show("New workout has been added");
            var workoutsWindow = new WorkoutsWindow();
            workoutsWindow.Show();
            Application.Current.Windows[0].Close();
        }

        private void UpdateVisibility()
        {
            RepetitionsVisibility = StrengthWorkout ? Visibility.Visible : Visibility.Collapsed;
            DistanceVisibility = CardioWorkout ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SetWorkout(bool isStrength)
        {
            StrengthWorkout = isStrength;
            CardioWorkout = !isStrength;

            StrengthWorkoutEnabled = !isStrength;
            CardioWorkoutEnabled = isStrength;

            UpdateVisibility();
        }

        private void CalculateCaloriesBurned()
        {
            if (StrengthWorkout)
            {
                CaloriesBurned = (int)Duration.TotalMinutes * Repetitions; // Räkna kalorier baserat på minuter
            }
            else if (CardioWorkout)
            {
                CaloriesBurned = (int)Duration.TotalMinutes * (int)Distance; // Räkna kalorier baserat på minuter
            }
        }
    }
}