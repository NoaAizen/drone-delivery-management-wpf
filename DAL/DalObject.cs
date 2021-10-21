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

            public static void AddStation (int Id, int Name, int ChargeSlots, double longitude, double latitude)
            {
                DataSource.listStations.Add(new IDAL.DO.Station(Id, Name, ChargeSlots, longitude, latitude));
            }
            public static void AddDrone(int Id, string Model, IDAL.DO.WeightCategories MAX_weight,IDAL.DO.StatusDrone status, double battery)
            {
                DataSource.listDrones.Add(new IDAL.DO.Drone(Id, Model, MAX_weight, status, battery));
            }
            public static void AddCustomer(int Id, string Name_Customers, string Telephon, double longitude, double latitude)
            {
                DataSource.listCustomers.Add(new IDAL.DO.Customer(Id, Name_Customers, Telephon, longitude, latitude));
            }
            public static void AddParcel(int Id, int IdSender, int IdTarget, IDAL.DO.WeightCategories MAX_weight,
               IDAL.DO.Priorities priorities, int IdDrone, DateTime Requsted, DateTime Schduled, DateTime PickedUp, DateTime Delivered)
            {
                DataSource.Config.CounterForParcels++;
                DataSource.listParcels.Add(new IDAL.DO.Parcel(Id, IdSender, IdTarget, MAX_weight, priorities, IdDrone, Requsted, Schduled, PickedUp, Delivered));
            }


        }
    }
}
