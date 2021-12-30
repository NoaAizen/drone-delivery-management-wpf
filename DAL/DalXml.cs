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
        
        string stetionsPath = @"C:\Users\User\source\repos\OriyaAharoni\dotNet5782_3394_8965\DAL\Station.xml";
        
        public void AddStation(Station station)
        {
            XElement stationRoot= XMLTools.LoadListFromXMLElement(stetionsPath);
            XElement id = new XElement("id", station.Id);
            XElement name = new XElement("name", station.Name);
            XElement longitude = new XElement("longitude", station.Longitude);
            XElement latitude = new XElement("latitude", station.Latitude);
            XElement availableStations = new XElement("availableStations", station.AvailableStations);
            stationRoot.Add(new XElement("student", id, name, longitude, latitude, availableStations));
            stationRoot.Save(stetionsPath);
            XMLTools.SaveListToXMLElement(stationRoot, stetionsPath);
        }
        
        /// <summary>
        /// פונקצית הוספת רחפן לרשימת רחפנים 
        /// </summary>
        /// <param name="d"></param>
        public void AddDrone(DO.Drone d) { }
        /// <summary>
        ///  פונקציית קליטת לקוח חדש לרשימת הלקוחות 
        /// </summary>
        /// <param name="c"></param>
        public void AddCustomer(DO.Customer c) { }
        /// <summary>
        ///  פונקציית קליטת חבילה למשלוח
        /// </summary>
        /// <param name="p"></param>
        public int AddParcel(DO.Parcel p) { return 0; }
        /// <summary>
        /// עדכון מודל רחפן
        /// </summary>
        /// <param name="id">מזהה הרחפן לעדכון</param>
        /// <param name="model">שם המודל חדש</param>
        public void UpdateDroneModel(int id, string model) { }
        /// <summary>
        /// עדכון נתוני תחנה
        /// </summary>
        /// <param name="id">מזהה תחנה</param>
        /// <param name="name">שם חדש</param>
        /// <param name="totalChargingStations">כמות עמדות טעינה כוללת</param>
        public void UpdateStation(int id, string name, int totalChargingStations) { }
        /// <summary>
        /// עדכון נתוני לקוח
        /// </summary>
        /// <param name="id">מספר מזהה של הלקוח</param>
        /// <param name="name">שם חדש</param>
        /// <param name="phone">טלפון חדש</param>
        public void UpdateCustomer(int id, string name, string phone) { }
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

        /// <summary>
        /// פונקציית שליחת רחפן לטעינה בתחנת בסיס
        /// </summary>
        /// <param name="idDrone"></param>
        /// <param name="idStation"></param>
        public void SendingDroneForCharging(int idDrone, int idStation) { }


        /// <summary>
        /// פונקציית שחרור רחפן מטעינה בתחנת בסיס
        /// </summary>
        /// <param name="idDrone"></param>
        /// <param name="idStation"></param>
        public void ReleaseDroneFromCharging(int idDrone, int idStation) { }

        /// <summary>
        /// פונקציית להדפסה תחנה אחת
        /// </summary>
        /// <param name="idStation"></param>
        /// <returns></returns>
        public DO.Station GetStation(int idStation) { return new(); }

        /// <summary>
        /// פונקציית להדפסת רחפן אחת
        /// </summary>
        /// <param name="idDrone"></param>
        /// <returns></returns>
        public DO.Drone GetDrone(int idDrone) { return new(); }

        /// <summary>
        /// פונקציית הדפסת לקוח אחד
        /// </summary>
        /// <param name="idCustomer"></param>
        /// <returns></returns>
        public DO.Customer GetCustomer(int idCustomer) { return new(); }

        /// <summary>
        /// הדפסת חבילה אחת
        /// </summary>
        /// <param name="idParcel"></param>
        /// <returns></returns>
        public DO.Parcel GetParcel(int idParcel) { return new(); }//הדפסת חבילה

        /// <summary>
        /// פונקציית הדפסת כל התחנות
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.Station> GetStationList() { return new List<Station>(); }

        /// <summary>
        /// פונקציית הדפסת כל הרחפנים
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.Drone> GetDroneList() { return new List<Drone>(); }

        /// <summary>
        /// פונקציית הדפסת כל לקוחות
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.Customer> GetCustomerList() { return new List<Customer>(); }

        /// <summary>
        /// פונקציית הדפסת כל חבילות
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.Parcel> GetParcelList() { return new List<Parcel>(); }
        /// <summary>
        /// תצוגת רשימת רחפנים בטעינה
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.DroneCharge> GetDroneChargesList() { return new List<DroneCharge>(); }
        /// <summary>
        /// פונקציית הדפסת  חבילות שעוד לא שויכו לרחפן 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.Parcel> GetParcelNoDroneList() { return new List<Parcel>(); }


        /// <summary>
        /// פונקציית הדפסת תחנות עם עמדות טעינה פנויות
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.Station> GetAvailableChargingStationsList() { return new List<Station>(); }
        /// <summary>
        /// פונקצייה המחזירה רשימת מספרים מזהים של רחפנים הנמצאים בתחנה כלשהי
        /// </summary>
        /// <param name="stationId">מזהה תחנה</param>
        /// <returns>רשימת מזהי הרחפנים הנטענים בתחנה זו </returns>
        public IEnumerable<int> GetDronesInStationId(int stationId) { return new List<int>(); }
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
