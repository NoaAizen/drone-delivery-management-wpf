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
            internal static List<IDAL.DO.Drone> listDrones = new List<IDAL.DO.Drone>(10);
            internal static List<IDAL.DO.Station> listStations = new List<IDAL.DO.Station>(5);
            internal static List<IDAL.DO.Customer> listCustomers = new List<IDAL.DO.Customer>(100);
            internal static List<IDAL.DO.Parcel> listParcels = new List<IDAL.DO.Parcel>(1000);

            internal class Config
            {
                public static int CounterForParcels { get; set; }// מספר רץ עבור חבילות
                public static Random r = new Random();

                public static void Initialize()
                {
                    for(int i=0; i<2; i++)
                    {
                        int IdStation = i;
                        int NameStation = r.Next(1, 10000);
                        int ChargeSlots = r.Next(1, 100);
                    }

                }

            }
        }
    }
}
