using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace DalObject
    {
        class DataSource
        {
            internal static List<IDAL.DO.Drone> listDrones = new List<IDAL.DO.Drone>();
            internal static List<IDAL.DO.Station> listStations = new List<IDAL.DO.Station>();
            internal static List<IDAL.DO.Customer> listCustomers = new List<IDAL.DO.Customer>();
            internal static List<IDAL.DO.Parcel> listParcels = new List<IDAL.DO.Parcel>();
            
            public void InitializationDrone()
            {

            }
           
        }
    }
}
