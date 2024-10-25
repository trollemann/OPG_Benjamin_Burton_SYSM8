namespace Fit_Track.Model
{
    public abstract class Workout
    {
        //egenskaper
        public string Date { get; set; }
        public string Type { get; set; }
        public int Duration { get; set; }
        public int CaloriesBurned { get; set; }
        public string Notes { get; set; }

        //konstruktor
        public Workout(string date, string type, int duration, int caloriesBurned, string notes)
        {
            Date = date;
            Type = type;
            Duration = duration;
            CaloriesBurned = caloriesBurned;
            Notes = notes;
        }

        //abstrakt metod
        public abstract int CalculateCaloriesBurned();
    }
}