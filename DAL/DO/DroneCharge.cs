using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{
    /// <summary>
    /// מידע על רחפן בטעינה - טעינת סוללת רחפן
    /// </summary>
    public struct DroneCharge 
    {
        /// <summary>
        /// מזהה רחפן
        /// </summary>
        public int DroneId { get; set; }
        /// <summary>
        /// מזהה תחנת-בסיס
        /// </summary>
        public int StationId { get; set; }
        
        /// <summary>
        ///פונקצית טוסטרניג(Tostring)- בשביל הדפסה
        /// </summary>
        /// <returns>מחרוזת עם שדות הרחפן בטעינה</returns>
        public override string ToString()
        {
            return String.Format("DroneCharge- DroneId: {0}, StationId: {1}"
                , DroneId, StationId);
        }

    }
}

