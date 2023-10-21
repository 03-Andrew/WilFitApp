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
        public string date {  get; set; }
        public string time { get; set; }
        public string workoutType { get; set; }
        public string workoutCategory { get; set;}
        public string duration { get; set;}
        public string description { get; set;}

    }
}
