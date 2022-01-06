using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace BO
{
    /// <summary>
    /// ישות לוגית של תחנה
    /// </summary>
    public class Station
    {
        /// <summary>
        /// מספר מזהה
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// שם תחנה
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// מספר עמודות הטענה
        /// </summary>
        public int AvailableStations { get; set; }
        /// <summary>
        /// מיקום
        /// </summary>
        public Location Location { get; set; }
        /// <summary>
        /// רשימת רחפנים בטעינה
        /// </summary>
        public List<DroneInCharging> DroneInChargingsList { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}

