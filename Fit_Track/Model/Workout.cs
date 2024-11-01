namespace Fit_Track.Model
{
    public abstract class Workout
    {
        //EGENSKAPER
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public TimeSpan Duration { get; set; }
        public int CaloriesBurned { get; set; }
        public string Notes { get; set; }

        //KONSTRUKTOR
        public Workout(DateTime date, string type, TimeSpan duration, int caloriesBurned, string notes)
        {
            Date = date.Date;
            Type = type;
            Duration = duration;
            CaloriesBurned = caloriesBurned;
            Notes = notes;
        }
    }
}