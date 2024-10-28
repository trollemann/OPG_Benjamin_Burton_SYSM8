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
        //EGENSKAP
        public int Distance { get; set; }

        //KONSTRUKTOR
        public CardioWorkout(string date, string type, int duration, int caloriesBurned, string notes, int distance) : base(date, type, duration, caloriesBurned, notes)
        {
            //kollar så distans inte är negativt
            if (distance < 0)
            {
                throw new ArgumentException("Distance cannot be negative.", nameof(distance));
            }

            Distance = distance;
        }

        //METOD
        //overriding
        public override int CalculateCaloriesBurned()
        {
            return Duration * Distance;
        }
    }
}