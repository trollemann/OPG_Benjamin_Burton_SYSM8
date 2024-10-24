using Fit_Track.Model;
using Fit_Track.View;
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

        public RelayCommand AddWorkoutCommand { get; }
        public RelayCommand RemoveWorkoutCommand { get; }
        public RelayCommand WorkoutDetailsCommand { get; }
        public RelayCommand UserDetailsCommand { get; }
        public RelayCommand InfoCommand { get; }
        public RelayCommand SignOutCommand { get; }

        public WorkoutsWindowViewModel()
        {
            //hämta en specifik användare
            var currentUser = User.GetUsers();

            //kontrollera att det inte är null
            //sätter Username i WorkoutsWindowViewModel genom att hämta det från User.CurrentUser.Username
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

        private void ExecuteUserDetails(object param)
        {
            UserDetailsWindow userDetailsWindow = new UserDetailsWindow();

            userDetailsWindow.Show();
        }

        private void ExecuteAddWorkout(object param)
        {
            AddWorkoutWindow addWorkoutWindow = new AddWorkoutWindow();

            addWorkoutWindow.Show();
        }

        private void ExecuteRemoveWorkout(object param)
        {

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