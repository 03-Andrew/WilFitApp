using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WilFitApp.MVVM.Model
{
    public class WorkoutLog
    {
        public string workoutID { get; set; }
        public string Date {  get; set; }
        public string Time { get; set; }
        public string workoutType { get; set; }
        public string workoutCategory { get; set;}
        public string Duration { get; set;}
        public string Description { get; set;}

    }
}
