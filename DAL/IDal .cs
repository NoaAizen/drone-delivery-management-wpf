using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public interface IDal
    {
        /// <summary>
        /// פונקצית  הוספת רחפן לרשימת הרחפנים הקיימים 
        /// </summary>
        /// <param name="s"></param>
        void AddStation(DO.Station s);
        /// <summary>
        /// פונקצית הוספת רחפן לרשימת רחפנים 
        /// </summary>
        /// <param name="d"></param>
        void AddDrone(DO.Drone d);
        /// <summary>
        ///  פונקציית קליטת לקוח חדש לרשימת הלקוחות 
        /// </summary>
        /// <param name="c"></param>
        void AddCustomer(DO.Customer c);
        /// <summary>
        ///  פונקציית קליטת חבילה למשלוח
        /// </summary>
        /// <param name="p"></param>
        int AddParcel(DO.Parcel p);
        /// <summary>
        /// עדכון מודל רחפן
        /// </summary>
        /// <param name="id">מזהה הרחפן לעדכון</param>
        /// <param name="model">שם המודל חדש</param>
        public void UpdateDroneModel(int id, string model);
        /// <summary>
        /// עדכון נתוני תחנה
        /// </summary>
        /// <param name="id">מזהה תחנה</param>
        /// <param name="name">שם חדש</param>
        /// <param name="totalChargingStations">כמות עמדות טעינה כוללת</param>
        public void UpdateStation(int id, string name, int totalChargingStations);
        /// <summary>
        /// עדכון נתוני לקוח
        /// </summary>
        /// <param name="id">מספר מזהה של הלקוח</param>
        /// <param name="name">שם חדש</param>
        /// <param name="phone">טלפון חדש</param>
        public void UpdateCustomer(int id, string name, string phone);
        /// <summary>
        /// פונקצית שיוך חבילה לרחפן 
        /// </summary>
        /// <param name="idDrone"></param>
        /// <param name="idParcel"></param>
        void UpdateDroneToParcel(int idDrone, int idParcel);
        /// <summary>
        /// פונקציית איסוף חבילה ע"י רחפן 
        /// </summary>
        /// <param name="idDrone"></param>
        /// <param name="idParcel"></param>
        void CollectionParcelFromDrone(int idDrone, int idParcel);

        /// <summary>
        /// פונקציית אספקת חבילה ללקוח 
        /// </summary>
        /// <param name="idCustomer"></param>
        /// <param name="idParcel"></param>
        void DeliveryParcelForCustomer(int idCustomer, int idParcel);

        /// <summary>
        /// פונקציית שליחת רחפן לטעינה בתחנת בסיס
        /// </summary>
        /// <param name="idDrone"></param>
        /// <param name="idStation"></param>
        void SendingDroneForCharging(int idDrone, int idStation);


        /// <summary>
        /// פונקציית שחרור רחפן מטעינה בתחנת בסיס
        /// </summary>
        /// <param name="idDrone"></param>
        /// <param name="idStation"></param>
        void ReleaseDroneFromCharging(int idDrone, int idStation);

        /// <summary>
        /// פונקציית להדפסה תחנה אחת
        /// </summary>
        /// <param name="idStation"></param>
        /// <returns></returns>
        DO.Station GetStation(int idStation);

        /// <summary>
        /// פונקציית להדפסת רחפן אחת
        /// </summary>
        /// <param name="idDrone"></param>
        /// <returns></returns>
        DO.Drone GetDrone(int idDrone);

        /// <summary>
        /// פונקציית הדפסת לקוח אחד
        /// </summary>
        /// <param name="idCustomer"></param>
        /// <returns></returns>
        DO.Customer GetCustomer(int idCustomer);

        /// <summary>
        /// הדפסת חבילה אחת
        /// </summary>
        /// <param name="idParcel"></param>
        /// <returns></returns>
        DO.Parcel GetParcel(int idParcel);//הדפסת חבילה

        /// <summary>
        /// פונקציית הדפסת כל התחנות
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.Station> GetStationList();

        /// <summary>
        /// פונקציית הדפסת כל הרחפנים
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.Drone> GetDroneList();

        /// <summary>
        /// פונקציית הדפסת כל לקוחות
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.Customer> GetCustomerList();

        /// <summary>
        /// פונקציית הדפסת כל חבילות
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.Parcel> GetParcelList();
        /// <summary>
        /// תצוגת רשימת רחפנים בטעינה
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.DroneCharge> GetDroneChargesList();
        /// <summary>
        /// פונקציית הדפסת  חבילות שעוד לא שויכו לרחפן 
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.Parcel> GetParcelNoDroneList();


        /// <summary>
        /// פונקציית הדפסת תחנות עם עמדות טעינה פנויות
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.Station> GetAvailableChargingStationsList();
        /// <summary>
        /// פונקצייה המחזירה רשימת מספרים מזהים של רחפנים הנמצאים בתחנה כלשהי
        /// </summary>
        /// <param name="stationId">מזהה תחנה</param>
        /// <returns>רשימת מזהי הרחפנים הנטענים בתחנה זו </returns>
        IEnumerable<int> GetDronesInStationId(int stationId);
        /// <summary>
        /// פונקצייה המחזירה את רשימת כל החבילות שלקוח שלח
        /// </summary>
        /// <param name="senderId">מזהה לקוח</param>
        /// <returns>רשימת החבילות ששלח</returns>
        IEnumerable<Parcel> GetSenderParcels(int senderId);
        /// <summary>
        /// פונקצייה המחזירה את רשימת כל החבילות שלקוח קיבל
        /// </summary>
        /// <param name="targetId">מזהה לקוח</param>
        /// <returns>רשימת החבילות שקיבל</returns>
        IEnumerable<Parcel> GetTargetParcels(int targetId);
        /// <summary>
        /// מתודת בקשת צריכת חשמל ע"י רחפן
        /// </summary>
        /// <returns>מערך של תכונות סטטיות עבור צריכת חשמל לק"מ ע"י רחפן</returns>
        double[] PowerRequestToDrone();

        /// <summary>
        /// מחיקת חבילה 
        /// </summary>
        /// <param name="parcel">חבילה למחיקה</param>
        public void DeleteParcel(Parcel parcel);






        public IEnumerable<Drone> GetDroneList(Predicate<Drone> p);
    }
}

