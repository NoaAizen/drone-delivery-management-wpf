using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;





namespace BO
{
    public class Parcel
    {
        public int Id { get; set; }// מספר מזהה חבילה
        public CustomerInParcel CustomerInParcelSender { get; set; }//לקוח בחבילה- השולח
        public CustomerInParcel CustomerInParcelRecipient { get; set; }//לקוח בחבילה -המקבל
        public WeightCategories Weight { get; set; }// קטגורית משקל
        public Priorities Priority { get; set; } // עדיפות
        public DroneInParcel DroneInParcel { get; set; }// רחפן בחבילה
        public DateTime? Requested { get; set; } // זמן יצירת חבילה למשלוח 
        public DateTime? Scheduled { get; set; } // זמן שיוך החבילה לרחפן 
        public DateTime? PickedUp { get; set; } // זמן איסוף חבילה מהשולח 
        public DateTime? Delivered { get; set; } // זמן הגעת החבילה למקבל

        public override string ToString()
        {
            return this.ToStringProperty();
        }

    }
}

