using Fit_Track.Model;
using Fit_Track.View;
using System.Collections.ObjectModel;
using System.Windows;

namespace Fit_Track.ViewModel
{
    public class WorkoutsWindowViewModel : ViewModelBase
    {
        //referens till den aktuella användaren
        public User CurrentUser { get; }

        //EGENSKAPER
        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        //valda träningen
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
        private ObservableCollection<Workout> _userWorkouts;
        public ObservableCollection<Workout> UserWorkouts
        {
            get => _userWorkouts;
            private set
            {
                _userWorkouts = value;
                OnPropertyChanged();
            }
        }

        //samlingar för att hålla cardio- och styrketräningar
        public ObservableCollection<CardioWorkout> CardioWorkouts { get; private set; }
        public ObservableCollection<StrengthWorkout> StrengthWorkouts { get; private set; }

        //KOMMANDON
        public RelayCommand AddWorkoutCommand { get; }
        public RelayCommand RemoveWorkoutCommand { get; }
        public RelayCommand WorkoutDetailsCommand { get; }
        public RelayCommand UserDetailsCommand { get; }
        public RelayCommand InfoCommand { get; }
        public RelayCommand SignOutCommand { get; }

        //KONSTRUKTOR
        public WorkoutsWindowViewModel(User currentUser)
        {
            CurrentUser = currentUser;

            //initialisera träningslistor
            CardioWorkouts = new ObservableCollection<CardioWorkout>();
            StrengthWorkouts = new ObservableCollection<StrengthWorkout>();

            //initialisera UserWorkouts med en ny ObservableCollection
            UserWorkouts = new ObservableCollection<Workout>();

            //initialisera existerande träningspass
            InitializeWorkouts();

            //sätt användarnamnet från CurrentUser
            Username = CurrentUser.Username;

            //initiera kommandon
            AddWorkoutCommand = new RelayCommand(ExecuteAddWorkout);
            RemoveWorkoutCommand = new RelayCommand(ExecuteRemoveWorkout, CanExecuteRemoveWorkout);
            WorkoutDetailsCommand = new RelayCommand(ExecuteWorkoutDetails, CanExecuteWorkoutDetails);
            UserDetailsCommand = new RelayCommand(ExecuteUserDetails);
            InfoCommand = new RelayCommand(ExecuteInfo);
            SignOutCommand = new RelayCommand(ExecuteSignOut);
        }

        //METODER
        private void InitializeWorkouts()
        {
            //töm befintliga samlingar för att undvika dubbletter vid upprepade anrop
            CardioWorkouts.Clear();
            StrengthWorkouts.Clear();
            UserWorkouts.Clear();

            if (CurrentUser.Admin)
            {
                //admin hämtar träningspass från alla användare
                foreach (var user in User.GetUsers())
                {
                    foreach (var workout in user.Workouts.ToList())
                    {
                        AddWorkoutToCollections(workout);
                    }
                }
            }
            else
            {
                //om ej admin, lägg till endast träningspass från CurrentUser
                foreach (var workout in CurrentUser.Workouts.ToList())
                {
                    AddWorkoutToCollections(workout);
                }
            }
        }

        private bool CanExecuteWorkoutDetails(object param)
        {
            return SelectedWorkout != null;
        }

        private void ExecuteUserDetails(object param)
        {
            var userDetailsWindow = new UserDetailsWindow
            {
                //sätt DataContext till användardetaljer
                DataContext = new UserDetailsViewModel(CurrentUser)
            };
            userDetailsWindow.Show();
        }

        private void ExecuteAddWorkout(object param)
        {
            var addWorkoutWindow = new AddWorkoutWindow { DataContext = new AddWorkoutWindowViewModel(this) };
            addWorkoutWindow.Show();
        }

        private bool CanExecuteRemoveWorkout(object param)
        {
            return SelectedWorkout != null;
        }

        private void ExecuteRemoveWorkout(object param)
        {
            if (SelectedWorkout == null) return;

            //ta bort träningspass från rätt samling
            if (SelectedWorkout is CardioWorkout cardioWorkout)
            {
                CardioWorkouts.Remove(cardioWorkout);
            }
            else if (SelectedWorkout is StrengthWorkout strengthWorkout)
            {
                StrengthWorkouts.Remove(strengthWorkout);
            }

            //om admin, hitta användare o ta bort träningspass från användarens samling
            if (CurrentUser.Admin)
            {
                var owner = User.GetUsers().FirstOrDefault(user => user.Workouts.Contains(SelectedWorkout));
                owner?.RemoveWorkout(SelectedWorkout);
            }
            else
            {
                //om ej admin, ta bort träningspass från CurrentUser:s lista
                CurrentUser.RemoveWorkout(SelectedWorkout);
            }

            //ta bort träningspasset från UserWorkouts och nollställ vald träning
            UserWorkouts.Remove(SelectedWorkout);
            SelectedWorkout = null;
        }

        private void ExecuteWorkoutDetails(object param)
        {
            WorkoutDetailsWindow workoutDetailsWindow = new WorkoutDetailsWindow
            {
                DataContext = new WorkoutDetailsWindowViewModel(SelectedWorkout)
            };
            var workoutsWindow = param as Window;
            workoutDetailsWindow.Show();
        }

        private void ExecuteInfo(object param)
        {
            MessageBox.Show("Fit Track är en plattform för att hjälpa användare att nå sina träningsmål genom enkel planering, spårning och analys av träningspass. " +
                "Knappfunktionerna är självförklarande - börja lägga till träningspass för att komma igång.\n\n" +
                "- ChatGPT");
        }

        private void ExecuteSignOut(object param)
        {
            var workoutsWindow = param as Window;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            workoutsWindow.Close();
        }

        public void UpdateWorkoutInList(Workout updatedWorkout)
        {
            //nollställ vald träning för att uppdatera UI
            SelectedWorkout = null;
            SelectedWorkout = updatedWorkout;
        }

        private void AddWorkoutToCollections(Workout workout)
        {
            //lägg till träningspass i UserWorkouts, antingen i cardioWorkouts eller strengthWorkouts
            UserWorkouts.Add(workout);
            if (workout is CardioWorkout cardioWorkout)
            {
                CardioWorkouts.Add(cardioWorkout);
            }
            else if (workout is StrengthWorkout strengthWorkout)
            {
                StrengthWorkouts.Add(strengthWorkout);
            }
        }
    }
}