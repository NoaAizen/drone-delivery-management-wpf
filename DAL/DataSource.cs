using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DalObject
{
    internal static class DataSource
    {/// <summary>
     /// הגדרת הרשימות
     /// </summary>
        internal static List<DO.Drone> listDrones = new List<DO.Drone>();
        internal static List<DO.Station> listStations = new List<DO.Station>();
        internal static List<DO.Customer> listCustomers = new List<DO.Customer>();
        internal static List<DO.Parcel> listParcels = new List<DO.Parcel>();
        internal static List<DO.DroneCharge> listDroneCharges = new List<DO.DroneCharge>();

        
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
                WeightCategories maxWeight;
                Priorities priority;
                DateTime? requsted, schduled, pickedUp, delivered;
                TimeSpan time = new TimeSpan(1, 0, 0);
                int id, chargeSlots, temp, senderId, targetId, droneId;
                double longitude, latitude; //battery;
                string model, name, phone;
                //אתחול של תחנות
                for (int i = 0; i < 2; i++)
                {
                    id = i;
                    temp = r.Next(1000, 10000);
                    name = "st" + temp;
                    chargeSlots = r.Next(1, 100);
                    longitude = r.NextDouble() * (180 + 180) - 180;//NextDouble() * (maximum - minimum) + minimum;
                    latitude = r.NextDouble() * (90 + 90) - 90;
                    listStations.Add( new() 
                    { Id = id, Name = name, AvailableStations = chargeSlots, Longitude = longitude, Latitude = latitude });
                }
                //אתחול של רחפנים 
                for (int i = 0; i < 10; i++)
                {
                    id = i+1;
                    temp = r.Next(1, 10000);
                    model = "FF" + temp;
                    maxWeight = (WeightCategories)r.Next(0, 3);
                    //status = (StatusDrone)r.Next(0, 3);
                    //battery = r.NextDouble() * (100) ;
                    listDrones.Add(new() { Id = id, Model = model, MaxWeight = maxWeight }); //, status, battery));
                }

                //אתחול של לוקחות
                for (int i = 0; i < 10; i++)
                {
                    id = i;
                    name = Arr[i];
                    temp = r.Next(1000000, 10000000);
                    phone = temp.ToString();
                    longitude = r.NextDouble() * (180 + 180) - 180;
                    latitude = r.NextDouble() * (90 + 90) - 90;
                    listCustomers.Add(new() 
                    { Id = id, Name = name, Phone = phone, Longitude = longitude, Latitude = latitude });
                }

                ///אתחול של חבילות במשלוח וסופקו
                for (int i = 0; i < 2; i++)
                {
                    id = i;
                    senderId = i;
                    targetId = i+1;
                    maxWeight = (WeightCategories)r.Next(0, 3);
                    priority = (Priorities)r.Next(0, 3);
                    droneId = i+1;
                    requsted = RandomDay();
                    schduled = requsted + time;
                    pickedUp = schduled + time;
                    delivered = pickedUp + time;
                    CounterForParcels++;
                    listParcels.Add(new()
                    {
                        Id = id,
                        SenderId = senderId,
                        TargetId = targetId,
                        Weight = maxWeight,
                        Priority = priority,
                        DroneId = droneId,
                        Requested = requsted,
                        Scheduled = schduled,
                        PickedUp = pickedUp,
                        Delivered = delivered
                    });
                }

                ///אתחול של חבילות במשלוח ולא סופקו
                for (int i = 2; i < 5; i++)
                {
                    id = i;
                    senderId = i;
                    targetId = i + 1;
                    maxWeight = (WeightCategories)r.Next(0, 3);
                    priority = (Priorities)r.Next(0, 3);
                    droneId = i + 1;
                    requsted = RandomDay();
                    schduled = requsted + time;
                    pickedUp = schduled + time;
                    delivered = null;
                    CounterForParcels++;
                    listParcels.Add(new()
                    {
                        Id = id,
                        SenderId = senderId,
                        TargetId = targetId,
                        Weight = maxWeight,
                        Priority = priority,
                        DroneId = droneId,
                        Requested = requsted,
                        Scheduled = schduled,
                        PickedUp = pickedUp,
                        Delivered = delivered
                    });
                }

                ///אתחול של חבילות לא במשלוח
                for (int i = 5; i < 10; i++)
                {
                    id = i;
                    senderId = 8;
                    targetId = 9;
                    maxWeight = (WeightCategories)r.Next(0, 3);
                    priority = (Priorities)r.Next(0, 3);
                    droneId = 0;
                    requsted = RandomDay();
                    schduled = null;
                    pickedUp = null;
                    delivered = null;
                    CounterForParcels++;
                    listParcels.Add(new()
                    {
                        Id = id,
                        SenderId = senderId,
                        TargetId = targetId,
                        Weight = maxWeight,
                        Priority = priority,
                        DroneId = droneId,
                        Requested = requsted,
                        Scheduled = schduled,
                        PickedUp = pickedUp,
                        Delivered = delivered
                    });
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

