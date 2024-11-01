using Fit_Track.Model;
using Fit_Track.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace Fit_Track.ViewModel
{
    public class WorkoutsWindowViewModel : ViewModelBase
    {
        public List<string> SortingList { get; } = new List<string> { "Date", "Type", "Duration" };

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

        public ICollectionView SortWorkouts { get; set; }

        private string _selectedSorting;
        public string SelectedSorting
        {
            get => _selectedSorting;
            set
            {
                _selectedSorting = value;
                OnPropertyChanged();
                SortAndFilter();
            }
        }

        private string _searchFilter;
        public string SearchFilter
        {
            get => _searchFilter;
            set
            {
                _searchFilter = value;
                OnPropertyChanged();
                SortAndFilter();
            }
        }
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

            WorkoutsList = new ObservableCollection<Workout>();
            SortWorkouts = CollectionViewSource.GetDefaultView(WorkoutsList);

            Username = CurrentUser.Username;

            AddWorkoutCommand = new RelayCommand(ExecuteAddWorkout);
            RemoveWorkoutCommand = new RelayCommand(ExecuteRemoveWorkout, CanExecuteRemoveWorkout);
            CopyWorkoutCommand = new RelayCommand(ExecuteCopyWorkout, CanExecuteCopyWorkout);
            WorkoutDetailsCommand = new RelayCommand(ExecuteWorkoutDetails, CanExecuteWorkoutDetails);
            UserDetailsCommand = new RelayCommand(ExecuteUserDetails);
            InfoCommand = new RelayCommand(ExecuteInfo);
            SignOutCommand = new RelayCommand(ExecuteSignOut);

            InitializeWorkouts();
            SortAndFilter();
        }

        //METODER
        private void ExecuteUserDetails(object param)
        {
            var userDetailsWindow = new UserDetailsWindow { DataContext = new UserDetailsViewModel(CurrentUser) };

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
            if (SelectedWorkout == null)
            {
                MessageBox.Show("Please select a workout first");
            }

            if (CurrentUser.Admin)
            {
                var user = User.GetUsers().FirstOrDefault(u => u._workout.Contains(SelectedWorkout));
                user.RemoveWorkout(SelectedWorkout);
            }
            else
            {
                CurrentUser.RemoveWorkout(SelectedWorkout);
            }

            InitializeWorkouts();
        }
        private bool CanExecuteCopyWorkout(object param)
        {
            return SelectedWorkout != null;
        }
        private void ExecuteCopyWorkout(object param)
        {
            if (SelectedWorkout == null)
            {
                MessageBox.Show("Please select a workout first");
            }

            Workout copiedWorkout = null;

            if (SelectedWorkout is StrengthWorkout strengthWorkout)
            {
                copiedWorkout = new StrengthWorkout(
                strengthWorkout.Date,
                strengthWorkout.Type,
                strengthWorkout.Duration,
                strengthWorkout.CaloriesBurned,
                strengthWorkout.Notes,
                strengthWorkout.Repetitions);
            }
            else if (SelectedWorkout is CardioWorkout cardioWorkout)
            {
                copiedWorkout = new CardioWorkout(
                cardioWorkout.Date,
                cardioWorkout.Type,
                cardioWorkout.Duration,
                cardioWorkout.CaloriesBurned,
                cardioWorkout.Notes,
                cardioWorkout.Distance);
            }

            var copyWorkoutWindow = new CopyWorkoutWindow { DataContext = new CopyWorkoutWindowViewModel(copiedWorkout, this) };

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
            if (SelectedWorkout == null)
            {
                MessageBox.Show("Please select a workout first");
            }

            var workoutDetailsWindow = new WorkoutDetailsWindow { DataContext = new WorkoutDetailsWindowViewModel(SelectedWorkout) };
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
        //sätter det valda träningspasset värden i WorkoutDetails o skriver sedan över (ifall det görs ändringar)
        public void SetSelectedWorkout(Workout workout)
        {
            SelectedWorkout = workout;
            OnPropertyChanged(nameof(SelectedWorkout));
        }

        //rensar alla träningspass innan den fylls med ny data
        public void InitializeWorkouts()
        {
            WorkoutsList.Clear();

            //om nuvarande användare är admin, lägg till alla träningspass från alla användare
            if (CurrentUser.Admin)
            {
                foreach (var user in User.GetUsers()) 
                {
                    foreach (var workout in user._workout)
                    {
                        WorkoutsList.Add(workout);
                    }
                }
            }
            //om användare inte är admin, lägg enbart till nuvarande användarens träningspass
            else
            {
                foreach (var workout in CurrentUser._workout)
                {
                    WorkoutsList.Add(workout);
                }
            }
        }

        private void SortAndFilter()
        {
            //sätter ett filter för att sortera träningspass
            SortWorkouts.Filter = item =>
            {
                //kontrollerar om item är av typen Workout
                if (item is Workout workout)
                {
                    //returnerar true om något av villkoren matchar
                    //det står dd/MM/yy, men man söker (MM/dd/yyyy) : söka duration = (hours:minutes:seconds)
                    return string.IsNullOrEmpty(SearchFilter) || workout.Type.Contains(SearchFilter, StringComparison.OrdinalIgnoreCase) ||
                           workout.Duration.ToString().Contains(SearchFilter) || workout.Date.ToString("dd/MM/yyyy").Contains(SearchFilter);
                }
                return false;
            };

            //rensar tidigare sortering
            SortWorkouts.SortDescriptions.Clear();

            switch (SelectedSorting)
            {
                case "Date":
                    //sorterar efter Date
                    SortWorkouts.SortDescriptions.Add(new SortDescription(nameof(Workout.Date), ListSortDirection.Ascending));
                    break;

                case "Type":
                    //sorterar efter Type
                    SortWorkouts.SortDescriptions.Add(new SortDescription(nameof(Workout.Type), ListSortDirection.Ascending));
                    break;

                case "Duration":
                    //sorterar efter Duration
                    SortWorkouts.SortDescriptions.Add(new SortDescription(nameof(Workout.Duration), ListSortDirection.Descending));
                    break;
            }
        }
    }
}