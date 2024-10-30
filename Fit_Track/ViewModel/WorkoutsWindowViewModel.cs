using Fit_Track.Model;
using Fit_Track.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            set
            {
                _userWorkouts = value;
                OnPropertyChanged();
            }
        }

        //samlingar för att hålla cardio- och styrketräningar
        private ObservableCollection<CardioWorkout> _cardioWorkouts;
        public ObservableCollection<CardioWorkout> CardioWorkouts
        {
            get => _cardioWorkouts;
            private set
            {
                _cardioWorkouts = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<StrengthWorkout> _strengthWorkouts;
        public ObservableCollection<StrengthWorkout> StrengthWorkouts
        {
            get => _strengthWorkouts;
            private set
            {
                _strengthWorkouts = value;
                OnPropertyChanged();
            }
        }

        //KOMMANDON
        public RelayCommand AddWorkoutCommand { get; }
        public RelayCommand RemoveWorkoutCommand { get; }
        public RelayCommand CopyWorkoutCommand { get; }
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
            CopyWorkoutCommand = new RelayCommand(ExecuteCopyWorkout, CanExecuteCopyWorkout);
            WorkoutDetailsCommand = new RelayCommand(ExecuteWorkoutDetails, CanExecuteWorkoutDetails);
            UserDetailsCommand = new RelayCommand(ExecuteUserDetails);
            InfoCommand = new RelayCommand(ExecuteInfo);
            SignOutCommand = new RelayCommand(ExecuteSignOut);
        }

        //METODER
        private void ExecuteUserDetails(object param)
        {
            var userDetailsWindow = new UserDetailsWindow
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
            addWorkoutWindow.Show();
        }

        private bool CanExecuteRemoveWorkout(object param)
        {
            return SelectedWorkout != null;
        }

        private void ExecuteRemoveWorkout(object param)
        {
            if (SelectedWorkout == null) return;

            CurrentUser.Workouts.Remove(SelectedWorkout);
            
            if(SelectedWorkout is CardioWorkout)
            {
                _userWorkouts.Remove(SelectedWorkout);
            }
            else
            {
                _userWorkouts.Remove(SelectedWorkout);
            }

            //om admin, hitta användare o ta bort träningspass från användarens samling
            if (CurrentUser.Admin)
            {
                var owner = User.GetUsers().FirstOrDefault(user => user.Workouts.Contains(SelectedWorkout));
                owner?.RemoveWorkout(SelectedWorkout);
                OnPropertyChanged();
            }
            else
            {
                //om ej admin, ta bort träningspass från CurrentUser:s lista
                CurrentUser.RemoveWorkout(SelectedWorkout);
                OnPropertyChanged();
            }

            //ta bort träningspasset från UserWorkouts och nollställ vald träning
            UserWorkouts.Remove(SelectedWorkout);
            InitializeWorkouts();
            SelectedWorkout = null;
        }




        private bool CanExecuteCopyWorkout(object param)
        {
            return SelectedWorkout != null;
        }

        private void ExecuteCopyWorkout(object param)
        {
            CopyWorkoutWindow copyWorkoutWindow = new CopyWorkoutWindow
            {
                DataContext = new WorkoutDetailsWindowViewModel(SelectedWorkout)
            };
            var workoutsWindow = param as Window;
            copyWorkoutWindow.Show();
            Application.Current.Windows[0].Close();
        }

            private bool CanExecuteWorkoutDetails(object param)
        {
            return SelectedWorkout != null;
        }

        private void ExecuteWorkoutDetails(object param)
        {
            WorkoutDetailsWindow workoutDetailsWindow = new WorkoutDetailsWindow
            {
                DataContext = new WorkoutDetailsWindowViewModel(SelectedWorkout)
            };
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
            var workoutsWindow = param as Window;
            var mainWindow = new MainWindow();
            mainWindow.Show();
            workoutsWindow.Close();
        }

        private void AddWorkoutToList(Workout workout)
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

        public void UpdateWorkoutList(Workout updatedWorkout)
        {
            //nollställ vald träning för att uppdatera UI
            SelectedWorkout = null;
            SelectedWorkout = updatedWorkout;
        }

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
                        AddWorkoutToList(workout);
                    }
                }
            }
            else
            {
                //om ej admin, lägg till endast träningspass från CurrentUser
                foreach (var workout in CurrentUser.Workouts.ToList())
                {
                    AddWorkoutToList(workout);
                }
            }
        }
    }
}