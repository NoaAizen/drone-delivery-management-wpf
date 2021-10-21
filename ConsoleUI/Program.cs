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
            DAL.DalObject.DalObject.AddStation(id, Name, ChargeSlots, longitude, latitude);
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
            DAL.DalObject.DalObject.AddDrone(id, model, MAX_weight, status, battery);
        
        }
    }
}
