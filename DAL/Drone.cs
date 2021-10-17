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
            public struct Drone
            {
                public int Id { get; set; }// מספר מזהה
                public string Moodle { get; set; }// מודל רחפן
                public WeightCategories MaxWeight { get; set; }// קטגוריית משקל
                public StatusDrone Status  { get; set; }// מצב רחפן
                public double Battery { get; set; } // מצב סוללה


                public override string ToString()
                {
                    return String.Format("Drone- Id: {0}, Moodle: {1}, Max Weight: {2}, Status: {3}, Battery: {4}"
                        , Id, Moodle, MaxWeight, Status, Battery);
                }
            }
        }
    }
}
