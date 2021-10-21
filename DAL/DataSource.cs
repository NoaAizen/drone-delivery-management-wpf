using DAL.IDAL.DO;
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
                    string[] Arr = new string[10] { "noa", "avi", "oriya", "ori", "rachel","tamar","ben","gad","dan","moshe" };//מערך שמות של הלקוח
                    StatusDrone status;
                    WeightCategories MAX_weight;
                    int Id, Name, ChargeSlots, temp;
                    double longitude, latitude, battery;
                    string modle, Name_Customers,Telephon;
                    for (int i = 0; i < 2; i++)
                    {
                        Id = i;
                        Name = r.Next(1, 10000);
                        ChargeSlots = r.Next(1, 100);
                        longitude = r.Next(-180, 180);
                        latitude = r.Next(-90, 90);
                        listStations.Add(new IDAL.DO.Station(Id, Name, ChargeSlots, longitude, latitude));
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        Id = i;
                        temp = r.Next(1, 10000);
                        modle = "FF" + temp;
                        MAX_weight = (WeightCategories)r.Next(0, 3);
                        status = (StatusDrone)r.Next(0, 3);
                        battery = r.Next(0, 101);
                        listDrones.Add(new IDAL.DO.Drone(Id, modle, MAX_weight, status, battery));
                    }


                    for (int i = 0; i < 10; i++)
                    {
                        Id = i;
                        Name_Customers = Arr[i];
                        temp = r.Next(1000000, 10000001);
                        Telephon = temp.ToString();
                        longitude = r.Next(-180, 180);
                        latitude = r.Next(-90, 90);
                        listCustomers.Add(new IDAL.DO.Customer(Id, Name_Customers, Telephon, longitude, latitude));
                    }

                }


            }
        }
    }
}
