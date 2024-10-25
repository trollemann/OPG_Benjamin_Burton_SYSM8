namespace Fit_Track.Model
{
    public class StrengthWorkout : Workout
    {
        //egenskap
        public int Repetitions { get; set; }

        //konstruktor
        public StrengthWorkout(string date, string type, int duration, int caloriesBurned, string notes, int repetitions) : base(date, type, duration, caloriesBurned, notes)
        {
            Repetitions = repetitions;
        }

        //override metod
        public override int CalculateCaloriesBurned()
        {
            return Duration * Repetitions;
        }
    }
}