using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{
    /// <summary>
    /// מידע על הלקוח
    /// </summary>
    public struct Customer
    {
        /// <summary>
        /// מספר מזהה
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// שם לקוח
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// מספר טלפון
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// קו רוחב
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// קו אורך
        /// </summary>
        public double Longitude { get; set; }
        
        /// <summary>
        /// פונקצית טוסטרניג(Tostring)- בשביל הדפסה
        /// </summary>
        /// <returns>מחרוזת עם שדות הלקוח</returns>
        public override string ToString()
        {
            return String.Format("Customer- Id: {0}, Name: {1}, Longitude: {2}, Latitude: {3}, Phone: {4}"
                , Id, Name, Longitude, Latitude, Phone);
        }
    }
}


