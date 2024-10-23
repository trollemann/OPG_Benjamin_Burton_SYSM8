using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit_Track.Model
{
    public class StrengthWorkout : Workout
    {
        public int Repetitions { get; set; }

        public StrengthWorkout(DateTime date, string type, TimeSpan duration, int caloriesBurned, string notes, int repetitions) : base(date, type, duration, caloriesBurned, notes)
        {
            Repetitions = repetitions;
        }

        public override int CalculateCaloriesBurned()
        {
            return (int)Duration.TotalMinutes * Repetitions;
        }
    }
}