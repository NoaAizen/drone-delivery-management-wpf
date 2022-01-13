using BO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BlApi
{
    public interface IBL
    {
        /// <summary>
        /// הוספת תחנת בסיס
        /// </summary>
        /// <param name="s">ישות לוגית של תחנה להוספה</param>
        public void AddStation(Station s);
        /// <summary>
        /// הוספת רחפן
        /// </summary>
        /// <param name="d">ישות לוגית של רחפן להוספה</param>
        /// <param name="stationId">מספר תחנת בסיס לטעינה ראשונית</param>
        public void AddDrone(DroneToList d, int stationId);
        /// <summary>
        /// הוספת לקוח
        /// </summary>
        /// <param name="c">ישות לוגית של לקוח להוספה</param>
        public void AddCustomer(Customer c);
        /// <summary>
        /// הוספת חבילה
        /// </summary>
        /// <param name="p">ישות לוגית של חבילה להוספה</param>
        /// <returns>מזהה חבילה</returns>
        public int AddParcel(Parcel p);
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
        /// פונקצית שליחת רחפן לטעינה
        /// </summary>
        /// <param name="id">מזהה רחפן</param>
        public void SendingDroneForCharging(int id);
        /// <summary>
        /// פונקציית שחרור רחפן מטעינה
        /// </summary>
        /// <param name="id">מזהה רחפן</param>
        /// <param name="chargingTime">פרק זמן בטעינה</param>
        public void ReleaseDroneFromCharging(int id, TimeSpan chargingTime);//מה זה פרק זמן בטעינה?
        /// <summary>
        /// פונקצית שיוך חבילה לרחפן 
        /// </summary>
        /// <param name="idDrone">מזהה רחפן</param>
        public void UpdateDroneToParcel(int idDrone);
        /// <summary>
        /// פונקציית איסוף חבילה ע"י רחפן 
        /// </summary>
        /// <param name="idDrone">מזהה רחפן</param>
        public void CollectionParcelFromDrone(int idDrone);
        /// <summary>
        /// אספקת חבילה ע"י רחפן
        /// </summary>
        /// <param name="idDrone">מזהה רחפן</param>
        public void DeliveryParcelByDrone(int idDrone);
        /// <summary>
        /// תצוגת תחנה
        /// </summary>
        /// <param name="id">מזהה תחנה</param>
        /// <returns>ישות לוגית של תחנה</returns>
        public Station GetStation(int id);
        /// <summary>
        /// תצוגת רחפן
        /// </summary>
        /// <param name="id">מזהה רחפן</param>
        /// <returns>ישות לוגית של רחפן</returns>
        public Drone GetDrone(int id);
        /// <summary>
        /// תצוגת לקוח
        /// </summary>
        /// <param name="id">מזהה לקוח</param>
        /// <returns>ישות לוגית של לקוח</returns>
        public Customer GetCustomer(int id);
        /// <summary>
        /// תצוגת חבילה
        /// </summary>
        /// <param name="id">מזהה חבילה</param>
        /// <returns>ישות לוגית של חבילה</returns>
        public Parcel GetParcel(int id);

        /// <summary>
        /// פונקציית תצוגת כל התחנות
        /// </summary>
        /// <returns></returns>
        IEnumerable<StationToList> GetStationList();

        /// <summary>
        /// פונקציית תצוגת כל הרחפנים
        /// </summary>
        /// <returns>רשימת כל הרחפנים</returns>
        IEnumerable<DroneToList> GetDroneList();

        /// <summary>
        /// פונקציית תצוגת כל הלקוחות
        /// </summary>
        /// <returns></returns>
        IEnumerable<CustomerToList> GetCustomerList();

        /// <summary>
        /// פונקציית תצוגת רשימת החבילות
        /// </summary>
        /// <returns>רשימת כל החבילות</returns>
        IEnumerable<ParcelToList> GetParcelList();

        /// <summary>
        /// פונקציית תצוגת חבילות שעוד לא שויכו לרחפן 
        /// </summary>
        /// <returns>רשימת חבילות שעוד לא שויכו לרחפן</returns>
        IEnumerable<ParcelToList> GetParcelNoDroneList();

        /// <summary>
        /// פונקציית תצוגת תחנות עם עמדות טעינה פנויות
        /// </summary>
        /// <returns>רשימת תחנות עם עמדות טעינה פנויות</returns>
        IEnumerable<StationToList> GetAvailableChargingStationsList();
        /// <summary>
        /// פונקצית תצוגת רשימת רחפנים לפי תנאי
        /// </summary>
        /// <param name="p">פרדיקט</param>
        /// <returns>רשימת רחפנים לפי תנאי</returns>
        public IEnumerable<DroneToList> GetPartOfDroneList(Predicate<DroneToList> p);
        public IEnumerable<ParcelToList> GetParcelList(Predicate<ParcelToList> predicate);
        public IEnumerable<DO.DroneCharge> GetDroneChargesList();

        /// <summary>
        /// מחיקת חבילה 
        /// </summary>
        /// <param name="parcel">חבילה למחיקה</param>
        public void DeleteParcel(Parcel parcel);

        public void StartDroneSimulator(int id, Action updateDrone, Func<bool> checkStop);
        public IEnumerable<UserToLIst> GetUSList();
        public void ChangePassword(string password, int id);


    }
}

