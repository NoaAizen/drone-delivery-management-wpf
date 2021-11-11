using System;
using System.Collections.Generic;

namespace ConsoleUI
{
//הגדרה של enum 
    public enum Choice {ADD=1, UPDATE, VIEW, VIEWLIST, EXIT};
    public enum Add {STATION=1, DRONE, CUSTOMER, PARCEL};
    public enum Update {ASSING=1, COLLECTION, DELIVERY, CHARGING, RELEASE };
    public enum View { STATION = 1, DRONE, CUSTOMER, PARCEL };
    public enum ViewList { STATIONS = 1, DRONES, CUSTOMERS, PARCELS, NODRONE, AVAILABLE };

    class Program
    {
        public static DAL.DalObject.DalObject D = new DAL.DalObject.DalObject();//קריאה לבנאי שמתאחל
        static void Main(string[] args)
        {
            //D = new DAL.DalObject.DalObject();//קריאה לבנאי שמתאחל
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
                switch(choice)
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
                        switch(add) //אוופציות של הוספה
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
1: for assign a parcel to a drone
2: for collection a parcel by a drone
3: for delivery a parcel for a customer
4: for sending a drone for charging
5: for release a drone from charging");
                        update = (Update)int.Parse(Console.ReadLine());
                        switch(update)//עדכון
                        {
                            case Update.ASSING://שיוך חבילה לרחפן 
                                UpdateDroneToParcelData();
                                break;
                            case Update.COLLECTION:// איסוף חבילה ע"י רחפ ן 
                                CollectionParcelFromDroneData();
                                break;
                            case Update.DELIVERY:// אספקת חבילה ל-לקוח 
                                DeliveryParcelForCustomerData();
                                break;
                            case Update.CHARGING:// שליחת רחפן לטעינה בתחנת -בסיס 
                                SendingDroneForChargingData();
                                break;
                            case Update.RELEASE:// שחרור רחפן מטעינה בתחנת -בסיס 
                                ReleaseDroneFromChargingData();
                                break;
                            default:
                                Console.WriteLine("Enter a number between 1 to 5");
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
                            default:
                                Console.WriteLine("Enter a number between 1 to 6");
                                break;
                        };
                        break;

                    case Choice.EXIT:
                        break;

                    default:
                        Console.WriteLine("Enter a number between 1 to 5" );
                        break;
                };
            }
           
        }
       static int temp = 0;
       //static DAL.DalObject.DalObject D = new DAL.DalObject.DalObject();
        //קליטת והדפסת נותנים
        /// <summary>
        /// קליטה של של הוספת אטובוס
        /// </summary>
        public static void AddStationData()
        {
            Console.WriteLine("Enter station's Id:");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter station's name:");
            int Name = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Number of charging stations available:");
            int ChargeSlots = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter station's longitude:");
            double longitude = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter station's latitude:");
            double latitude = double.Parse(Console.ReadLine());
            DAL.IDAL.DO.Station s = new DAL.IDAL.DO.Station(id, Name, ChargeSlots, longitude, latitude);
            D.AddStation(s);
        }
        /// <summary>
        /// קליטת נתונים של רחפן
        /// </summary>
        public static void AddDroneData()
        {
            Console.WriteLine("Enter drone's Id:");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter drone's model:");
            string model = (Console.ReadLine());
            Console.WriteLine("Enter drone's weight categories(Light = 0, Intermediate = 1, Heavy = 2):");
            temp = int.Parse(Console.ReadLine());
            DAL.IDAL.DO.WeightCategories MAX_weight = (DAL.IDAL.DO.WeightCategories)temp;
            Console.WriteLine("Enter drone's status:(Available = 0, Maintenance = 1, Shipping = 2)");
            temp = int.Parse(Console.ReadLine());
            //DAL.IDAL.DO.StatusDrone status = (DAL.IDAL.DO.StatusDrone)temp;
            Console.WriteLine("Enter drone's battery:");
            //double battery = double.Parse(Console.ReadLine());
            DAL.IDAL.DO.Drone d = new DAL.IDAL.DO.Drone(id, model, MAX_weight/*, status, battery*/);
            D.AddDrone(d);
        
        }
        /// <summary>
        /// קליטת נתונים של לקוח
        /// </summary>
        public static void AddCustomerData()
        {
            Console.WriteLine("Enter customer's Id:");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter customer's name:");
            string Name = Console.ReadLine();
            Console.WriteLine("Enter customer's phone number:");
            string Telephon = Console.ReadLine();
            Console.WriteLine("Enter customer's longitude:");
            double longitude = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter customer's latitude:");
            double latitude = double.Parse(Console.ReadLine());
            DAL.IDAL.DO.Customer c = new DAL.IDAL.DO.Customer(id, Name, Telephon, longitude, latitude);
            D.AddCustomer(c);
        }
        /// <summary>
        /// קליטת נתונים של חבילה
        /// </summary>
        public static void AddParcelData()
        {
            Console.WriteLine("Enter Parcel's Id:");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Id of sender:");
            int IdSender = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Id of target:");
            int IdTarget = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Parcel's weight categories(Light = 0, Intermediate = 1, Heavy = 2):");
            temp = int.Parse(Console.ReadLine());
            DAL.IDAL.DO.WeightCategories MAX_weight = (DAL.IDAL.DO.WeightCategories)temp;
            Console.WriteLine("Enter Parcel's priority(Normal = 0, Fast = 1, Emergency = 2):");
            temp = int.Parse(Console.ReadLine());
            DAL.IDAL.DO.Priorities priorities = (DAL.IDAL.DO.Priorities)temp;
            Console.WriteLine("Enter Id of Drone:");
            int IdDrone = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Parcel's Requsted time:");
            DateTime Requsted = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Enter Parcel's Schduled time:");
            DateTime Schduled = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Enter Parcel's PickedUp time:");
            DateTime PickedUp = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Enter Parcel's Delivered time:");
            DateTime Delivered = DateTime.Parse(Console.ReadLine());
            DAL.IDAL.DO.Parcel p= new DAL.IDAL.DO.Parcel(id, IdSender, IdTarget, MAX_weight, 
                priorities, IdDrone, Requsted, Schduled, PickedUp, Delivered);
           D.AddParcel(p);//DalObjectקריאה לפונקציה שנמצאת ב

        }

        /// <summary>
        /// קליטת נתונים של
        /// עדכון נתונים של שיוך של רחפו ללקוח
        /// </summary>
        public static void UpdateDroneToParcelData()
        {
            Console.WriteLine("Enter Parcel's Id:");
            int idParcel = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Drone's Id:");
            int idDrone = int.Parse(Console.ReadLine());
            D.UpdateDroneToParcel(idDrone, idParcel);//DalObjectקריאה לפונקציה שנמצאת ב
        }
        /// <summary>
        /// קליטת נתונים
        /// של  איסוף חבילה ע"י רחפ ן 
        /// </summary>
        public static void CollectionParcelFromDroneData()
        {
            Console.WriteLine("Enter Parcel's Id:");
            int idParcel = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Drone's Id:");
            int idDrone = int.Parse(Console.ReadLine());
            D.CollectionParcelFromDrone(idDrone, idParcel);//DalObjectקריאה לפונקציה שנמצאת ב
        }
        /// <summary>
        /// קליטת נתונים של
        ///  אספקת חבילה ל-לקוח 
        /// </summary>
        public static void DeliveryParcelForCustomerData()
        {
            Console.WriteLine("Enter Parcel's Id:");
            int idParcel = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Customer's Id:");
            int idCustomer = int.Parse(Console.ReadLine());
            D.DeliveryParcelForCustomer(idCustomer, idParcel);//DalObjectקריאה לפונקציה שנמצאת ב
        }

        /// <summary>
        /// קליטת נתונים של
        /// שליחת רחפן לטעינה בתחנת -בסיס 
        /// </summary>
        public static void SendingDroneForChargingData()
        {
            Console.WriteLine("Enter Drone's Id:");
            int idDrone = int.Parse(Console.ReadLine());
            ViewAvailableChargingStationslListPrint();
            Console.WriteLine("Enter Station's Id:");
            int idStation = int.Parse(Console.ReadLine());
            D.SendingDroneForCharging(idDrone, idStation);//DalObjectקריאה לפונקציה שנמצאת ב
        }
        /// <summary>
        /// קליטת נתונים של
        ///  שחרור רחפן מטעינה בתחנת -בסיס 
        /// </summary>
        public static void ReleaseDroneFromChargingData()
        {
            Console.WriteLine("Enter Drone's Id:");
            int idDrone = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Station's Id:");
            int idStation = int.Parse(Console.ReadLine());
            D.ReleaseDroneFromCharging(idDrone, idStation);//DalObjectקריאה לפונקציה שנמצאת ב
        }
        /// <summary>
        /// קליטת והדפסת נתונים של רשומת  תחנה
        /// </summary>
        public static void ViewStationPrint()
        {
            Console.WriteLine("Enter Station's Id:");
            int idStation = int.Parse(Console.ReadLine());
            DAL.IDAL.DO.Station s= D.ViewStation(idStation);//DalObjectקריאה לפונקציה שנמצאת ב
            Console.WriteLine(s);
        }
        /// <summary>
        ///         /// קליטת והדפסת נתונים של רשומת  רחפן
        /// </summary>
        public static void ViewDronePrint()
        {
            Console.WriteLine("Enter Drone's Id:");
            int idDrone = int.Parse(Console.ReadLine());
            DAL.IDAL.DO.Drone d = D.ViewDrone(idDrone);//DalObjectקריאה לפונקציה שנמצאת ב
            Console.WriteLine(d);
        }
        /// <summary>
        ///          קליטת והדפסת נתונים של רשומת לקןח 

        /// </summary>
        public static void ViewCustomerPrint()
        {
            Console.WriteLine("Enter Customer's Id:");
            int idCustomer = int.Parse(Console.ReadLine());
            DAL.IDAL.DO.Customer c = D.ViewCustomer(idCustomer);//DalObjectקריאה לפונקציה שנמצאת ב
            Console.WriteLine(c);
        }
        /// <summary>
        ///          קליטת והדפסת נתונים של רשומת של חבילה

        /// </summary>
        public static void ViewParcelPrint()
        { 
            Console.WriteLine("Enter Parcel's Id:");
            int idParcel = int.Parse(Console.ReadLine());
            DAL.IDAL.DO.Parcel p = D.ViewParcel(idParcel);//DalObjectקריאה לפונקציה שנמצאת ב
            Console.WriteLine(p);
        }
        /// <summary>
        ///           והדפסת נתונים של רשימת תחנות

        /// </summary>
        public static void ViewStationListPrint()
        {
             List<DAL.IDAL.DO.Station> s = (List<DAL.IDAL.DO.Station>)D.ViewStationList();//DalObjectקריאה לפונקציה שנמצאת ב
            foreach (DAL.IDAL.DO.Station item in s)
              {
                 Console.WriteLine(item) ;
              }
        }

        /// <summary>
        ///                   הדפסת נתונים של רשימת רחפנים
        /// </summary>
        public static void ViewDroneListPrint()
        {
            List<DAL.IDAL.DO.Drone> d = (List<DAL.IDAL.DO.Drone>)D.ViewDroneList();//DalObjectקריאה לפונקציה שנמצאת ב
            foreach (DAL.IDAL.DO.Drone item in d)
            {
                Console.WriteLine(item);
            }
        }
        /// <summary>
        ///   הדפסת נתונים של רשימת לקוחות
        /// </summary>
        public static void ViewCustomerListPrint()
        {
            List<DAL.IDAL.DO.Customer> c = (List<DAL.IDAL.DO.Customer>)D.ViewCustomerList();//DalObjectקריאה לפונקציה שנמצאת ב
            foreach (DAL.IDAL.DO.Customer item in c)
            {
                Console.WriteLine(item);
            }
        }
        /// <summary>
        ///   הדפסת נתונים של רשימת חבילות
        /// </summary>
        public static void ViewParcelListPrint()
        {
            List<DAL.IDAL.DO.Parcel> p = (List<DAL.IDAL.DO.Parcel>)D.ViewParcelList();//DalObjectקריאה לפונקציה שנמצאת ב
            foreach (DAL.IDAL.DO.Parcel item in p)
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
            List<DAL.IDAL.DO.Parcel> p = (List<DAL.IDAL.DO.Parcel>)D.ViewParcelNoDronelList();//DalObjectקריאה לפונקציה שנמצאת ב
            foreach (DAL.IDAL.DO.Parcel item in p)
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
            List<DAL.IDAL.DO.Station> s = (List<DAL.IDAL.DO.Station>)D.ViewAvailableChargingStationslList();//DalObjectקריאה לפונקציה שנמצאת ב
            foreach (DAL.IDAL.DO.Station item in s)
            {
                Console.WriteLine(item);
            }
        }
    }
}
