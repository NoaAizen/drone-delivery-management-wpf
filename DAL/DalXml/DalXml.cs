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
using System.Runtime.CompilerServices;

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
        string stationPath = @"..\..\..\..\DAL\xml\Station.xml";
        string customerPath = @"..\..\..\..\DAL\xml\Customer.xml";
        string dronePath = @"..\..\..\..\DAL\xml\Drone.xml";
        string droneChargePath = @"..\..\..\..\DAL\xml\DroneCharge.xml";
        string parcelPath = @"..\..\..\..\DAL\xml\Parcel.xml";
        string configPath = @"..\..\..\..\DAL\xml\Config.xml";
        string userPath = @"..\..\..\..\DAL\xml\User.xml";


        //-----------------------------------ADD-----------------------------------

        #region Station
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(DO.Customer c) 
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            if (customers.Exists(x => x.Id == c.Id))
                throw new AlreadyExistException("This customer already exist");
            customers.Add(c);
            XMLTools.SaveListToXMLSerializer(customers, customerPath);
        }
        #endregion

        #region Parcel
        /// <summary>
        ///  פונקציית קליטת חבילה למשלוח
        /// </summary>
        /// <param name="p"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int AddParcel(DO.Parcel p) 
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            XElement cofingRoot = XMLTools.LoadListFromXMLElement(configPath);
            p.Id = Convert.ToInt32(cofingRoot.Element("CounterForParcels").Value);
            p.Requested = DateTime.Now;
            p.Scheduled = null;
            p.PickedUp = null;
            p.Delivered = null;
            p.DroneId = 0;
            if (parcels.Exists(x => x.Id == p.Id))
                throw new AlreadyExistException("This parcel already exist");
            parcels.Add(p);
            cofingRoot.Element("CounterForParcels").Value = (Convert.ToInt32(cofingRoot.Element("CounterForParcels").Value)+1).ToString();//הגדלה של מספר רץ ב-1 וטעינה לקובץ שמקבל רק STRING
            XMLTools.SaveListToXMLElement(cofingRoot, configPath);
            XMLTools.SaveListToXMLSerializer(parcels, parcelPath);
            return p.Id;
        }
        #endregion

        #region User
        public void AddUser(DO.User u)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(userPath);
            if (users.Exists(x => x.Id == u.Id))
                throw new AlreadyExistException("This user already exist");
            users.Add(u);
            XMLTools.SaveListToXMLSerializer(users, userPath);
        }
        #endregion

        //-----------------------------------UPDATE-----------------------------------

        #region Drone
        /// <summary>
        /// עדכון מודל רחפן
        /// </summary>
        /// <param name="id">מזהה הרחפן לעדכון</param>
        /// <param name="model">שם המודל חדש</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
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

        #region Assignment
        /// <summary>
        /// פונקצית שיוך חבילה לרחפן 
        /// </summary>
        /// <param name="idDrone"></param>
        /// <param name="idParcel"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneToParcel(int idDrone, int idParcel) {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
            if (!parcels.Exists(x => x.Id == idParcel))
                throw new DoesntExistException("This parcel doesn't exist");
            if (!drones.Exists(x => x.Id == idDrone))
                throw new DoesntExistException("This drone doesn't exist");
            Parcel parcel = parcels.Find(x => x.Id == idParcel);
            parcels.Remove(parcel);
            parcel.DroneId = idDrone;
            parcel.Scheduled = DateTime.Now;
            parcels.Add(parcel);
            XMLTools.SaveListToXMLSerializer(parcels, parcelPath);
        }
        #endregion

        #region Collection
        /// <summary>
        /// פונקציית איסוף חבילה ע"י רחפן 
        /// </summary>
        /// <param name="idDrone"></param>
        /// <param name="idParcel"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CollectionParcelFromDrone(int idDrone, int idParcel) {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
            if (!parcels.Exists(x => x.Id == idParcel))
                throw new DoesntExistException("This parcel doesn't exist");
            if (!drones.Exists(x => x.Id == idDrone))
                throw new DoesntExistException("This drone doesn't exist");
            Parcel parcel = parcels.Find(x => x.Id == idParcel);
            parcels.Remove(parcel);
            parcel.DroneId = idDrone;
            parcel.PickedUp = DateTime.Now;
            parcels.Add(parcel);
            XMLTools.SaveListToXMLSerializer(parcels, parcelPath);

        }
        #endregion

        #region Delivery
        /// <summary>
        /// פונקציית אספקת חבילה ללקוח 
        /// </summary>
        /// <param name="idCustomer"></param>
        /// <param name="idParcel"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeliveryParcelForCustomer(int idCustomer, int idParcel) {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            if (!parcels.Exists(x => x.Id == idParcel))
                throw new DoesntExistException("This parcel doesn't exist");
            if (!customers.Exists(x => x.Id == idCustomer))
                throw new DoesntExistException("This customer doesn't exist");
            Parcel parcel = parcels.Find(x => x.Id == idParcel);
            parcels.Remove(parcel);
            parcel.Delivered = DateTime.Now;
            parcels.Add(parcel);
            XMLTools.SaveListToXMLSerializer(parcels, parcelPath);
        }
        #endregion

        #region Charging
        /// <summary>
        /// פונקציית שליחת רחפן לטעינה בתחנת בסיס
        /// </summary>
        /// <param name="idDrone"></param>
        /// <param name="idStation"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
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

        #region ChangePassword
        public void ChangePassword(string password, int id)
        {
            var Users = XMLTools.LoadListFromXMLSerializer<User>(userPath);
            if (!Users.Exists(x => x.Id == id))
                throw new DoesntExistException("This user doesn't exist");
            User user = Users.Find(x => x.Id == id);
            Users.Remove(user);
            user.Password = password;
            Users.Add(user);
            XMLTools.SaveListToXMLSerializer(Users, userPath);
        }
        #endregion

        //-----------------------------------REQUEST-----------------------------------

        #region Station
        /// <summary>
        /// פונקציית להדפסה תחנה אחת
        /// </summary>
        /// <param name="idStation"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DO.Customer GetCustomer(int idCustomer) 
        {
            var customers = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            if (!customers.Exists(x => x.Id == idCustomer))
                throw new DoesntExistException("This customer doesn't exist");
            Customer customer = customers.Find(x => x.Id == idCustomer);
            return customer; 
        }
        #endregion

        #region Parcel
        /// <summary>
        /// הדפסת חבילה אחת
        /// </summary>
        /// <param name="idParcel"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DO.Parcel GetParcel(int idParcel)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);

            if (!parcels.Exists(x => x.Id == idParcel))
                throw new DoesntExistException("This parcel doesn't exist");
            Parcel parcel = parcels.Find(x => x.Id == idParcel);
            return parcel;
        }
        #endregion

        //-----------------------------------LIST-REQUEST-----------------------------------

        #region Stations
        /// <summary>
        /// תצוגת כל התחנות
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        /// תצוגת כל הרחפנים
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
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

        #region Parcel
        /// <summary>
        /// תצוגת כל החבילות
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DO.Parcel> GetParcelList() 
        {
            return XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
        }
        #endregion

        #region DroneCharges
        /// <summary>
        /// תצוגת רשימת רחפנים בטעינה
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DO.DroneCharge> GetDroneChargesList() 
        {
            return XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath); 
        }
        #endregion

        #region Parcels no drone
        /// <summary>
        /// פונקציית הדפסת  חבילות שעוד לא שויכו לרחפן 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DO.Parcel> GetParcelNoDroneList() 
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);

            return (from item in parcels
                    where item.DroneId == 0
                    select item).ToList();
        }
        #endregion

        #region Available charging stations
        /// <summary>
        /// פונקציית הדפסת תחנות עם עמדות טעינה פנויות
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
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

        #region PartOfDrone
        public IEnumerable<Drone> GetDroneList(Predicate<Drone> p)
        {
            var drones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
            return (from item in drones
                    where p(item)
                    select item).ToList();
        }
        #endregion

        #region PartOfStation
        public IEnumerable<Station> GetPartOfStationList(Predicate<Station> predicate)
        {
            return (from item in DataSource.listStations
                    where predicate(item)
                    select item).ToList();
        }
        #endregion

        #region PartOfCustomer
        public IEnumerable<Customer> GetPartOfCustomerList(Predicate<Customer> predicate)
        {
            var customers = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);

            return (from item in customers
                    where predicate(item)
                    select item).ToList();
        }
        #endregion

        #region PartOfParcel
        public IEnumerable<Parcel> GetParcelList(Predicate<Parcel> predicate)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);

            return (from item in parcels
                    where predicate(item)
                    select item).ToList();
        }
        #endregion

        #region User
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetUserList()
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(userPath);
            return from item in users
                   select item;
        }
        #endregion

        //-----------------------------------HELP-METHODS-----------------------------------

        #region GetDronesInStationId
        /// <summary>
        /// פונקצייה המחזירה רשימת מספרים מזהים של רחפנים הנמצאים בתחנה כלשהי
        /// </summary>
        /// <param name="stationId">מזהה תחנה</param>
        /// <returns>רשימת מזהי הרחפנים הנטענים בתחנה זו </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<int> GetDronesInStationId(int stationId)
        {
            var droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
            return (from item in droneCharges
                    where item.StationId == stationId
                    select item.DroneId).ToList();
        }
        #endregion

        #region GetSenderParcels

        /// <summary>
        /// פונקצייה המחזירה את רשימת כל החבילות שלקוח שלח
        /// </summary>
        /// <param name="senderId">מזהה לקוח</param>
        /// <returns>רשימת החבילות ששלח</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetSenderParcels(int senderId) 
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);

            return (from item in parcels
                    where item.SenderId == senderId
                    select item).ToList();
        }
        #endregion

        #region GetTargetParcels

        /// <summary>
        /// פונקצייה המחזירה את רשימת כל החבילות שלקוח קיבל
        /// </summary>
        /// <param name="targetId">מזהה לקוח</param>
        /// <returns>רשימת החבילות שקיבל</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetTargetParcels(int targetId) 
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            return (from item in parcels
                    where item.TargetId == targetId
                    select item).ToList();
        }
        #endregion

        #region PowerRequestToDrone
        /// <summary>
        /// מתודת בקשת צריכת חשמל ע"י רחפן
        /// </summary>
        /// <returns>מערך של תכונות סטטיות עבור צריכת חשמל לק"מ ע"י רחפן</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] PowerRequestToDrone() 
        {
            XElement cofingRoot = XMLTools.LoadListFromXMLElement(configPath);

            double[] arr = new double[]
            {
                Convert.ToDouble(cofingRoot.Element("available").Value),
                Convert.ToDouble(cofingRoot.Element("lightWeight").Value),
                Convert.ToDouble(cofingRoot.Element("mediumWeight").Value),
                Convert.ToDouble(cofingRoot.Element("heavyWeight").Value),
                Convert.ToDouble(cofingRoot.Element("chargingRate").Value),
            };
            return arr;
        }
        #endregion

        #region DeleteParcel
        /// <summary>
        /// מחיקת חבילה 
        /// </summary>
        /// <param name="parcel">חבילה למחיקה</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(Parcel parcel) 
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            parcels.Remove(parcel);
            XMLTools.SaveListToXMLSerializer(parcels, parcelPath);

        }
        #endregion

    }
}
