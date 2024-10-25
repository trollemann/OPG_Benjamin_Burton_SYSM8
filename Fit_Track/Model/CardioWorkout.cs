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

        public CardioWorkout(string date, string type, int duration, int caloriesBurned, string notes, int distance) : base(date, type, duration, caloriesBurned, notes)
        {
            Distance = distance;
        }

        public override int CalculateCaloriesBurned()
        {
            return Duration * Distance;
        }
    }
}
