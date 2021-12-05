using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DalObject
{
    internal class DataSource
    {/// <summary>
     /// הגדרת הרשימות
     /// </summary>
        internal static List<IDAL.DO.Drone> listDrones = new List<IDAL.DO.Drone>();
        internal static List<IDAL.DO.Station> listStations = new List<IDAL.DO.Station>();
        internal static List<IDAL.DO.Customer> listCustomers = new List<IDAL.DO.Customer>();
        internal static List<IDAL.DO.Parcel> listParcels = new List<IDAL.DO.Parcel>();
        internal static List<IDAL.DO.DroneCharge> listDroneCharges = new List<IDAL.DO.DroneCharge>();


        internal class Config
        {
            //שדות
            public static int CounterForParcels { get; set; }// מספר רץ עבור חבילות
            //תכונות סטטיות עבור צריכת חשמל לק"מ ע"י רחפן
            internal static double available = 2;//פנוי
            internal static double lightWeight = 5;//נושא משקל קל
            internal static double mediumWeight  = 7;//נושא משקל בינוני
            internal static double heavyWeight = 10;//נושא משקל כבד
            internal static double chargingRate = 30;//קצב טעינת רחפן - % בשעה
            public static Random r = new Random();
            /// <summary>
            /// פונקציית אתחול נתונים
            /// </summary>
            public static void Initialize()
            {
                //משתני עזר
                string[] Arr = new string[10] { "noa", "avi", "oriya", "ori", "rachel", "tamar", "ben", "gad", "dan", "moshe" };//מערך שמות של הלקוח
                                                                                                                                //  StatusDrone status;
                WeightCategories MAX_weight;
                Priorities priorities;
                DateTime Requsted, Schduled, PickedUp, Delivered;
                int Id, ChargeSlots, temp, IdSender, IdTarget, IdDrone;
                double longitude, latitude; //battery;
                string modle, Name, Name_Customers, Telephon;
                //אתחול של תחנות
                for (int i = 0; i < 2; i++)
                {
                    Id = i;
                    temp = r.Next(1000, 10000);
                    Name = "st" + temp;
                    ChargeSlots = r.Next(1, 100);
                    longitude = r.NextDouble() * (180 + 180) - 180;//NextDouble() * (maximum - minimum) + minimum;
                    latitude = r.NextDouble() * (90 + 90) - 90;
                    listStations.Add(new IDAL.DO.Station(Id, Name, ChargeSlots, longitude, latitude));
                }
                //אתחול של רחפנים 
                for (int i = 0; i < 10; i++)
                {
                    Id = i+1;
                    temp = r.Next(1, 10000);
                    modle = "FF" + temp;
                    MAX_weight = (WeightCategories)r.Next(0, 3);
                    //status = (StatusDrone)r.Next(0, 3);
                    //battery = r.NextDouble() * (100) ;
                    listDrones.Add(new IDAL.DO.Drone(Id, modle, MAX_weight)); //, status, battery));
                }

                //אתחול של לוקחות
                for (int i = 0; i < 10; i++)
                {
                    Id = i;
                    Name_Customers = Arr[i];
                    temp = r.Next(1000000, 10000000);
                    Telephon = temp.ToString();
                    longitude = r.NextDouble() * (180 + 180) - 180;
                    latitude = r.NextDouble() * (90 + 90) - 90;
                    listCustomers.Add(new IDAL.DO.Customer(Id, Name_Customers, Telephon, longitude, latitude));
                }

                ///אתחול של חבילות במשלוח וסופקו
                for (int i = 0; i < 2; i++)
                {
                    Id = i;
                    IdSender = i;
                    IdTarget = i+1;
                    MAX_weight = (WeightCategories)r.Next(0, 3);
                    priorities = (Priorities)r.Next(0, 3);
                    IdDrone = i+1;
                    Requsted = RandomDay();
                    Schduled = Requsted.AddHours(1);
                    PickedUp = Schduled.AddHours(1);
                    Delivered = PickedUp.AddHours(1);
                    CounterForParcels++;
                    listParcels.Add(new IDAL.DO.Parcel(Id, IdSender, IdTarget, MAX_weight, priorities, IdDrone, Requsted, Schduled, PickedUp, Delivered));
                }

                ///אתחול של חבילות במשלוח ולא סופקו
                for (int i = 2; i < 5; i++)
                {
                    Id = i;
                    IdSender = i;
                    IdTarget = i + 1;
                    MAX_weight = (WeightCategories)r.Next(0, 3);
                    priorities = (Priorities)r.Next(0, 3);
                    IdDrone = i + 1;
                    Requsted = RandomDay();
                    Schduled = Requsted.AddHours(1);
                    PickedUp = Schduled.AddHours(1);
                    Delivered = DateTime.MinValue;
                    CounterForParcels++;
                    listParcels.Add(new IDAL.DO.Parcel(Id, IdSender, IdTarget, MAX_weight, priorities, IdDrone, Requsted, Schduled, PickedUp, Delivered));
                }

                ///אתחול של חבילות לא במשלוח
                for (int i = 5; i < 10; i++)
                {
                    Id = i;
                    IdSender = 8;
                    IdTarget = 9;
                    MAX_weight = (WeightCategories)r.Next(0, 3);
                    priorities = (Priorities)r.Next(0, 3);
                    IdDrone = 0;
                    Requsted = RandomDay();
                    Schduled = DateTime.MinValue;
                    PickedUp = DateTime.MinValue;
                    Delivered = DateTime.MinValue;
                    CounterForParcels++;
                    listParcels.Add(new IDAL.DO.Parcel(Id, IdSender, IdTarget, MAX_weight, priorities, IdDrone, Requsted, Schduled, PickedUp, Delivered));
                }

            }
            /// <summary>
            /// פונקצית עזר לחישוב של זמנים 
            /// </summary>
            /// <returns></returns>
            public static DateTime RandomDay()
            {
                DateTime start = new DateTime(2010, 1, 1, r.Next(8, 18), r.Next(0, 60), 0);
                int range = (DateTime.Today - start).Days;
                return start.AddDays(r.Next(range));
            }
        }
    }
}

