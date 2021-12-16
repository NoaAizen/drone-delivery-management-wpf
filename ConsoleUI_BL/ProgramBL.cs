using System;
using System.Collections.Generic;

namespace ConsoleUI_BL
{
    //הגדרה של enum 
    public enum Choice { ADD = 1, UPDATE, VIEW, VIEWLIST, EXIT };
    public enum Add { STATION = 1, DRONE, CUSTOMER, PARCEL };
    public enum Update { DRONE = 1, STATION, CUSTOMERS, CHARGING, RELEASE, ASSIGN, COLLECTION, DELIVERY };
    public enum View { STATION = 1, DRONE, CUSTOMER, PARCEL };
    public enum ViewList { STATIONS = 1, DRONES, CUSTOMERS, PARCELS, NODRONE, AVAILABLE, DRONECHARGES };

    class ProgramBL
    {
        //public static DAL.DalObject.DalObject D = new DAL.DalObject.DalObject();//קריאה לבנאי שמתאחל
        static DalApi.IDal D = DalApi.DalFactory.GetDal("1");//קריאה לבנאי שמתאחל
        static BlApi.IBL bl = BlApi.BlFactory.GetBl();//קריאה לבנאי שמתאחל
        static void Main(string[] args)
        {
            try
            {
                Choice choice = 0;
                Add add;
                Update update;
                View view;
                ViewList viewList;
                while (choice != (Choice)5)
                {
                    Console.WriteLine(@"What action would you like to take? 
Enter your choice: 
1: for add
2: for update
3: for view
4: for list view
5: for exit");
                    choice = (Choice)int.Parse(Console.ReadLine());
                    switch (choice)
                    {
                        case Choice.ADD:
                            Console.WriteLine(@"
What addition would you like to make? 
Enter your selection:
1: for add a station
2: for add a drone
3: for add a customer
4: for add a parcel");
                            add = (Add)int.Parse(Console.ReadLine());
                            switch (add) //אופציות של הוספה
                            {
                                case Add.STATION://הוספת תחנה
                                    AddStationData();
                                    break;
                                case Add.DRONE://הוספת רחפן
                                    AddDroneData();
                                    break;
                                case Add.CUSTOMER://הוספת לקוח
                                    AddCustomerData();
                                    break;
                                case Add.PARCEL://הוספת חבילה
                                    AddParcelData();
                                    break;
                                default:
                                    Console.WriteLine("Enter a number between 1 to 4");
                                    break;
                            };
                            break;

                        case Choice.UPDATE:
                            Console.WriteLine(@"
What update would you like to make? 
Enter your selection:
1: for update a drone model
2: for update a station
3: for update a customer
4: for sending a drone for charging
5: for release a drone from charging
6: for assign a parcel to a drone
7: for collection a parcel by a drone
8: for delivery a parcel by a drone");
                            update = (Update)int.Parse(Console.ReadLine());
                            switch (update)//עדכון
                            {
                                //{ DRONE=1, STATION, CUSTOMERS, CHARGING, RELEASE, ASSING , COLLECTION, DELIVERY };
                                case Update.DRONE://עדכון מודל רחפן 
                                    UpdateDroneModelData();
                                    break;
                                case Update.STATION://עדכון תחנה 
                                    UpdateStationData();
                                    break;
                                case Update.CUSTOMERS://עדכון לקוח 
                                    UpdateCustomerData();
                                    break;
                                case Update.CHARGING:// שליחת רחפן לטעינה בתחנת -בסיס 
                                    SendingDroneForChargingData();
                                    break;
                                case Update.RELEASE:// שחרור רחפן מטעינה בתחנת -בסיס 
                                    ReleaseDroneFromChargingData();
                                    break;
                                case Update.ASSIGN://שיוך חבילה לרחפן 
                                                   //UpdateDroneToParcelData();
                                    break;
                                case Update.COLLECTION:// איסוף חבילה ע"י רחפן 
                                    CollectionParcelFromDroneData();
                                    break;
                                case Update.DELIVERY:// אספקת חבילה ע"י רחפן 
                                    DeliveryParcelForCustomerData();
                                    break;
                                default:
                                    Console.WriteLine("Enter a number between 1 to 8");
                                    break;
                            };
                            break;

                        case Choice.VIEW:
                            Console.WriteLine(@"
Which view would you like? 
Enter your selection:
1: for view a station
2: for view a drone
3: for view a customer
4: for view a parcel");
                            view = (View)int.Parse(Console.ReadLine());
                            switch (view)//הדפסה של רשומה אחת
                            {
                                case View.STATION:///\ תצוגת תחנת-בסיס 
                                    ViewStationPrint();
                                    break;
                                case View.DRONE:// תצוגת רחפן 
                                    ViewDronePrint();
                                    break;
                                case View.CUSTOMER:// תצוגת לקוח 
                                    ViewCustomerPrint();
                                    break;
                                case View.PARCEL:// תצוגת חבילה
                                    ViewParcelPrint();
                                    break;
                                default:
                                    Console.WriteLine("Enter a number between 1 to 4");
                                    break;
                            };
                            break;

                        case Choice.VIEWLIST:
                            Console.WriteLine(@"
Which view of list would you like? 
Enter your selection:
1: for view the stations list
2: for view the drones list
3: for view the customers list
4: for view the parcels list
5: for view parcels that have not yet been assigned to a drone
6: for view stations with available charging stations");
                            viewList = (ViewList)int.Parse(Console.ReadLine());
                            switch (viewList)//הדפסת כל הרשימות
                            {
                                case ViewList.STATIONS://הצגת רשימת תחנות-בסיס 
                                    ViewStationListPrint();
                                    break;
                                case ViewList.DRONES:// הצגת רשימת הרחפנים 
                                    ViewDroneListPrint();
                                    break;
                                case ViewList.CUSTOMERS:// הצגת רשימת הלקוחות 
                                    ViewCustomerListPrint();
                                    break;
                                case ViewList.PARCELS:// הצגת רשימת החבילות 
                                    ViewParcelListPrint();
                                    break;
                                case ViewList.NODRONE:// הצגת רשימת חבילות שעוד לא שויכו לרחפן 
                                    ViewParcelNoDronelListPrint();
                                    break;
                                case ViewList.AVAILABLE://הצגת תחנות-בסיס עם עמדות טעינה פנויות 
                                    ViewAvailableChargingStationslListPrint();
                                    break;
                                case ViewList.DRONECHARGES://הצגת תחנות-בסיס עם עמדות טעינה פנויות 
                                    GetDroneChargesListPrint();
                                    break;
                                default:
                                    Console.WriteLine("Enter a number between 1 to 6");
                                    break;
                            };
                            break;

                        case Choice.EXIT:
                            break;

                        default:
                            Console.WriteLine("Enter a number between 1 to 5");
                            break;
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        //קליטת והדפסת נותנים        
        static int temp;
        /// <summary>
        /// קליטה נתונים של תחנה
        /// </summary>
        public static void AddStationData()
        {
            Console.WriteLine("Enter station's Id:");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter station's name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter Number of charging stations available:");
            int chargeSlots = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter station's longitude:");
            double longitude = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter station's latitude:");
            double latitude = double.Parse(Console.ReadLine());
            BO.Location location = new() { Longitude = longitude, Latitude = latitude };
            BO.Station s = new()
            { Id = id, Name = name, AvailableStations = chargeSlots, Location = location, DroneInChargingsList = null };
            bl.AddStation(s);
        }
        /// <summary>
        /// קליטת נתונים של רחפן
        /// </summary>
        public static void AddDroneData()
        {
            Console.WriteLine("Enter drone's Id:");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter drone's model:");
            string model = Console.ReadLine();
            Console.WriteLine("Enter drone's weight categories(Light = 0, Intermediate = 1, Heavy = 2):");
            temp = int.Parse(Console.ReadLine());
            BO.WeightCategories maxWeight = (BO.WeightCategories)temp;
            Console.WriteLine("Enter charging station's number:");
            int stationNumber = int.Parse(Console.ReadLine());
            BO.DroneToList d = new() { Id = id, Model = model, MaxWeight = maxWeight };
            bl.AddDrone(d, stationNumber);
        }
        /// <summary>
        /// קליטת נתונים של לקוח
        /// </summary>
        public static void AddCustomerData()
        {
            Console.WriteLine("Enter customer's Id:");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter customer's name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter customer's phone number:");
            string telephon = Console.ReadLine();
            Console.WriteLine("Enter customer's longitude:");
            double longitude = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter customer's latitude:");
            double latitude = double.Parse(Console.ReadLine());
            BO.Location location = new() { Longitude = longitude, Latitude = latitude };
            BO.Customer c = new() { Id = id, Name = name, Phone = telephon, Location = location };
            bl.AddCustomer(c);
        }
        /// <summary>
        /// קליטת נתונים של חבילה
        /// </summary>
        public static void AddParcelData()
        {
            Console.WriteLine("Enter Id of sender:");
            int IdSender = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Id of target:");
            int IdTarget = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Parcel's weight categories(Light = 0, Intermediate = 1, Heavy = 2):");
            temp = int.Parse(Console.ReadLine());
            BO.WeightCategories maxWeight = (BO.WeightCategories)temp;
            Console.WriteLine("Enter Parcel's priority(Normal = 0, Fast = 1, Emergency = 2):");
            temp = int.Parse(Console.ReadLine());
            BO.Priorities priorities = (BO.Priorities)temp;
            BO.CustomerInParcel sender = new() { Id = IdSender };
            BO.CustomerInParcel recipient = new() { Id = IdTarget };
            BO.Parcel p = new()
            {
                CustomerInParcelSender = sender,
                CustomerInParcelRecipient = recipient,
                Weight = maxWeight,
                Priority = priorities
            };
            int parcelId = bl.AddParcel(p);
            Console.WriteLine("Parcel's Id: {0}", parcelId);
        }
        /// <summary>
        /// עדכון מודל רחפן
        /// </summary>
        public static void UpdateDroneModelData()
        {
            Console.WriteLine("Enter drone's Id:");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter new model:");
            string model = Console.ReadLine();
            bl.UpdateDroneModel(id, model);
        }
        public static void UpdateStationData()
        {
            Console.WriteLine("Enter station's Id:");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter station's name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter Number of charging stations:");
            int chargeSlots = int.Parse(Console.ReadLine());
            bl.UpdateStation(id, name, chargeSlots);
        }
        /// <summary>
        /// עדכון נתוני לקוח
        /// </summary>
        public static void UpdateCustomerData()
        {
            Console.WriteLine("Enter customer's Id:");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter customer's name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter customer's phone number:");
            string telephon = Console.ReadLine();
            bl.UpdateCustomer(id, name, telephon);
        }
        /// <summary>
        /// קליטת נתונים של
        /// שליחת רחפן לטעינה בתחנת -בסיס 
        /// </summary>
        public static void SendingDroneForChargingData()
        {
            Console.WriteLine("Enter Drone's Id:");
            int idDrone = int.Parse(Console.ReadLine());
            bl.SendingDroneForCharging(idDrone);
        }
        /// <summary>
        /// קליטת נתונים של
        ///  שחרור רחפן מטעינה בתחנת -בסיס 
        /// </summary>
        public static void ReleaseDroneFromChargingData()
        {
            Console.WriteLine("Enter Drone's Id:");
            int idDrone = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter charging time period:");
            TimeSpan chargingTime = TimeSpan.Parse(Console.ReadLine());
            bl.ReleaseDroneFromCharging(idDrone, chargingTime);
        }
        ///// <summary>
        ///// קליטת נתונים של
        ///// עדכון נתונים של שיוך של רחפו ללקוח
        ///// </summary>
        //public static void UpdateDroneToParcelData()
        //{
        //    Console.WriteLine("Enter Parcel's Id:");
        //    int idParcel = int.Parse(Console.ReadLine());
        //    Console.WriteLine("Enter Drone's Id:");
        //    int idDrone = int.Parse(Console.ReadLine());
        //    D.UpdateDroneToParcel(idDrone, idParcel);//DalObjectקריאה לפונקציה שנמצאת ב
        //}
        /// <summary>
        /// קליטת נתונים
        /// של  איסוף חבילה ע"י רחפן 
        /// </summary>
        public static void CollectionParcelFromDroneData()
        {
            Console.WriteLine("Enter Drone's Id:");
            int idDrone = int.Parse(Console.ReadLine());
            bl.CollectionParcelFromDrone(idDrone);
        }
        /// <summary>
        /// קליטת נתונים של
        ///  אספקת חבילה ל-לקוח 
        /// </summary>
        public static void DeliveryParcelForCustomerData()
        {
            Console.WriteLine("Enter Drone's Id:");
            int idParcel = int.Parse(Console.ReadLine());
            bl.DeliveryParcelByDrone(idParcel);
        }
        /// <summary>
        /// הדפסת נתונים של תחנה
        /// </summary>
        public static void ViewStationPrint()
        {
            Console.WriteLine("Enter Station's Id:");
            int idStation = int.Parse(Console.ReadLine());
            BO.Station s = bl.GetStation(idStation);
            Console.WriteLine(s);
        }
        public static void ViewCustomerPrint()
        {
            Console.WriteLine("Enter Customer's Id:");
            int idCustomer = int.Parse(Console.ReadLine());
            BO.Customer c = bl.GetCustomer(idCustomer);
            Console.WriteLine(c);
        }
        /// <summary>
        ///הדפסת נתונים של רחפן
        /// </summary>
        public static void ViewDronePrint()
        {
            Console.WriteLine("Enter Drone's Id:");
            int idDrone = int.Parse(Console.ReadLine());
            BO.Drone d = bl.GetDrone(idDrone);
            Console.WriteLine(d);
        }
        ///// <summary>
        ////הדפסת נתונים של לקוח 
        ///// </summary>
        //public static void ViewCustomerPrint()
        //{
        //    Console.WriteLine("Enter Customer's Id:");
        //    int idCustomer = int.Parse(Console.ReadLine());
        //    DalApi.DO.Customer c = D.GetCustomer(idCustomer);//DalObjectקריאה לפונקציה שנמצאת ב
        //    Console.WriteLine(c);
        //}
        /// <summary>
        ///הדפסת נתונים של חבילה
        /// </summary>
        public static void ViewParcelPrint()
        {
            Console.WriteLine("Enter Parcel's Id:");
            int idParcel = int.Parse(Console.ReadLine());
            BO.Parcel p = bl.GetParcel(idParcel);
            Console.WriteLine(p);
        }
        /// <summary>
        ///הדפסת נתונים של רשימת תחנות
        /// </summary>
        public static void ViewStationListPrint()
        {
            List<DO.Station> s = (List<DO.Station>)D.GetStationList();//DalObjectקריאה לפונקציה שנמצאת ב
            foreach (DO.Station item in s)
            {
                Console.WriteLine(item);
            }
            foreach (var item in bl.GetStationList())
            {
                Console.WriteLine(item);
            }
        }
        /// <summary>
        ///הדפסת נתונים של רשימת רחפנים
        /// </summary>
        public static void ViewDroneListPrint()
        {
            //List<DalApi.DO.Drone> d = (List<DalApi.DO.Drone>)D.GetDroneList();//DalObjectקריאה לפונקציה שנמצאת ב
            //foreach (DalApi.DO.Drone item in d)
            //{
            //    Console.WriteLine(item);
            //}
            foreach (var item in bl.GetDroneList())
            {
                Console.WriteLine(item);
            }
        }
        /// <summary>
        ///הדפסת נתונים של רשימת לקוחות
        /// </summary>
        public static void ViewCustomerListPrint()
        {
            //List<DalApi.DO.Customer> c = (List<DalApi.DO.Customer>)D.GetCustomerList();//DalObjectקריאה לפונקציה שנמצאת ב
            //foreach (DalApi.DO.Customer item in c)
            //{
            //    Console.WriteLine(item);
            //}
            foreach (var item in bl.GetCustomerList())
            {
                Console.WriteLine(item);
            }
        }
        /// <summary>
        ///הדפסת נתונים של רשימת חבילות
        /// </summary>
        public static void ViewParcelListPrint()
        {
            List<DO.Parcel> p = (List<DO.Parcel>)D.GetParcelList();//DalObjectקריאה לפונקציה שנמצאת ב
            foreach (DO.Parcel item in p)
            {
                Console.WriteLine(item);
            }
            foreach (var item in bl.GetParcelList())
            {
                Console.WriteLine(item);
            }
        }
        /// <summary>
        ///            הדפסת נתונים של רשימת חבילות
        ///שעוד לא שויכו לרחפן 
        /// </summary>
        public static void ViewParcelNoDronelListPrint()
        {
            //List<DalApi.DO.Parcel> p = (List<DalApi.DO.Parcel>)D.GetParcelNoDroneList();//DalObjectקריאה לפונקציה שנמצאת ב
            //foreach (DalApi.DO.Parcel item in p)
            //{
            //    Console.WriteLine(item);
            //}
            foreach (var item in bl.GetParcelNoDroneList())
            {
                Console.WriteLine(item);
            }
        }
        /// <summary>
        ///  הדפסת נתונים של רשימת תחנות בסיס
        ///  עם עמדות טעינה פנויות 
        /// 
        /// </summary>
        public static void ViewAvailableChargingStationslListPrint()
        {
            //List<DO.Station> s = (List<DO.Station>)D.GetAvailableChargingStationsList();//DalObjectקריאה לפונקציה שנמצאת ב
            //foreach (DO.Station item in s)
            //{
            //    Console.WriteLine(item);
            //}
            foreach (var item in bl.GetAvailableChargingStationsList())
            {
                Console.WriteLine(item);
            }
        }

        public static void GetDroneChargesListPrint()
        {
            foreach (var item in bl.GetDroneChargesList())
            {
                Console.WriteLine(item);
            }
        }
    }
}
