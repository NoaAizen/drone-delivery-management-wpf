using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml.Linq;
using DalApi;
using DO;
using DalObject;

namespace DalXml
{
    internal sealed class DalXml : IDal
    {
        static readonly DalXml instance = new DalXml();
        public static DalXml Instance { get => instance; }
        static DalXml() { }
        DalXml()
        {
            XmlSource.Config.Initialize();
        }

        //string stetionsPath = @"DAL\xml\Station.xml";
        string stationPath = @"C:\Users\User\source\repos\OriyaAharoni\dotNet5782_3394_8965\DAL\xml\Station.xml";
        string customerPath = @"C:\Users\User\source\repos\OriyaAharoni\dotNet5782_3394_8965\DAL\xml\Customer.xml";
        string dronePath = @"C:\Users\User\source\repos\OriyaAharoni\dotNet5782_3394_8965\DAL\xml\Drone.xml";
        string droneChargePath = @"C:\Users\User\source\repos\OriyaAharoni\dotNet5782_3394_8965\DAL\xml\DroneCharge.xml";
        string parcelPath = @"C:\Users\User\source\repos\OriyaAharoni\dotNet5782_3394_8965\DAL\xml\Parcel.xml";

        //-----------------------------------ADD-----------------------------------

        #region Station
        public void AddStation(Station station)
        {
            XElement stationRoot= XMLTools.LoadListFromXMLElement(stationPath);

            XElement stat = (from s in stationRoot.Elements()
                                where int.Parse(s.Element("Id").Value) == station.Id
                                select s).FirstOrDefault();
            if (stat != null)
                throw new AlreadyExistException("This station already exist");
            XElement id = new XElement("Id", station.Id);
            XElement name = new XElement("Name", station.Name);
            XElement longitude = new XElement("Longitude", station.Longitude);
            XElement latitude = new XElement("Latitude", station.Latitude);
            XElement availableStations = new XElement("AvailableStations", station.AvailableStations);
            stationRoot.Add(new XElement("Station", id, name, longitude, latitude, availableStations));
            stationRoot.Save(stationPath);
            XMLTools.SaveListToXMLElement(stationRoot, stationPath);
        }
        #endregion

        #region Drone
        /// <summary>
        /// פונקצית הוספת רחפן לרשימת רחפנים 
        /// </summary>
        /// <param name="d"></param>
        public void AddDrone(DO.Drone d) 
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
            if (drones.Exists(x => x.Id == d.Id))
                throw new AlreadyExistException("This drone already exist");
            drones.Add(d);
            XMLTools.SaveListToXMLSerializer(drones, dronePath);
        }
        #endregion

        #region Customer
        /// <summary>
        ///  פונקציית קליטת לקוח חדש לרשימת הלקוחות 
        /// </summary>
        /// <param name="c"></param>
        public void AddCustomer(DO.Customer c) 
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            if (customers.Exists(x => x.Id == c.Id))
                throw new AlreadyExistException("This customer already exist");
            customers.Add(c);
            XMLTools.SaveListToXMLSerializer(customers, customerPath);
        }
        #endregion
        /// <summary>
        ///  פונקציית קליטת חבילה למשלוח
        /// </summary>
        /// <param name="p"></param>
      
        public int AddParcel(DO.Parcel p) 
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            if (parcels.Exists(x => x.Id == p.Id))
                throw new AlreadyExistException("This parcel already exist");
            parcels.Add(p);
            XMLTools.SaveListToXMLSerializer(parcels, parcelPath);
            return CounterForParcels;


        }

        //-----------------------------------UPDATE-----------------------------------

        #region Drone
        /// <summary>
        /// עדכון מודל רחפן
        /// </summary>
        /// <param name="id">מזהה הרחפן לעדכון</param>
        /// <param name="model">שם המודל חדש</param>
        public void UpdateDroneModel(int id, string model) 
        {
            var drones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
            if (!drones.Exists(x => x.Id == id))
                throw new DoesntExistException("This drone doesn't exist");
            Drone drone = drones.Find(x => x.Id == id);
            drones.Remove(drone);
            drone.Model = model;
            drones.Add(drone);
            XMLTools.SaveListToXMLSerializer(drones, dronePath);
        }
        #endregion

        #region Station
        /// <summary>
        /// עדכון נתוני תחנה
        /// </summary>
        /// <param name="id">מזהה תחנה</param>
        /// <param name="name">שם חדש</param>
        /// <param name="totalChargingStations">כמות עמדות טעינה כוללת</param>
        public void UpdateStation(int id, string name, int totalChargingStations) 
        {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            XElement station = (from s in stationRoot.Elements()
                            where int.Parse(s.Element("Id").Value) == id
                                select s).FirstOrDefault();
            if (station != null)
            {
                if (name != "")
                    station.Element("Name").Value = name;
                if (totalChargingStations != 0)
                    station.Element("AvailableStations").Value = totalChargingStations.ToString();
                XMLTools.SaveListToXMLElement(stationRoot, stationPath);
            }
            else
                throw new DoesntExistException("This station doesn't exist");
        }
        #endregion

        #region Customer
        /// <summary>
        /// עדכון נתוני לקוח
        /// </summary>
        /// <param name="id">מספר מזהה של הלקוח</param>
        /// <param name="name">שם חדש</param>
        /// <param name="phone">טלפון חדש</param>
        public void UpdateCustomer(int id, string name, string phone)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            if (!customers.Exists(x => x.Id == id))
                throw new DoesntExistException("This customer doesn't exist");
            Customer customer = customers.Find(x => x.Id == id);
            customers.Remove(customer);
            if (name != "")
                customer.Name = name;
            if (phone != "")
                customer.Phone = phone;
            customers.Add(customer);
            XMLTools.SaveListToXMLSerializer(customers, customerPath);
        }
        #endregion

        /// <summary>
        /// פונקצית שיוך חבילה לרחפן 
        /// </summary>
        /// <param name="idDrone"></param>
        /// <param name="idParcel"></param>
        public void UpdateDroneToParcel(int idDrone, int idParcel) { }
        /// <summary>
        /// פונקציית איסוף חבילה ע"י רחפן 
        /// </summary>
        /// <param name="idDrone"></param>
        /// <param name="idParcel"></param>
        public void CollectionParcelFromDrone(int idDrone, int idParcel) { }

        /// <summary>
        /// פונקציית אספקת חבילה ללקוח 
        /// </summary>
        /// <param name="idCustomer"></param>
        /// <param name="idParcel"></param>
        public void DeliveryParcelForCustomer(int idCustomer, int idParcel) { }

        #region Charging
        /// <summary>
        /// פונקציית שליחת רחפן לטעינה בתחנת בסיס
        /// </summary>
        /// <param name="idDrone"></param>
        /// <param name="idStation"></param>
        public void SendingDroneForCharging(int idDrone, int idStation)
        {
            var drones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
            if (!drones.Exists(x => x.Id == idDrone))
                throw new DoesntExistException("This drone doesn't exist");
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            XElement station = (from s in stationRoot.Elements()
                                where int.Parse(s.Element("Id").Value) == idStation
                                select s).FirstOrDefault();
            if (station != null)
            {
                station.Element("AvailableStations").Value = 
                    (int.Parse(station.Element("AvailableStations").Value) - 1).ToString();
                XMLTools.SaveListToXMLElement(stationRoot, stationPath);
            }
            else
                throw new DoesntExistException("This station doesn't exist");
            var droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
            DroneCharge dc = new() { DroneId = idDrone, StationId = idStation };
            droneCharges.Add(dc);
            XMLTools.SaveListToXMLSerializer(droneCharges, droneChargePath);
        }
        #endregion

        #region Release
        /// <summary>
        /// פונקציית שחרור רחפן מטעינה בתחנת בסיס
        /// </summary>
        /// <param name="idDrone"></param>
        /// <param name="idStation"></param>
        public void ReleaseDroneFromCharging(int idDrone, int idStation)
        {
            var drones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
            if (!drones.Exists(x => x.Id == idDrone))
                throw new DoesntExistException("This drone doesn't exist");
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            XElement station = (from s in stationRoot.Elements()
                                where int.Parse(s.Element("Id").Value) == idStation
                                select s).FirstOrDefault();
            if (station != null)
            {
                station.Element("AvailableStations").Value =
                    (int.Parse(station.Element("AvailableStations").Value) + 1).ToString();
                XMLTools.SaveListToXMLElement(stationRoot, stationPath);
            }
            else
                throw new DoesntExistException("This station doesn't exist");
            var droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
            DroneCharge dc = (from item in droneCharges
                              where item.DroneId == idDrone && item.StationId == idStation
                              select item).FirstOrDefault();
            droneCharges.Remove(dc);
            XMLTools.SaveListToXMLSerializer(droneCharges, droneChargePath);
        }
        #endregion

        //-----------------------------------REQUEST-----------------------------------

        #region Station
        /// <summary>
        /// פונקציית להדפסה תחנה אחת
        /// </summary>
        /// <param name="idStation"></param>
        /// <returns></returns>
        public DO.Station GetStation(int idStation) 
        {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            XElement stat = (from s in stationRoot.Elements()
                             where int.Parse(s.Element("Id").Value) == idStation
                             select s).FirstOrDefault();
            if (stat == null)
                throw new DoesntExistException("This station doesn't exist");
            Station station = (from s in stationRoot.Elements()
                        where int.Parse(s.Element("Id").Value) == idStation
                        select new Station()
                        {
                            Id = Int32.Parse(s.Element("Id").Value),
                            Name = s.Element("Name").Value,
                            Longitude = double.Parse(s.Element("Longitude").Value),
                            Latitude = double.Parse(s.Element("Latitude").Value),
                            AvailableStations = Int32.Parse(s.Element("AvailableStations").Value)
                        }).FirstOrDefault();
            return station;
             
        }
        #endregion

        #region Drone
        /// <summary>
        /// פונקציית להדפסת רחפן אחת
        /// </summary>
        /// <param name="idDrone"></param>
        /// <returns></returns>
        public DO.Drone GetDrone(int idDrone) 
        {
            var drones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
            if (!drones.Exists(x => x.Id == idDrone))
                throw new DoesntExistException("This drone doesn't exist");
            Drone drone = drones.Find(x => x.Id == idDrone);
            return drone;
        }
        #endregion

        #region Customer
        /// <summary>
        /// פונקציית הדפסת לקוח אחד
        /// </summary>
        /// <param name="idCustomer"></param>
        /// <returns></returns>
        public DO.Customer GetCustomer(int idCustomer) 
        {
            var customers = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            if (!customers.Exists(x => x.Id == idCustomer))
                throw new DoesntExistException("This customer doesn't exist");
            Customer customer = customers.Find(x => x.Id == idCustomer);
            return customer; 
        }
        #endregion

        /// <summary>
        /// הדפסת חבילה אחת
        /// </summary>
        /// <param name="idParcel"></param>
        /// <returns></returns>
        public DO.Parcel GetParcel(int idParcel) { return new(); }//הדפסת חבילה

        //-----------------------------------LIST-REQUEST-----------------------------------

        #region Stations
        /// <summary>
        /// פונקציית הדפסת כל התחנות
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.Station> GetStationList()
        {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);

            return (from s in stationRoot.Elements()
                   select new Station()
                   {
                       Id = Int32.Parse(s.Element("Id").Value),
                       Name = s.Element("Name").Value,
                       Longitude = double.Parse(s.Element("Longitude").Value),
                       Latitude = double.Parse(s.Element("Latitude").Value),
                       AvailableStations = Int32.Parse(s.Element("AvailableStations").Value)
                   }).ToList();
        }
        #endregion

        #region Drones
        /// <summary>
        /// פונקציית הדפסת כל הרחפנים
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.Drone> GetDroneList() 
        {
            return XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
        }
        #endregion

        #region Customer
        /// <summary>
        /// פונקציית הדפסת כל לקוחות
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.Customer> GetCustomerList() 
        {
            return XMLTools.LoadListFromXMLSerializer<Customer>(customerPath); 
        }
        #endregion

        /// <summary>
        /// פונקציית הדפסת כל חבילות
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.Parcel> GetParcelList() 
        {
            return XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
        }

        #region DroneCharges
        /// <summary>
        /// תצוגת רשימת רחפנים בטעינה
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.DroneCharge> GetDroneChargesList() 
        {
            return XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath); 
        }
        #endregion

        /// <summary>
        /// פונקציית הדפסת  חבילות שעוד לא שויכו לרחפן 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.Parcel> GetParcelNoDroneList() { return new List<Parcel>(); }

        #region Available charging stations
        /// <summary>
        /// פונקציית הדפסת תחנות עם עמדות טעינה פנויות
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.Station> GetAvailableChargingStationsList()
        {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);

            return (from s in stationRoot.Elements()
                    where int.Parse(s.Element("AvailableStations").Value) > 0
                    select new Station()
                    {
                        Id = Int32.Parse(s.Element("Id").Value),
                        Name = s.Element("Name").Value,
                        Longitude = double.Parse(s.Element("Longitude").Value),
                        Latitude = double.Parse(s.Element("Latitude").Value),
                        AvailableStations = Int32.Parse(s.Element("AvailableStations").Value)
                    }).ToList();
        }
        #endregion

        /// <summary>
        /// פונקצייה המחזירה רשימת מספרים מזהים של רחפנים הנמצאים בתחנה כלשהי
        /// </summary>
        /// <param name="stationId">מזהה תחנה</param>
        /// <returns>רשימת מזהי הרחפנים הנטענים בתחנה זו </returns>
        public IEnumerable<int> GetDronesInStationId(int stationId)
        {
            var droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
            return (from item in droneCharges
                    where item.StationId == stationId
                    select item.DroneId).ToList();
        }
        /// <summary>
        /// פונקצייה המחזירה את רשימת כל החבילות שלקוח שלח
        /// </summary>
        /// <param name="senderId">מזהה לקוח</param>
        /// <returns>רשימת החבילות ששלח</returns>
        public IEnumerable<Parcel> GetSenderParcels(int senderId) { return new List<Parcel>(); }
        /// <summary>
        /// פונקצייה המחזירה את רשימת כל החבילות שלקוח קיבל
        /// </summary>
        /// <param name="targetId">מזהה לקוח</param>
        /// <returns>רשימת החבילות שקיבל</returns>
        public IEnumerable<Parcel> GetTargetParcels(int targetId) { return new List<Parcel>(); }
        /// <summary>
        /// מתודת בקשת צריכת חשמל ע"י רחפן
        /// </summary>
        /// <returns>מערך של תכונות סטטיות עבור צריכת חשמל לק"מ ע"י רחפן</returns>
        public double[] PowerRequestToDrone() {  double[] arr = new double[] { }; return arr; }

        /// <summary>
        /// מחיקת חבילה 
        /// </summary>
        /// <param name="parcel">חבילה למחיקה</param>
        public void DeleteParcel(Parcel parcel) { }






        public IEnumerable<Drone> GetDroneList(Predicate<Drone> p) { return new List<Drone>(); }
        public IEnumerable<Parcel> GetParcelList(Predicate<Parcel> predicate) { return new List<Parcel>(); }













        //public static void saveListToXML(List<Station> list, string path)
        //{
        //    XmlSerializer x = new XmlSerializer(list.GetType());
        //    FileStream fs = new FileStream(path, FileMode.Create);
        //    x.Serialize(fs, list);
        //}
        //XElement stationRoot;
        //string FPath = @"C:\Users\User\source\repos\OriyaAharoni\dotNet5782_3394_8965\DAL\Station.xml";



    }
}
