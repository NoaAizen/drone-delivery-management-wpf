using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BO
{
    /// <summary>
    /// ישות לוגית של רחפן
    /// </summary>
    public class Drone
    {
        /// <summary>
        /// מספר מזהה
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// מודל רחפן
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// קטגוריית משקל
        /// </summary>
        public WeightCategories MaxWeight { get; set; }
        /// <summary>
        /// מצב רחפן
        /// </summary>
        public StatusDrone Status { get; set; }
        /// <summary>
        /// מצב סוללה
        /// </summary>
        public double Battery { get; set; }
        /// <summary>
        /// חבילה בהעברה
        /// </summary>
        public ParcelInTransfer ParcelInTransfer { get; set; }
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


