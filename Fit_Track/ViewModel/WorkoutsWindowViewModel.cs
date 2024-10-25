using Fit_Track.Model;
using Fit_Track.View;
using System.Collections.ObjectModel;
using System.Windows;

namespace Fit_Track.ViewModel
{
    public class WorkoutsWindowViewModel : ViewModelBase
    {
        private string _username;

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CardioWorkout> CardioWorkouts { get; private set; }
        public ObservableCollection<StrengthWorkout> StrengthWorkouts { get; private set; }

        public RelayCommand AddWorkoutCommand { get; }
        public RelayCommand RemoveWorkoutCommand { get; }
        public RelayCommand WorkoutDetailsCommand { get; }
        public RelayCommand UserDetailsCommand { get; }
        public RelayCommand InfoCommand { get; }
        public RelayCommand SignOutCommand { get; }
        public object StrengthWorkout { get; internal set; }

        public WorkoutsWindowViewModel()
        {
            // hämta en specifik användare
            var currentUser = User.GetUsers();

            // Initiera CardioWorkouts
            CardioWorkouts = new ObservableCollection<CardioWorkout>();
            StrengthWorkouts = new ObservableCollection<StrengthWorkout>();


            // Initiera och lägg till existerande cardio workouts
            InitializeCardioWorkouts();
            InitializeStrengthWorkouts();


            // kontrollera att det inte är null
            // sätter Username i WorkoutsWindowViewModel genom att hämta det från User.CurrentUser.Username
            if (User.CurrentUser != null)
            {
                Username = User.CurrentUser.Username;
            }

            AddWorkoutCommand = new RelayCommand(ExecuteAddWorkout);
            RemoveWorkoutCommand = new RelayCommand(ExecuteRemoveWorkout);
            WorkoutDetailsCommand = new RelayCommand(ExecuteWorkoutDetails);
            UserDetailsCommand = new RelayCommand(ExecuteUserDetails);
            InfoCommand = new RelayCommand(ExecuteInfo);
            SignOutCommand = new RelayCommand(ExecuteSignOut);
        }

        private void InitializeCardioWorkouts()
        {
            // Lägg till existerande cardio workouts
            CardioWorkouts.Add(new CardioWorkout("11/11/24", "Long distance", 60, 0, "run around the park", 10));
            // Lägg till fler pass om så önskas
        }

        private void InitializeStrengthWorkouts()
        {
            StrengthWorkouts.Add(new StrengthWorkout("14/11/24", "Upper body", 60, 0, "intense sets", 10));
        }

        private void ExecuteUserDetails(object param)
        {
            UserDetailsWindow userDetailsWindow = new UserDetailsWindow();
            userDetailsWindow.Show();
        }

        private void ExecuteAddWorkout(object param)
        {
            AddWorkoutWindow addWorkoutWindow = new AddWorkoutWindow { DataContext = new AddWorkoutWindowViewModel(this) };
            addWorkoutWindow.Show();
        }

        private void ExecuteRemoveWorkout(object param)
        {
            // Implementera logik för att ta bort en vald workout
            // Här kan du använda en metod för att få vald item från ListBox och ta bort den från CardioWorkouts
        }

        private void ExecuteWorkoutDetails(object param)
        {
            WorkoutDetailsWindow workoutDetailsWindow = new WorkoutDetailsWindow();
            workoutDetailsWindow.Show();
        }

        private void ExecuteInfo(object param)
        {
            MessageBox.Show("Fit Track is a fitness and health platform designed to help users reach their fitness goals through easy planning, tracking, and analysis of workouts. " +
                "The buttons are pretty self-explanatory; just start adding workouts to get going.\n\n " +
                "The person behind the app, Fit Track, is a genius with seriously impressive abs, big guns, and more.\n-ChatGPT");
        }

        private void ExecuteSignOut(object param)
        {
            var workoutsWindow = param as Window;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            workoutsWindow.Close();
        }
    }
}