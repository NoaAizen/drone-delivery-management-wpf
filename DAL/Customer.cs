using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace IDAL
    {
        namespace DO
        {
            public struct Customer
            {
                public int Id { get; set; }// מספר מזהה
                public string Name { get; set; }// שם לקוח
                public string Phone{ get; set; }// מספר טלפון
                public double Latitude { get; set; } // קו רוחב
                public double Longitude { get; set; }// קו אורך

                public override string ToString()
                {
                    return String.Format("Customer- Id: {0}, Name: {1}, Longitude: {2}, Latitude: {3}, Phone: {4}"
                        , Id, Name, Longitude, Latitude, Phone);
                }
            }
        }
    }
}
