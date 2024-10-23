using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit_Track.Model
{
    public abstract class Workout
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public TimeSpan Duration { get; set; }
        public int CaloriesBurned { get; set; }
        public string Notes { get; set; }

        public Workout(DateTime date, string type, TimeSpan duration, int caloriesBurned, string notes)
        {
            Date = date;
            Type = type;
            Duration = duration;
            CaloriesBurned = caloriesBurned;
            Notes = notes;
        }

        public abstract int CalculateCaloriesBurned();
    }
}