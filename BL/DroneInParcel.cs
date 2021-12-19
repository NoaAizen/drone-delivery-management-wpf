using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    /// <summary>
    ///ישות לוגית רחפן בחבילה
    /// </summary>
    public class DroneInParcel
    {
        /// <summary>
        /// מספר מזהה
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// מצב סוללה
        /// </summary>
        public double Battery { get; set; }
        /// <summary>
        /// מיקום נוכחי
        /// </summary>
        public Location CurrentLocation { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}


