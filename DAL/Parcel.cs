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
              

                public Parcel(int id, int idSender, int idTarget, WeightCategories mAX_weight, Priorities priorities, int idDrone, DateTime requsted, DateTime schduled, DateTime pickedUp, DateTime delivered) 
                {
                    Id = id;
                    SenderId = idSender;
                    TargetId = idTarget;
                    Weight = mAX_weight;
                    Priority = priorities;
                    DroneId = 0;
                    Requested = requsted;
                    Scheduled = schduled;
                    PickedUp = pickedUp;
                    Delivered = delivered;
                }

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


                public override string ToString()
                {
                    return String.Format("Parcel- Id: {0}, SenderId: {1}, TargetId: {2}, Weight: {3}, Priority: {4}," +
                        " Requested: {5}, DroneId: {6}, Scheduled: {7}, PickedUp: {8}, Delivered:{9}"
                        , Id, SenderId, TargetId, Weight, Priority, Requested, DroneId, Scheduled, PickedUp, Delivered);
                }
            }
        }
    }
}
