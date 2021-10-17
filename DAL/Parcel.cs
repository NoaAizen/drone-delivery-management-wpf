using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace IDAL
    {
        namespace DO
        {
            public struct Parcel
            {
                public int Id { get; set; }// מספר מזהה חבילה
                public int SenderId { get; set; }// מזהה לוקח שולח
                public int TargetId { get; set; }// מזהה לוקח מקבל
                public WeightCategories Weight{ get; set; }// קטגורית משקל
                public Priorities Priority { get; set; } // עדיפות
                public DateTime Requested { get; set; } // זמן יצירת חבילה למשלוח 
                public int DroneId  { get; set; } // מזהה רחפן מבצע
                public DateTime Scheduled { get; set; } // זמן שיוך החבילה לרחפן 
                public DateTime PickedUp { get; set; } // זמן איסוף חבילה מהשולח 
                public DateTime Delivered { get; set; } // זמן הגעת החבילה למקבל 



            }
        }
    }
}
