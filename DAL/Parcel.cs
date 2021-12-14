using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{
    /// <summary>
    /// מידע על החבילה
    /// </summary>
    public struct Parcel
    {
        /// <summary>
        /// מספר מזהה חבילה
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// מזהה לקוח שולח
        /// </summary>
        public int SenderId { get; set; }
        /// <summary>
        /// מזהה לקוח מקבל
        /// </summary>
        public int TargetId { get; set; }
        /// <summary>
        /// קטגורית משקל
        /// </summary>
        public WeightCategories Weight { get; set; }
        /// <summary>
        /// עדיפות
        /// </summary>
        public Priorities Priority { get; set; }
        /// <summary>
        /// זמן יצירת חבילה למשלוח
        /// </summary>
        public DateTime? Requested { get; set; }
        /// <summary>
        /// מזהה רחפן מבצע
        /// </summary>
        public int DroneId { get; set; }
        /// <summary>
        /// זמן שיוך החבילה לרחפן
        /// </summary>
        public DateTime? Scheduled { get; set; }
        /// <summary>
        /// זמן איסוף חבילה מהשולח
        /// </summary>
        public DateTime? PickedUp { get; set; }
        /// <summary>
        /// זמן הגעת החבילה למקבל
        /// </summary>
        public DateTime? Delivered { get; set; }   

        /// <summary>
        /// פונקצית טוסטרניג(Tostring)- בשביל הדפסה
        /// </summary>
        /// <returns>מחרוזת עם שדות החבילה</returns>
        public override string ToString()
        {
            return String.Format("Parcel- Id: {0}, SenderId: {1}, TargetId: {2}, Weight: {3}, Priority: {4}," +
                " Requested: {5}, DroneId: {6}, Scheduled: {7}, PickedUp: {8}, Delivered:{9}"
                , Id, SenderId, TargetId, Weight, Priority, Requested, DroneId, Scheduled, PickedUp, Delivered);
        }
    }
}


