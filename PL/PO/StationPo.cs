using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PL
{
    public class StationPo : INotifyPropertyChanged//(מחלקת עזר להזרמת מידע (נלמד בשיעור
    {
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Id"));
                }
            }
        }
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }
        private int availableStations;
        public int AvailableStations
        {
            get { return availableStations; }
            set
            {
                availableStations = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AvailableStations"));
                }
            }
        }
        private double longitude;
        public double Longitude
        {
            get { return longitude; }
            set
            {
                longitude = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Longitude"));
                }
            }
        }
        private double latitude;
        public double Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Latitude"));
                }
            }
        }
        private List<BO.DroneInCharging> droneInChargingsList;
        public List<BO.DroneInCharging> DroneInChargingsList
        {
            get { return droneInChargingsList; }
            set
            {
                droneInChargingsList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("DroneInChargingsList"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    
}
