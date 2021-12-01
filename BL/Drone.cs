using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace IBL
{
    namespace BO
    {
        public class Drone
        {
            public int Id { get; set; }// מספר מזהה
            public string Model { get; set; }// מודל רחפן
            public WeightCategories MaxWeight { get; set; }// קטגוריית משקל
            public StatusDrone Status { get; set; }// מצב רחפן
            public double Battery { get; set; } // מצב סוללה
            public ParcelInTransfer ParcelInTransfer { get; set; }//חבילה בהעברה
            public Location CurrentLocation { get; set; }//מיקום נוכחי
        }
    }
}

