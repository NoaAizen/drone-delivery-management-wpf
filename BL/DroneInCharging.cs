using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;





namespace BO
{
    /// <summary>
    /// ישות לוגית רחפן בטעינה
    /// </summary>
    public class DroneInCharging
    {
        /// <summary>
        /// מספר מזהה
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// מצב סוללה
        /// </summary>
        public double Battery { get; set; } 
    }
}

