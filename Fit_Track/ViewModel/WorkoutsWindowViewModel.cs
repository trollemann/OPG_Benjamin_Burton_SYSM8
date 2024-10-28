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
        public ObservableCollection<Workout> UserWorkouts => CurrentUser.Workouts;

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

            //initialisera träningslistorna
            CardioWorkouts = new ObservableCollection<CardioWorkout>();
            StrengthWorkouts = new ObservableCollection<StrengthWorkout>();

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
            //existerande träningspass om användarnamnet är "user"
            if (CurrentUser.Username == "user")
            {
                var cardioWorkout = new CardioWorkout("2024-11-02", "Jogging", 60, 350, "Night run", 10);
                CurrentUser.AddWorkout(cardioWorkout);

                var sampleStrengthWorkout = new StrengthWorkout("2024-11-01", "Upper Body", 120, 250, "Outdoor session", 10);
                CurrentUser.AddWorkout(sampleStrengthWorkout);
            }

            //lägg till existerande träningspass från CurrentUser till listorna
            foreach (var workout in CurrentUser.Workouts)
            {
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
            if (SelectedWorkout is CardioWorkout cardioWorkout)
            {
                CardioWorkouts.Remove(cardioWorkout);
            }
            else if (SelectedWorkout is StrengthWorkout strengthWorkout)
            {
                StrengthWorkouts.Remove(strengthWorkout);
            }

            //ta bort träningspasset från CurrentUser
            CurrentUser.RemoveWorkout(SelectedWorkout);
            SelectedWorkout = null;
        }

        private void ExecuteWorkoutDetails(object param)
        {
            WorkoutDetailsWindow workoutDetailsWindow = new WorkoutDetailsWindow();

            //passera valda träningen till WorkoutDetailsWindow
            workoutDetailsWindow.DataContext = new WorkoutDetailsWindowViewModel(SelectedWorkout);
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

        public void UpdateWorkoutInList(Workout updatedWorkout)
        {
            //återställ den valda träningen för att uppdatera UI
            SelectedWorkout = null;
            SelectedWorkout = updatedWorkout;
        }
    }
}