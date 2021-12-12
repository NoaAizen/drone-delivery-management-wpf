using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{
    public struct Customer
    {

        /// <summary>
        /// בנאי 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name_Customers"></param>
        /// <param name="telephon"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        public Customer(int id, string name_Customers, string telephon, double longitude, double latitude) : this()
        {
            Id = id;
            Name = name_Customers;
            Phone = telephon;
            Longitude = longitude;
            Latitude = latitude;
        }
        /// <summary>
        /// שדות
        /// </summary>
        public int Id { get; set; }// מספר מזהה
        public string Name { get; set; }// שם לקוח
        public string Phone { get; set; }// מספר טלפון
        public double Latitude { get; set; } // קו רוחב
        public double Longitude { get; set; }// קו אורך
        /// <summary>
        ///                 /// פונקצית טוסטרניג(Tostring)- בשביל הדפסה
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("Customer- Id: {0}, Name: {1}, Longitude: {2}, Latitude: {3}, Phone: {4}"
                , Id, Name, Longitude, Latitude, Phone);
        }
    }
}


