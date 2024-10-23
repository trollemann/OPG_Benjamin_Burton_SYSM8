using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fit_Track.Model
{
    public class CardioWorkout : Workout
    {
        public int Distance { get; set; }

        public CardioWorkout(DateTime date, string type, TimeSpan duration, int caloriesBurned, string notes, int distance) : base(date, type, duration, caloriesBurned, notes)
        {
            Distance = distance;
        }

        public override int CalculateCaloriesBurned()
        {
            return (int)Duration.TotalMinutes * Distance;
        }
    }
}
