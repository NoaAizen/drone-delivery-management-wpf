using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{
    /// <summary>
    /// מידע על הרחפן
    /// </summary>
    public struct Drone
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
        /// פונקצית טוסטרניג(Tostring)- בשביל הדפסה
        /// </summary>
        /// <returns>מחרוזת עם שדות הרחפן</returns>
        public override string ToString()
        {
            return String.Format("Drone- Id: {0}, Model: {1}, Max Weight: {2}"
                , Id, Model, MaxWeight);
        }
    }
}


