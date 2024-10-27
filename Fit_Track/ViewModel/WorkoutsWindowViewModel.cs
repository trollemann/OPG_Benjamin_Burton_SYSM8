using Fit_Track.Model;
using Fit_Track.View;
using System.Collections.ObjectModel;
using System.Windows;

namespace Fit_Track.ViewModel
{
    public class WorkoutsWindowViewModel : ViewModelBase
    {
        //egenskaper
        private string _username;
        public User CurrentUser { get; }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        private Workout _selectedWorkout;
        public Workout SelectedWorkout
        {
            get { return _selectedWorkout; }
            set
            {
                _selectedWorkout = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<CardioWorkout> CardioWorkouts { get; private set; }
        public ObservableCollection<StrengthWorkout> StrengthWorkouts { get; private set; }
        public ObservableCollection<Workout> UserWorkouts => CurrentUser.Workouts;


        //kommandon
        public RelayCommand AddWorkoutCommand { get; }
        public RelayCommand RemoveWorkoutCommand { get; }
        public RelayCommand WorkoutDetailsCommand { get; }
        public RelayCommand UserDetailsCommand { get; }
        public RelayCommand InfoCommand { get; }
        public RelayCommand SignOutCommand { get; }

        //konstruktor
        public WorkoutsWindowViewModel(User currentUser)
        {
            CurrentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));

            //initialisera träningslistorna
            CardioWorkouts = new ObservableCollection<CardioWorkout>();
            StrengthWorkouts = new ObservableCollection<StrengthWorkout>();

            //initialisera existerande träningspass
            InitializeWorkouts();

            Username = CurrentUser.Username;

            AddWorkoutCommand = new RelayCommand(ExecuteAddWorkout);
            RemoveWorkoutCommand = new RelayCommand(ExecuteRemoveWorkout, CanExecuteRemoveWorkout);
            WorkoutDetailsCommand = new RelayCommand(ExecuteWorkoutDetails, CanExecuteWorkoutDetails);
            UserDetailsCommand = new RelayCommand(ExecuteUserDetails);
            InfoCommand = new RelayCommand(ExecuteInfo);
            SignOutCommand = new RelayCommand(ExecuteSignOut);
        }

        private void InitializeWorkouts()
        {
            //lägger till existerande träningspass om användarnamnet är user
            if (CurrentUser.Username == "user")
            {
                if (!CurrentUser.Workouts.OfType<CardioWorkout>().Any())
                {
                    var cardioWorkout = new CardioWorkout("2024-11-02", "Jogging", 60, 350, "Night run", 10);
                    CurrentUser.AddWorkout(cardioWorkout);
                }

                if (!CurrentUser.Workouts.OfType<StrengthWorkout>().Any())
                {
                    var sampleStrengthWorkout = new StrengthWorkout("2024-11-01", "Upper Body", 120, 250, "Outdoor session", 10);
                    CurrentUser.AddWorkout(sampleStrengthWorkout);
                }
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

            CurrentUser.RemoveWorkout(SelectedWorkout); // Assuming there's a RemoveWorkout method in User
            SelectedWorkout = null; // Clear selection
        }

        private void ExecuteWorkoutDetails(object param)
        {
            // Create a new instance of the WorkoutDetailsWindow
            WorkoutDetailsWindow workoutDetailsWindow = new WorkoutDetailsWindow();

            // Pass the selected workout to the WorkoutDetailsWindow
            workoutDetailsWindow.DataContext = new WorkoutDetailsWindowViewModel(SelectedWorkout);

            // Show the WorkoutDetailsWindow
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
            // Reset the SelectedWorkout to refresh the UI
            SelectedWorkout = null;
            SelectedWorkout = updatedWorkout;
        }

    }
}