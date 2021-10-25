using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace DalObject
    {
        public class DalObject
        {
            DalObject()
            {
                DataSource.Config.Initialize();
            }

            public static void AddStation (DAL.IDAL.DO.Station s)
            {
                DataSource.listStations.Add(s);
            }
            public static void AddDrone(DAL.IDAL.DO.Drone d)
            {
                DataSource.listDrones.Add( d);
            }
            public static void AddCustomer(DAL.IDAL.DO.Customer c)
            {
                DataSource.listCustomers.Add(c);
            }
            public static void AddParcel(DAL.IDAL.DO.Parcel p)
            {
                DataSource.Config.CounterForParcels++;
                DataSource.listParcels.Add(p);
            }


        }
    }
}
