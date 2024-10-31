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
                CalculateCaloriesBurned();

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
                CalculateCaloriesBurned();
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
                CalculateCaloriesBurned();
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

            if (StrengthWorkoutEnabled)
            {
                Workout newStrengthWorkout = new StrengthWorkout(Date, Type, 0, 0, Notes, 0);
                _workoutsWindowViewModel.CurrentUser.AddWorkout(newStrengthWorkout);
            }
            else
            {
                Workout newCardioWorkout = new CardioWorkout(Date, Type, 0, 0, Notes, 0);
                _workoutsWindowViewModel.CurrentUser.AddWorkout(newCardioWorkout);
            }

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

        private void SetWorkout(bool Strength)
        {
            StrengthWorkout = Strength;
            CardioWorkout = !Strength;

            StrengthWorkoutEnabled = !Strength;
            CardioWorkoutEnabled = Strength;

            UpdateVisibility();
        }

        private void CalculateCaloriesBurned()
        {
            //försöker omvandla Duration till heltal
            if (!int.TryParse(Duration, out int duration))
            {
                CaloriesBurned = "0";
                return;
            }

            //försöker omvandla Repetitions till heltal
            if (StrengthWorkout && int.TryParse(Repetitions, out int repetitions))
            {
                CaloriesBurned = (repetitions * duration).ToString();
            }
            //försöker omvandla Distance till heltal och beräkna kalorier
            else if (CardioWorkout && int.TryParse(Distance, out int distance))
            {
                CaloriesBurned = (distance * duration).ToString();
            }
            else
            {
                CaloriesBurned = "0";
            }
        }
    }
}