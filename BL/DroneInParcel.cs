using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    namespace IBL
    {
        namespace BO
        {
            class DroneInParcel//רחפן בחבילה
            {
                public int Id { get; set; }// מספר מזהה
                public double Battery { get; set; } // מצב סוללה
                public Location CurrentLocation { get; set; }//מיקום נוכחי
            }
        }

    }
}