namespace Fit_Track.Model
{
    public class StrengthWorkout : Workout
    {
        //EGENSKAP
        public int Repetitions { get; set; }

        //KONSTRUKTOR
        public StrengthWorkout(string date, string type, int duration, int caloriesBurned, string notes, int repetitions) : base(date, type, duration, caloriesBurned, notes)
        {
            Repetitions = repetitions;
        }
    }
}