namespace Fit_Track.Model
{
    public class CardioWorkout : Workout
    {
        public int Distance { get; set; }

        public CardioWorkout(DateTime date, string type, TimeSpan duration, int caloriesBurned, string notes, int distance)
            : base(date, type, duration, caloriesBurned, notes)
        {
            Distance = distance;
        }
    }
}