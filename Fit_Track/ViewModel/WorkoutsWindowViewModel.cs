using Fit_Track.Model;
using Fit_Track.View;
using System.Collections.ObjectModel;
using System.Windows;

namespace Fit_Track.ViewModel
{
    public class WorkoutsWindowViewModel : ViewModelBase
    {
        //EGENSKAPER
        public User CurrentUser { get; }

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

            //initialisera UserWorkouts med en ny ObservableCollection (uppdateras direkt)
            WorkoutsList = new ObservableCollection<Workout>();

            //sätt användarnamnet från CurrentUser (userDetails-knappen)
            Username = CurrentUser.Username;

            //initiera kommandon
            AddWorkoutCommand = new RelayCommand(ExecuteAddWorkout);
            RemoveWorkoutCommand = new RelayCommand(ExecuteRemoveWorkout, CanExecuteRemoveWorkout);
            WorkoutDetailsCommand = new RelayCommand(ExecuteWorkoutDetails, CanExecuteWorkoutDetails);
            UserDetailsCommand = new RelayCommand(ExecuteUserDetails);
            InfoCommand = new RelayCommand(ExecuteInfo);
            SignOutCommand = new RelayCommand(ExecuteSignOut);

            //initialisera existerande träningspass
            InitializeWorkouts();
        }

        //METODER
        private void ExecuteUserDetails(object param)
        {
            var userDetailsWindow = new UserDetailsWindow()
            {
                //hämta information från CurrentUser till userDetails   
                DataContext = new UserDetailsViewModel(CurrentUser)
            };

            var workoutsWindow = param as Window;
            userDetailsWindow.Show();
            Application.Current.Windows[0].Close();
        }

        private void ExecuteAddWorkout(object param)
        {
            var addWorkoutWindow = new AddWorkoutWindow { DataContext = new AddWorkoutWindowViewModel(this) };

            var workoutsWindow = param as Window;
            addWorkoutWindow.Show();
            Application.Current.Windows[0].Close();
        }

        private bool CanExecuteRemoveWorkout(object param)
        {
            return SelectedWorkout != null;
        }

        private void ExecuteRemoveWorkout(object param)
        {
            if (CurrentUser.Admin)
            {
                var user = User.GetUsers().FirstOrDefault(u => u._workouts.Contains(SelectedWorkout));
                user.RemoveWorkout(SelectedWorkout);
            }
            else
            {
                CurrentUser.RemoveWorkout(SelectedWorkout);
            }

            InitializeWorkouts();
        }

        private bool CanExecuteWorkoutDetails(object param)
        {
            return SelectedWorkout != null;
        }

        private void ExecuteWorkoutDetails(object param)
        {
            WorkoutDetailsWindow workoutDetailsWindow = new WorkoutDetailsWindow { DataContext = new WorkoutDetailsWindowViewModel(SelectedWorkout) }; 

            var workoutsWindow = param as Window;
            workoutDetailsWindow.Show();
            Application.Current.Windows[0].Close();
        }

        private void ExecuteInfo(object param)
        {
            MessageBox.Show("Fit Track is a platform designed to help users achieve their fitness goals " +
                            "through easy planning, tracking, and analysis of workouts. The button functions are " +
                            "self-explanatory – start adding workouts to get started.\n\nThe person behind" +
                            " Fit Track is a genius with seriously impressive abs, big guns, and more.\n-ChatGPT");
        }

        private void ExecuteSignOut(object param)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Application.Current.Windows[0].Close();
        }

        private void AddWorkoutToList(Workout workout)
        {
            //lägg till träningspass i UserWorkouts, antingen i cardioWorkouts eller strengthWorkouts
            if (workout is CardioWorkout cardioWorkout)
            {
                WorkoutsList.Add(cardioWorkout);
            }
            else if (workout is StrengthWorkout strengthWorkout)
            {
                WorkoutsList.Add(strengthWorkout);
            }
        }

        public void UpdateWorkoutList(Workout updatedWorkout)
        {
            SelectedWorkout = updatedWorkout;
        }

        public void InitializeWorkouts()
        {
            WorkoutsList.Clear();

            if (CurrentUser.Admin)
            {
                foreach (var user in User.GetUsers())
                {
                    foreach (var workout in user._workouts)
                    {
                        WorkoutsList.Add(workout);
                    }
                }
            }
            else
            {
                foreach (var workout in CurrentUser._workouts)
                {
                    WorkoutsList.Add(workout);
                }
            }
        }
    }
}