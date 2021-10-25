using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            AddStationData();
           
        }
       static int temp = 0;

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
            DAL.DalObject.DalObject.AddStation(s);
        }

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
            DAL.IDAL.DO.StatusDrone status = (DAL.IDAL.DO.StatusDrone)temp;
            Console.WriteLine("Enter drone's battery:");
            double battery = double.Parse(Console.ReadLine());
            DAL.IDAL.DO.Drone d = new DAL.IDAL.DO.Drone(id, model, MAX_weight, status, battery);
            DAL.DalObject.DalObject.AddDrone(d);
        
        }

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
            DAL.DalObject.DalObject.AddCustomer(c);
        }

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
           DAL.DalObject.DalObject.AddParcel(p);
          
        }


        public static void UpdateDroneToParcelData()
        {
            Console.WriteLine("Enter Parcel's Id:");
            int idParcel = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Drone's Id:");
            int idDrone = int.Parse(Console.ReadLine());
            DAL.DalObject.DalObject.UpdateDroneToParcel(idDrone, idParcel);
        }
        public static void CollectionParcelFromDroneData()
        {
            Console.WriteLine("Enter Parcel's Id:");
            int idParcel = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Drone's Id:");
            int idDrone = int.Parse(Console.ReadLine());
            DAL.DalObject.DalObject.CollectionParcelFromDrone(idDrone, idParcel);
        }
        public static void DeliveryParcelForCustomerData()
        {
            Console.WriteLine("Enter Parcel's Id:");
            int idParcel = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Customer's Id:");
            int idCustomer = int.Parse(Console.ReadLine());
            DAL.DalObject.DalObject.DeliveryParcelForCustomer(idCustomer, idParcel);
        }

        ///////// שליחת רחפן לטעינה

        public static void ReleaseDroneFromChargingData()
        {
            Console.WriteLine("Enter Drone's Id:");
            int idDrone = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Station's Id:");
            int idStation = int.Parse(Console.ReadLine());
            DAL.DalObject.DalObject.ReleaseDroneFromCharging(idDrone, idStation);
        }

        public static void ViewStationPrint()
        {
            Console.WriteLine("Enter Station's Id:");
            int idStation = int.Parse(Console.ReadLine());
            DAL.IDAL.DO.Station s= DAL.DalObject.DalObject.ViewStation(idStation);
            Console.WriteLine(s);
        }
    }
}
