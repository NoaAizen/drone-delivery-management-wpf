using System;

namespace DAL
{
    namespace IDAL
    {
        namespace DO
        {
            public struct Station
            {
                public int Id { get; set; }// מספר מזהה
                public int Name { get; set; }// שם תחנה
                public double Longitude { get; set; }// קו אורך
                public double Latitude { get; set; } // קו רוחב
                public int AvailableStations { get; set; }// מספר עמודות הטענה 

                public override string ToString()
                {
                    return String.Format("Station- Id: {0}, Name: {1}, Longitude: {2}, Latitude: {3}, AvailableStations: {4}"
                        , Id, Name, Longitude, Latitude, AvailableStations); 
                }
            }
        }
    }
}
