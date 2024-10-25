using Fit_Track.Model;
using System.Windows;
using System.Windows.Input;

namespace Fit_Track.ViewModel
{
    public class AddWorkoutWindowViewModel : ViewModelBase
    {
        private string _date;
        public string Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }

        private string _type;
        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        private string _duration;
        public string Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                OnPropertyChanged();
                UpdateCaloriesBurned(); // uppdatera brända kalorier när duration ändras
            }
        }

        private string _caloriesBurned;
        public string CaloriesBurned
        {
            get { return _caloriesBurned; }
            private set
            {
                _caloriesBurned = value;
                OnPropertyChanged();
            }
        }

        private string _notes;
        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }

        private string _repetitions;
        public string Repetitions
        {
            get { return _repetitions; }
            set
            {
                _repetitions = value;
                OnPropertyChanged();
                UpdateCaloriesBurned(); //uppdatera brända kalorier när repetitions ändras
            }
        }

        private string _distance;
        public string Distance
        {
            get { return _distance; }
            set
            {
                _distance = value;
                OnPropertyChanged();
                UpdateCaloriesBurned(); //uppdatera brända kalorier när repetitions ändras
            }
        }

        private bool _strengthWorkout;
        public bool StrengthWorkout
        {
            get { return _strengthWorkout; }
            set
            {
                _strengthWorkout = value;
                OnPropertyChanged();
                UpdateCaloriesBurned(); //uppdatera brända kalorier om typ ändras
            }
        }

        private bool _cardioWorkout;
        public bool CardioWorkout
        {
            get { return _cardioWorkout; }
            set
            {
                _cardioWorkout = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand StrengthWorkoutCommand { get; }
        public ICommand CardioWorkoutCommand { get; }

        public AddWorkoutWindowViewModel()
        {
            StrengthWorkout = true; //styrketräning förvalt)
            CardioWorkout = false;

            // Initiera kommandon
            StrengthWorkoutCommand = new RelayCommand(_ => SetWorkout(true));
            CardioWorkoutCommand = new RelayCommand(_ => SetWorkout(false));
            SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
        }

        private void SetWorkout(bool isStrength)
        {
            StrengthWorkout = isStrength;
            CardioWorkout = !isStrength;
        }

        private bool CanExecuteSave(object param)
        {
            return !string.IsNullOrWhiteSpace(Date) &&
                   !string.IsNullOrWhiteSpace(Type) &&
                   !string.IsNullOrWhiteSpace(Duration) &&
                   (!StrengthWorkout || !string.IsNullOrWhiteSpace(Repetitions)) && //endast krävs om styrketräning
                   !string.IsNullOrWhiteSpace(Notes);
        }

        private void ExecuteSave(object param)
        {
            DateTime workoutDate = DateTime.Parse(Date);
            TimeSpan workoutDuration = TimeSpan.Parse(Duration);
            int caloriesBurned = int.TryParse(CaloriesBurned, out int result) ? result : 0;

            Workout newWorkout;

            if (StrengthWorkout)  //om användaren har valt styrketräning
            {
                int repetitions = int.Parse(Repetitions);
                newWorkout = new StrengthWorkout(
                    workoutDate,
                    Type,
                    workoutDuration,
                    caloriesBurned,
                    Notes,
                    repetitions
                );
            }
            else  //om användaren har valt konditionsträning
            {
                int distance = int.Parse(Distance); //använd Distans-egenskapen för CardioWorkout
                newWorkout = new CardioWorkout(
                    workoutDate,
                    Type,
                    workoutDuration,
                    caloriesBurned,
                    Notes,
                    distance
                );
            }

            MessageBox.Show("New workout has been added");

            if (param is Window addWorkoutWindow)
            {
                addWorkoutWindow.Close();
            }
        }

        private void UpdateCaloriesBurned()
        {
            if (TimeSpan.TryParse(Duration, out TimeSpan workoutDuration) &&
                int.TryParse(Repetitions, out int repetitions))
            {
                if (StrengthWorkout)
                {
                    var tempWorkout = new StrengthWorkout(DateTime.Now, Type, workoutDuration, 0, Notes, repetitions);
                    CaloriesBurned = tempWorkout.CalculateCaloriesBurned().ToString();
                }
                else
                {
                    CaloriesBurned = "0"; // Exempel, du kan räkna om kalorier för cardio om det är implementerat
                }
            }
            else
            {
                CaloriesBurned = "0";
            }
        }
    }
}