using IDAL.DO;
using IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DalObject
{
    public class DalObject : IDal
    {

        /// <summary>
        /// בנאי
        /// </summary>
        public DalObject()
        {
            DataSource.Config.Initialize();
        }
        /// <summary>
        /// פונקצית  הוספת רחפן לרשימת הרחפנים הקיימים 
        /// </summary>
        /// <param name="s"></param>
        public void AddStation(IDAL.DO.Station s)
        {
            if (DataSource.listStations.Exists(x => x.Id == s.Id))
                throw new AlreadyExistException("The station already exist");
            DataSource.listStations.Add(s);
        }
        /// <summary>
        /// פונקצית הוספת רחפן לרשימת רחפנים 
        /// </summary>
        /// <param name="d"></param>
        public void AddDrone(IDAL.DO.Drone d)
        {
            if (DataSource.listDrones.Exists(x => x.Id == d.Id))
                throw new AlreadyExistException("The drone already exist");
            DataSource.listDrones.Add(d);
        }
        /// <summary>
        ///  פונקציית קליטת לקוח חדש לרשימת הלקוחות 
        /// </summary>
        /// <param name="c"></param>
        public void AddCustomer(IDAL.DO.Customer c)
        {
            if (DataSource.listCustomers.Exists(x => x.Id == c.Id))
                throw new AlreadyExistException("The customers already exist");
            DataSource.listCustomers.Add(c);
        }
        /// <summary>
        ///  פונקציית קליטת חבילה למשלוח
        /// </summary>
        /// <param name="p"></param>
        public int AddParcel(IDAL.DO.Parcel p)
        {
            p.Id = DataSource.Config.CounterForParcels;
            p.Requested = DateTime.Now;
            p.Scheduled = DateTime.MinValue;
            p.PickedUp = DateTime.MinValue;
            p.Delivered = DateTime.MinValue;
            p.DroneId = 0;
            if (DataSource.listParcels.Exists(x => x.Id == p.Id))
                throw new AlreadyExistException("The parcels already exist");
            DataSource.Config.CounterForParcels++;//עדכון הרץ
            DataSource.listParcels.Add(p);
            return p.Id;
        }
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
                    IDAL.DO.Parcel p = DataSource.listParcels[i];
                    p.DroneId = idDrone;
                    p.Scheduled = DateTime.Now;
                    DataSource.listParcels[i] = p;
                    break;

                }
            }
        }
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
                    IDAL.DO.Parcel p = DataSource.listParcels[i];
                    p.PickedUp = DateTime.Now;
                    DataSource.listParcels[i] = p;
                    break;

                }
            }
            //for (int i = 0; i < DataSource.listDrones.Count; i++)//עדכון סטטוס של הרחפן שהוא לא פנוי
            //{

            //    if (DataSource.listDrones[i].Id == idDrone)
            //    {
            //        IDAL.DO.Drone d = DataSource.listDrones[i];
            //        //d.Status = (DAL.IDAL.DO.StatusDrone)2;
            //        DataSource.listDrones[i] = d;
            //        break;

            //    }
            //}
        }
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
                    IDAL.DO.Parcel p = DataSource.listParcels[i];
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
                    IDAL.DO.Drone d = DataSource.listDrones[i];
                    //d.Status = (DAL.IDAL.DO.StatusDrone)0;
                    DataSource.listDrones[i] = d;
                    break;

                }
            }
        }
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
            //        IDAL.DO.Drone d = DataSource.listDrones[i];
            //        //  d.Status = (DAL.IDAL.DO.StatusDrone)1;
            //        DataSource.listDrones[i] = d;
            //        break;
            //    }
            //}
            for (int i = 0; i < DataSource.listStations.Count; i++)//עדכון מספר תחנות הטענה פנויות
            {
                if (DataSource.listStations[i].Id == idStation)
                {
                    IDAL.DO.Station s = DataSource.listStations[i];
                    s.AvailableStations -= 1;
                    DataSource.listStations[i] = s;
                    break;
                }
            }
            IDAL.DO.DroneCharge dc = new IDAL.DO.DroneCharge(idDrone, idStation);
            DataSource.listDroneCharges.Add(dc);
        }

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
            //       IDAL.DO.Drone d = DataSource.listDrones[i];
            //        // d.Status = (DAL.IDAL.DO.StatusDrone)0;
            //        // d.Battery = 100;
            //        DataSource.listDrones[i] = d;
            //        break;
            //    }
            //}
            for (int i = 0; i < DataSource.listStations.Count; i++)//עדכון מספר תחנות הטענה פנויות
            {
                if (DataSource.listStations[i].Id == idStation)
                {
                    IDAL.DO.Station s = DataSource.listStations[i];
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
                    IDAL.DO.DroneCharge dc = DataSource.listDroneCharges[i];
                    DataSource.listDroneCharges.Remove(dc);
                    break;
                }
            }
        }

        /// <summary>
        /// פונקציית להדפסה תחנה אחת
        /// </summary>
        /// <param name="idStation"></param>
        /// <returns></returns>
        public IDAL.DO.Station ViewStation(int idStation)//
        {
            if (!DataSource.listStations.Exists(x => x.Id == idStation))
                throw new DoesntExistException("This station doesn't exist");
            IDAL.DO.Station s = new IDAL.DO.Station();
            for (int i = 0; i < DataSource.listStations.Count; i++)
            {
                if (DataSource.listStations[i].Id == idStation)
                {
                    s = DataSource.listStations[i];
                    return s;

                }

            }
            return s;
        }
        /// <summary>
        /// פונקציית להדפסת רחפן אחת
        /// </summary>
        /// <param name="idDrone"></param>
        /// <returns></returns>
        public IDAL.DO.Drone ViewDrone(int idDrone)
        {
            if (!DataSource.listDrones.Exists(x => x.Id == idDrone))
                throw new DoesntExistException("This drone doesn't exist");
            IDAL.DO.Drone d = new IDAL.DO.Drone();
            for (int i = 0; i < DataSource.listDrones.Count; i++)
            {
                if (DataSource.listDrones[i].Id == idDrone)
                {
                    d = DataSource.listDrones[i];
                    return d;

                }

            }
            return d;
        }
        /// <summary>
        /// פונקציית הדפסת לקוח אחד
        /// </summary>
        /// <param name="idCustomer"></param>
        /// <returns></returns>
        public IDAL.DO.Customer ViewCustomer(int idCustomer)//
        {
            if (!DataSource.listCustomers.Exists(x => x.Id == idCustomer))
                throw new DoesntExistException("This customer doesn't exist");
            IDAL.DO.Customer c = new IDAL.DO.Customer();
            for (int i = 0; i < DataSource.listCustomers.Count; i++)
            {
                if (DataSource.listCustomers[i].Id == idCustomer)
                {
                    c = DataSource.listCustomers[i];
                    return c;
                }
            }
            return c;
        }
        /// <summary>
        /// הדפסת חבילה אחת
        /// </summary>
        /// <param name="idParcel"></param>
        /// <returns></returns>
        public IDAL.DO.Parcel ViewParcel(int idParcel)//הדפסת חבילה
        {
            if (!DataSource.listParcels.Exists(x => x.Id == idParcel))
                throw new DoesntExistException("This parcel doesn't exist");
            IDAL.DO.Parcel p = new IDAL.DO.Parcel();
            for (int i = 0; i < DataSource.listParcels.Count; i++)
            {
                if (DataSource.listParcels[i].Id == idParcel)
                {
                    p = DataSource.listParcels[i];
                    return p;

                }
            }
            return p;
        }
        /// <summary>
        /// פונמיתת הדפסת כל התחנות
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IDAL.DO.Station> ViewStationList()//
        {
            return (from item in DataSource.listStations
                    select item).ToList();
            //List<IDAL.DO.Station> temp = new List<IDAL.DO.Station>();

            //for (int i = 0; i < DataSource.listStations.Count; i++)
            //{

            //    temp.Add(DataSource.listStations[i]);
            //}
            //return temp;
        }
        /// <summary>
        /// פונקציית הדפסת כל הרפנים
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IDAL.DO.Drone> ViewDroneList()
        {
            List<IDAL.DO.Drone> temp = new List<IDAL.DO.Drone>();

            for (int i = 0; i < DataSource.listDrones.Count; i++)
            {

                temp.Add(DataSource.listDrones[i]);
            }
            return temp;
        }
        /// <summary>
        /// פונקציית הדפסת כל לקוחות
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IDAL.DO.Customer> ViewCustomerList()
        {
            List<IDAL.DO.Customer> temp = new List<IDAL.DO.Customer>();

            for (int i = 0; i < DataSource.listCustomers.Count; i++)
            {

                temp.Add(DataSource.listCustomers[i]);
            }
            return temp;
        }
        /// <summary>
        /// פונקציית הדפסת כל חבילות
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IDAL.DO.Parcel> ViewParcelList()
        {
            List<IDAL.DO.Parcel> temp = new List<IDAL.DO.Parcel>();

            for (int i = 0; i < DataSource.listParcels.Count; i++)
            {

                temp.Add(DataSource.listParcels[i]);
            }
            return temp;
        }
        /// <summary>
        /// תצוגת רשימת רחפנים בטעינה
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneCharge> GetDroneChargesList()
        {
            return (from item in DataSource.listDroneCharges
                    select item).ToList();
        }
        /// <summary>
        /// פונקציית הדפסת  חבילות שעוד לא שויכו לרחפן 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IDAL.DO.Parcel> ViewParcelNoDroneList()
        {
            List<IDAL.DO.Parcel> temp = new List<IDAL.DO.Parcel>();

            for (int i = 0; i < DataSource.listParcels.Count; i++)
            {
                if (DataSource.listParcels[i].DroneId == 0)
                {
                    temp.Add(DataSource.listParcels[i]);
                    break;

                }

            }
            return temp;
        }
        /// <summary>
        /// פונקציית הדפסת תחנות עם עמדות טעינה פנויות
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IDAL.DO.Station> ViewAvailableChargingStationsList()
        {
            List<IDAL.DO.Station> temp = new List<IDAL.DO.Station>();

            for (int i = 0; i < DataSource.listStations.Count; i++)
            {
                if (DataSource.listStations[i].AvailableStations > 0)
                {
                    temp.Add(DataSource.listStations[i]);
                    break;
                }
            }
            return temp;
        }
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

    }
}


