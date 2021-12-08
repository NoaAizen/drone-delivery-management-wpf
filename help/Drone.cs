using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace help
{
    
    
        public class DroneToList//רחפן ברשימה
        {
            public int Id { get; set; }// מספר מזהה
            public string Model { get; set; }// מודל רחפן
            public WeightCategories MaxWeight { get; set; }// קטגוריית משקל
            public StatusDrone Status { get; set; }// מצב רחפן
            public double Battery { get; set; } // מצב סוללה
            public ParcelInTransfer ParcelInTransfer { get; set; }//חבילה בהעברה
            public Location CurrentLocation { get; set; }//מיקום נוכחי
            public int ParcelTransferredNumber { get; set; }// מספר חבילה מועברת (אם יש

            public override string ToString()
            {
                return this.ToStringProperty();
            }
        }
    }

