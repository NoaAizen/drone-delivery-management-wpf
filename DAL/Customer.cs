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


            }
        }
    }
}
