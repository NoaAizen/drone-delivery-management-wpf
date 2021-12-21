using DO;
using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DalObject
{
    internal sealed class DalObject : IDal
    {
        static readonly DalObject instance = new DalObject();//שדה פרטי סטטי שלsealed   
        internal static DalObject Instance { get => instance; }
        static DalObject() { }
        
        DalObject() 
        {
            DataSource.Config.Initialize();
        }

        /// <summary>
        /// בנאי
        /// </summary>
        //public DalObject()
        //{
        //    DataSource.Config.Initialize();
        //}


        //-----------------------------------ADD-----------------------------------

        #region Station
        /// <summary>
        //פונקצית הוספת תחנת בסיס לרשימת התחנות הקיימות 
        /// </summary>
        /// <param name="s"></param>
        public void AddStation(DO.Station s)
        {
            if (DataSource.listStations.Exists(x => x.Id == s.Id))
                throw new AlreadyExistException("The station already exist");
            DataSource.listStations.Add(s);
        }
        #endregion

        #region Drone
        /// <summary>
        /// פונקצית הוספת רחפן לרשימת רחפנים 
        /// </summary>
        /// <param name="d"></param>
        public void AddDrone(DO.Drone d)
        {
            if (DataSource.listDrones.Exists(x => x.Id == d.Id))
                throw new AlreadyExistException("The drone already exist");
            DataSource.listDrones.Add(d);
        }
        #endregion

        #region Customer
        /// <summary>
        ///  פונקציית קליטת לקוח חדש לרשימת הלקוחות 
        /// </summary>
        /// <param name="c"></param>
        public void AddCustomer(DO.Customer c)
        {
            if (DataSource.listCustomers.Exists(x => x.Id == c.Id))
                throw new AlreadyExistException("The customers already exist");
            DataSource.listCustomers.Add(c);
        }
        #endregion

        #region Parcel
        /// <summary>
        ///  פונקציית קליטת חבילה למשלוח
        /// </summary>
        /// <param name="p"></param>
        public int AddParcel(DO.Parcel p)
        {
            p.Id = DataSource.Config.CounterForParcels;
            p.Requested = DateTime.Now;
            p.Scheduled = null;
            p.PickedUp = null;
            p.Delivered = null;
            p.DroneId = 0;
            if (DataSource.listParcels.Exists(x => x.Id == p.Id))
                throw new AlreadyExistException("The parcels already exist");
            DataSource.Config.CounterForParcels++;//עדכון הרץ
            DataSource.listParcels.Add(p);
            return p.Id;
        }
        #endregion

        //-----------------------------------UPDATE-----------------------------------

        #region Drone
        /// <summary>
        /// עדכון מודל רחפן
        /// </summary>
        /// <param name="id">מזהה הרחפן לעדכון</param>
        /// <param name="model">שם המודל חדש</param>
        public void UpdateDroneModel(int id, string model)
        {
            if (!DataSource.listDrones.Exists(x => x.Id == id))
                throw new DoesntExistException("This drone doesn't exist");
            Drone d = DataSource.listDrones.Find(x => x.Id == id);
            DataSource.listDrones.Remove(d);
            d.Model = model;
            DataSource.listDrones.Add(d);
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
            if (!DataSource.listStations.Exists(x => x.Id == id))
                throw new DoesntExistException("This station doesn't exist");
            Station s = DataSource.listStations.Find(x => x.Id == id);
            DataSource.listStations.Remove(s);
            if (name != "")
                s.Name = name;
            if (totalChargingStations != 0)
                s.AvailableStations = totalChargingStations - GetDronesInStationId(id).Count();
            DataSource.listStations.Add(s);
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
            if (!DataSource.listCustomers.Exists(x => x.Id == id))
                throw new DoesntExistException("This customer doesn't exist");
            Customer c = DataSource.listCustomers.Find(x => x.Id == id);
            DataSource.listCustomers.Remove(c);
            if (name != "")
                c.Name = name;
            if (phone != "")
                c.Phone = phone;
            DataSource.listCustomers.Add(c);
        }
        #endregion

        #region Assignment
        /// <summary>
        /// פונקצית שיוך חבילה לרחפן 
        /// </summary>
        /// <param name="idDrone"></param>
        /// <param name="idParcel"></param>
        public void UpdateDroneToParcel(int idDrone, int idParcel)
        {
            if (!DataSource.listParcels.Exists(x => x.Id == idParcel))
                throw new DoesntExistException("This parcel doesn't exist");
            if (!DataSource.listDrones.Exists(x => x.Id == idDrone))
                throw new DoesntExistException("This drone doesn't exist");

            for (int i = 0; i < DataSource.listParcels.Count; i++)
            {
                if (DataSource.listParcels[i].Id == idParcel)
                {
                    DO.Parcel p = DataSource.listParcels[i];
                    p.DroneId = idDrone;
                    p.Scheduled = DateTime.Now;
                    DataSource.listParcels[i] = p;
                    break;

                }
            }
        }
        #endregion

        #region Collection
        /// <summary>
        /// פונקציית איסוף חבילה ע"י רחפן 
        /// </summary>
        /// <param name="idDrone"></param>
        /// <param name="idParcel"></param>
        public void CollectionParcelFromDrone(int idDrone, int idParcel)
        {
            if (!DataSource.listParcels.Exists(x => x.Id == idParcel))
                throw new DoesntExistException("This parcel doesn't exist");
            if (!DataSource.listDrones.Exists(x => x.Id == idDrone))
                throw new DoesntExistException("This drone doesn't exist");
            for (int i = 0; i < DataSource.listParcels.Count; i++)
            {
                if (DataSource.listParcels[i].Id == idParcel)
                {
                    DO.Parcel p = DataSource.listParcels[i];
                    p.PickedUp = DateTime.Now;
                    DataSource.listParcels[i] = p;
                    break;

                }
            }
            //for (int i = 0; i < DataSource.listDrones.Count; i++)//עדכון סטטוס של הרחפן שהוא לא פנוי
            //{

            //    if (DataSource.listDrones[i].Id == idDrone)
            //    {
            //        DalApi.DO.Drone d = DataSource.listDrones[i];
            //        //d.Status = (DAL.DalApi.DO.StatusDrone)2;
            //        DataSource.listDrones[i] = d;
            //        break;

            //    }
            //}
        }
        #endregion

        #region Delivery
        /// <summary>
        /// פונקציית אספקת חבילה ללקוח 
        /// </summary>
        /// <param name="idCustomer"></param>
        /// <param name="idParcel"></param>
        public void DeliveryParcelForCustomer(int idCustomer, int idParcel)
        {
            if (!DataSource.listParcels.Exists(x => x.Id == idParcel))
                throw new DoesntExistException("This parcel doesn't exist");
            if (!DataSource.listCustomers.Exists(x => x.Id == idCustomer))
                throw new DoesntExistException("This customer doesn't exist");
            int idDrone = 0;//בשביל שימוש בפרמטר הזה
            for (int i = 0; i < DataSource.listParcels.Count; i++)
            {
                if (DataSource.listParcels[i].Id == idParcel)
                {
                    DO.Parcel p = DataSource.listParcels[i];
                    p.Delivered = DateTime.Now;
                    //p.TargetId = idCustomer;
                    idDrone = p.DroneId;
                    DataSource.listParcels[i] = p;
                    break;

                }
            }
            for (int i = 0; i < DataSource.listDrones.Count; i++)//עדכון סטטוס
            {
                if (DataSource.listDrones[i].Id == idDrone)
                {
                    DO.Drone d = DataSource.listDrones[i];
                    //d.Status = (DAL.DalApi.DO.StatusDrone)0;
                    DataSource.listDrones[i] = d;
                    break;

                }
            }
        }
        #endregion

        #region Charging
        /// <summary>
        /// פונקציית שליחת רחפן לטעינה בתחנת בסיס
        /// </summary>
        /// <param name="idDrone"></param>
        /// <param name="idStation"></param>
        public void SendingDroneForCharging(int idDrone, int idStation)
        {
            if (!DataSource.listStations.Exists(x => x.Id == idStation))
                throw new DoesntExistException("This station doesn't exist");
            if (!DataSource.listDrones.Exists(x => x.Id == idDrone))
                throw new DoesntExistException("This drone doesn't exist");
            //for (int i = 0; i < DataSource.listDrones.Count; i++)//עדכון סטוטוס של הרחן
            //{
            //    if (DataSource.listDrones[i].Id == idDrone)
            //    {
            //        DalApi.DO.Drone d = DataSource.listDrones[i];
            //        //  d.Status = (DAL.DalApi.DO.StatusDrone)1;
            //        DataSource.listDrones[i] = d;
            //        break;
            //    }
            //}
            for (int i = 0; i < DataSource.listStations.Count; i++)//עדכון מספר תחנות הטענה פנויות
            {
                if (DataSource.listStations[i].Id == idStation)
                {
                    DO.Station s = DataSource.listStations[i];
                    s.AvailableStations -= 1;
                    DataSource.listStations[i] = s;
                    break;
                }
            }
            DO.DroneCharge dc = new() { DroneId = idDrone, StationId = idStation };
            DataSource.listDroneCharges.Add(dc);
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
            if (!DataSource.listStations.Exists(x => x.Id == idStation))
                throw new DoesntExistException("This station doesn't exist");
            if (!DataSource.listDrones.Exists(x => x.Id == idDrone))
                throw new DoesntExistException("This drone doesn't exist");
            //for (int i = 0; i < DataSource.listDrones.Count; i++)//  עדכון בטירה ועדכון סטטוס
            //{
            //    if (DataSource.listDrones[i].Id == idDrone)
            //    {
            //       DalApi.DO.Drone d = DataSource.listDrones[i];
            //        // d.Status = (DAL.DalApi.DO.StatusDrone)0;
            //        // d.Battery = 100;
            //        DataSource.listDrones[i] = d;
            //        break;
            //    }
            //}
            for (int i = 0; i < DataSource.listStations.Count; i++)//עדכון מספר תחנות הטענה פנויות
            {
                if (DataSource.listStations[i].Id == idStation)
                {
                    DO.Station s = DataSource.listStations[i];
                    s.AvailableStations += 1;
                    DataSource.listStations[i] = s;
                    break;
                }
            }
            for (int i = 0; i < DataSource.listDroneCharges.Count; i++)//עדכון של רשימת טעינת הסוללה 
            {
                if (DataSource.listDroneCharges[i].StationId == idStation &&
                    DataSource.listDroneCharges[i].DroneId == idDrone)
                {
                    DO.DroneCharge dc = DataSource.listDroneCharges[i];
                    DataSource.listDroneCharges.Remove(dc);
                    break;
                }
            }
        }
        #endregion

        //-----------------------------------REQUEST-----------------------------------

        #region Station
        /// <summary>
        /// תצוגת תחנה
        /// </summary>
        /// <param name="idStation">מזהה תחנה</param>
        /// <returns>תחנה</returns>
        public DO.Station GetStation(int idStation)
        {
            if (!DataSource.listStations.Exists(x => x.Id == idStation))
                throw new DoesntExistException("This station doesn't exist");
              
            return (from item in DataSource.listStations
                   where item.Id==idStation
                    select item).FirstOrDefault();
            
            
            //DO.Station s = new DO.Station();
            //for (int i = 0; i < DataSource.listStations.Count; i++)
            //{
            //    if (DataSource.listStations[i].Id == idStation)
            //    {
            //        s = DataSource.listStations[i];
            //        return s;

            //    }

            //}
            //return s;
        }
        #endregion

        #region Drone
        /// <summary>
        /// תצוגת רחפן
        /// </summary>
        /// <param name="idDrone">מזהה רחפן</param>
        /// <returns>רחפן</returns>
        public DO.Drone GetDrone(int idDrone)
        {
            if (!DataSource.listDrones.Exists(x => x.Id == idDrone))
                throw new DoesntExistException("This drone doesn't exist");
           
            return (from item in DataSource.listDrones
                    where item.Id == idDrone
                    select item).FirstOrDefault();



            //DO.Drone d = new DO.Drone();
            //for (int i = 0; i < DataSource.listDrones.Count; i++)
            //{
            //    if (DataSource.listDrones[i].Id == idDrone)
            //    {
            //        d = DataSource.listDrones[i];
            //        return d;

            //    }

            //}
            //return d;
        }
        #endregion

        #region Customer
        /// <summary>
        /// תצוגת לקוח
        /// </summary>
        /// <param name="idCustomer">מזהה לקוח</param>
        /// <returns>לקוח</returns>
        public DO.Customer GetCustomer(int idCustomer)
        {
            if (!DataSource.listCustomers.Exists(x => x.Id == idCustomer))
                throw new DoesntExistException("This customer doesn't exist");
            return (from item in DataSource.listCustomers
                    where item.Id == idCustomer
                    select item).FirstOrDefault();
            
            
            //DO.Customer c = new DO.Customer();
            //for (int i = 0; i < DataSource.listCustomers.Count; i++)
            //{
            //    if (DataSource.listCustomers[i].Id == idCustomer)
            //    {
            //        c = DataSource.listCustomers[i];
            //        return c;
            //    }
            //}
            //return c;
        }
        #endregion

        #region Parcel
        /// <summary>
        /// תצוגת חבילה
        /// </summary>
        /// <param name="idParcel">מזהה חבילה</param>
        /// <returns>חבילה</returns>
        public DO.Parcel GetParcel(int idParcel)//הדפסת חבילה
        {
            if (!DataSource.listParcels.Exists(x => x.Id == idParcel))
                throw new DoesntExistException("This parcel doesn't exist");
            return (from item in DataSource.listParcels
                    where item.Id == idParcel
                    select item).FirstOrDefault();

            //DO.Parcel p = new DO.Parcel();
            //for (int i = 0; i < DataSource.listParcels.Count; i++)
            //{
            //    if (DataSource.listParcels[i].Id == idParcel)
            //    {
            //        p = DataSource.listParcels[i];
            //        return p;

            //    }
            //}
            //return p;
        }
        #endregion

        //-----------------------------------LIST-REQUEST-----------------------------------

        #region Stations
        /// <summary>
        /// תצוגת כל התחנות
        /// </summary>
        /// <returns>רשימת כל התחנות</returns>
        public IEnumerable<DO.Station> GetStationList()
        {
            return (from item in DataSource.listStations
                    select item).ToList();
            //List<DalApi.DO.Station> temp = new List<DalApi.DO.Station>();

            //for (int i = 0; i < DataSource.listStations.Count; i++)
            //{

            //    temp.Add(DataSource.listStations[i]);
            //}
            //return temp;
        }
        #endregion

        #region Drones
        /// <summary>
        /// פונקציית תצוגת כל הרחפנים
        /// </summary>
        /// <returns>רשימת כל הרחפנים</returns>
        public IEnumerable<DO.Drone> GetDroneList()
        {

            return (from item in DataSource.listDrones
                    select item).ToList();

            //List<DO.Drone> temp = new List<DO.Drone>();

            //for (int i = 0; i < DataSource.listDrones.Count; i++)
            //{

            //    temp.Add(DataSource.listDrones[i]);
            //}
            //return temp;
        }
        #endregion

        #region Customers
        /// <summary>
        /// פונקציית תצוגת כל הלקוחות
        /// </summary>
        /// <returns>רשימת כל הלקוחות</returns>
        public IEnumerable<DO.Customer> GetCustomerList()
        {
            return (from item in DataSource.listCustomers
                    select item).ToList();
            //List<DO.Customer> temp = new List<DO.Customer>();

            //for (int i = 0; i < DataSource.listCustomers.Count; i++)
            //{

            //    temp.Add(DataSource.listCustomers[i]);
            //}
            //return temp;
        }
        #endregion

        #region Parcels
        /// <summary>
        /// פונקציית תצוגת כל החבילות
        /// </summary>
        /// <returns>רשימת כל החבילות</returns>
        public IEnumerable<DO.Parcel> GetParcelList()
        {
            return (from item in DataSource.listParcels
                    select item).ToList();
            //List<DO.Parcel> temp = new List<DO.Parcel>();

            //for (int i = 0; i < DataSource.listParcels.Count; i++)
            //{

            //    temp.Add(DataSource.listParcels[i]);
            //}
            //return temp;
        }
        #endregion

        #region DroneCharges
        /// <summary>
        /// תצוגת רשימת רחפנים בטעינה
        /// </summary>
        /// <returns>רשימת רחפנים בטעינה</returns>
        public IEnumerable<DroneCharge> GetDroneChargesList()
        {
            return (from item in DataSource.listDroneCharges
                    select item).ToList();
        }
        #endregion

        #region Parcels no drone
        /// <summary>
        /// פונקציית תצוגת חבילות שעוד לא שויכו לרחפן 
        /// </summary>
        /// <returns>רשימת חבילות שעוד לא שויכו לרחפן</returns>
        public IEnumerable<DO.Parcel> GetParcelNoDroneList()
        {
            //List<DO.Parcel> temp = new List<DO.Parcel>();

            return (from item in DataSource.listParcels
                    where item.DroneId==0
                    select item).ToList();


            //for (int i = 0; i < DataSource.listParcels.Count; i++)
            //{
            //    if (DataSource.listParcels[i].DroneId == 0)
            //    {
            //        temp.Add(DataSource.listParcels[i]);
            //    }

            //}
            //return temp;
        }
        #endregion

        #region Available charging stations
        /// <summary>
        /// פונקציית תצוגת תחנות עם עמדות טעינה פנויות
        /// </summary>
        /// <returns>רשימת תחנות עם עמדות טעינה פנויות</returns>
        public IEnumerable<DO.Station> GetAvailableChargingStationsList()
        {
            //List<DO.Station> temp = new List<DO.Station>();

            //for (int i = 0; i < DataSource.listStations.Count; i++)
            //{
            //    if (DataSource.listStations[i].AvailableStations > 0)
            //    {
            //        temp.Add(DataSource.listStations[i]);

            //    }
            //}
            //return temp;
            return (from item in DataSource.listStations
                    where item.AvailableStations >0
                    select item).ToList();
        }
        #endregion

        //-----------------------------------HELP-METHODS-----------------------------------

        #region GetDronesInStationId
        /// <summary>
        /// פונקצייה המחזירה רשימת מספרים מזהים של רחפנים הנמצאים בתחנה כלשהי
        /// </summary>
        /// <param name="stationId">מזהה תחנה</param>
        /// <returns>רשימת מזהי הרחפנים הנטענים בתחנה זו </returns>
        public IEnumerable<int> GetDronesInStationId(int stationId)
        {
            return (from item in DataSource.listDroneCharges
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
        public IEnumerable<Parcel> GetSenderParcels(int senderId)
        {
            return (from item in DataSource.listParcels
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
        public IEnumerable<Parcel> GetTargetParcels(int targetId)
        {
            return (from item in DataSource.listParcels
                    where item.TargetId == targetId
                    select item).ToList();
        }
        #endregion

        #region PowerRequestToDrone
        /// <summary>
        /// מתודת בקשת צריכת חשמל ע"י רחפן
        /// </summary>
        /// <returns>מערך של תכונות סטטיות עבור צריכת חשמל לק"מ ע"י רחפן</returns>
        public double[] PowerRequestToDrone()
        {
            double[] arr = new double[] { DataSource.Config.available, DataSource.Config.lightWeight,
                    DataSource.Config.mediumWeight, DataSource.Config.heavyWeight, DataSource.Config.chargingRate};
            return arr;
        }
        #endregion

        #region GetDroneList
        public IEnumerable<Drone> GetDroneList(Predicate<Drone> p)
        {
            return( from item in DataSource.listDrones
                    where p(item)
                    select item).ToList();
        }
        #endregion
    }
}


