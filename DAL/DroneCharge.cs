using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{

    public struct DroneCharge// טעינת סוללת רחפן 

    {
        /// <summary>
        /// בנאי
        /// </summary>
        /// <param name="idDrone"></param>
        /// <param name="idStation"></param>
        public DroneCharge(int idDrone, int idStation)
        {
            DroneId = idDrone;
            StationId = idStation;
        }
        /// <summary>
        /// שדטות
        /// </summary>
        public int DroneId { get; set; }// מזהה רחפן 
        public int StationId { get; set; }//  מזהה תחנת-בסיס 
        /// <summary>
        ///                 /// פונקצית טוסטרניג(Tostring)- בשביל הדפסה
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("DroneCharge- DroneId: {0}, StationId: {1}"
                , DroneId, StationId);
        }

    }
}

