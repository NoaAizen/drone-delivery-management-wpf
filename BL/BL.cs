using System;
using BL.IBL;
using BL.IBL.BO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DalObject;
using DAL.IDAL;

namespace BL
{
    class BL : IBL.IBL
    {
        public List<DroneToList> DronesList = new List<DroneToList>();//רשימת רחפנים

        BL()
        {
            IDal DalObj = new DalObject();
            List<Drone> DronesListtemp = new List<Drone>();

            //DronesListtemp = listDrones;
        }
        public Station AddStation(int id, int name, Location location, int AvailableStations)
        {
            Station s = new Station();
            s.Id = id;
            s.Name = name;
            s.Location = location;
            s.AvailableStations = AvailableStations;
            s.DroneInChargingsList = null;
            return s;
        }
    }
}
