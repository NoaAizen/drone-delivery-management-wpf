using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace BO
{
    public class Station
    {
        public int Id { get; set; }// מספר מזהה
        public string Name { get; set; }// שם תחנה
        public int AvailableStations { get; set; }// מספר עמודות הטענה
        public Location Location { get; set; }//מיקום
        public List<DroneInCharging> DroneInChargingsList { get; set; }//רשימת רחפנים בטעינה
                                                                       //public override string ToString()
                                                                       //{
                                                                       //    return String.Format("Station- Id: {0}, Name: {1}, AvailableStations: {2}, Location: {3}"
                                                                       //        , Id, Name, AvailableStations, Location);
                                                                       //}

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}

