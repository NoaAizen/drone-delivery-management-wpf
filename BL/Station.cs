using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    namespace IBL
    {
        namespace BO
        {
            class Station
            {
            
                public int Id { get; set; }// מספר מזהה
                public int Name { get; set; }// שם תחנה
                public int AvailableStations { get; set; }// מספר עמודות הטענה
                public Location Location { get; set; }//מיקום
                public List<DroneInCharging> DroneInChargingsList { get; set; }//רשימת רחפנים בטעינה
            }
        }
    }
}