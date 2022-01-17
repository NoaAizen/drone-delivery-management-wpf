using System;
using BO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using System.Runtime.CompilerServices;


namespace BL
{
    internal sealed class BL : BlApi.IBL
    {
        static readonly BL instance = new BL();//שדה פרטי סטטי 
        internal static BL Instance { get => instance; }
        internal List<DroneToList> DronesList = new List<DroneToList>();//רשימת רחפנים
        internal IDal dalObj;
        private static Random r;
        private double available;//פנוי
        private double lightWeight; //נושא משקל קל
        private double mediumWeight;//נושא משקל בינוני
        private double heavyWeight;//נושא משקל כבד
        private double chargingRate;//קצב טעינת רחפן - % בשעה

        static BL() { }

        #region constructor
        /// <summary>
        /// בנאי
        /// </summary>
        BL()
        {
            r = new Random();
            dalObj = DalFactory.GetDal("2");
            DO.Parcel parcel = new();
            StatusDrone status = 0;
            Location location = new();
            double battery = 0;
            int stationId = 0;
            double[] arr = dalObj.PowerRequestToDrone();
            available = arr[0];
            lightWeight = arr[1];
            mediumWeight = arr[2];
            heavyWeight = arr[3];
            chargingRate = arr[4];
            List<DO.Drone> drones = (List<DO.Drone>)dalObj.GetDroneList();
            List<DO.Parcel> parcels = (List<DO.Parcel>)dalObj.GetParcelList();
            foreach (var drone in drones)
            {
                if (parcels.Exists(x => x.DroneId == drone.Id))
                {//הרחפן במשלוח
                    parcel = (from item in parcels
                              where item.DroneId == drone.Id
                              select item).FirstOrDefault();
                    if (parcel.Scheduled != null && parcel.Delivered == null)//חבילה שעוד לא סופקה אך הרחפן כבר שויך
                    {
                        status = (StatusDrone)2;
                        if (parcel.PickedUp == null)//החבילה שויכה אך לא נאספה
                        {
                            stationId = findClosestStationToCustomer(parcel.SenderId);
                            location = findStationLocation(stationId);
                        }
                        else
                        {
                            location = findCustomerLocation(parcel.SenderId);
                        }
                        battery = r.NextDouble() * (100 - 50) + 50;//הגרלת סוללה בין 50 ל100
                    }
                    else
                    {//הרחפן לא במשלוח
                        status = (StatusDrone)r.Next(0, 2);
                        if (status == 0)//הרחפן פנוי
                        {
                            location = getRandomCustomerLocation();
                            battery = r.NextDouble() * (100 - 50) + 50;//הגרלת סוללה בין 50 ל100
                        }
                        else//הרחפן בתחזוקה
                        {
                            lock (dalObj)
                            {
                                stationId = getRandomStation();
                                location = findStationLocation(stationId);
                                dalObj.SendingDroneForCharging(drone.Id, stationId);
                                battery = r.NextDouble() * (20 - 0) + 0;
                            }
                        }
                    }
                }
                else
                {//הרחפן לא במשלוח
                    status = (StatusDrone)r.Next(0, 2);
                    if (status == 0)//הרחפן פנוי
                    {
                        location = getRandomCustomerLocation();
                        battery = r.NextDouble() * (100 - 50) + 50;//הגרלת סוללה בין 50 ל100
                    }
                    else//הרחפן בתחזוקה
                    {
                        lock (dalObj)
                        {
                            stationId
                                = getRandomStation();
                            location = findStationLocation(stationId);
                            dalObj.SendingDroneForCharging(drone.Id, stationId);
                            battery = r.NextDouble() * (20 - 0) + 0;
                        }
                    }
                }
                DroneToList blDrone = new()
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = (WeightCategories)drone.MaxWeight,
                    Status = status,
                    Battery = battery,
                    //ParcelInTransfer= GetDrone(drone.Id).ParcelInTransfer,
                    CurrentLocation = location,
                    //ParcelTransferredNumber= GetDrone(drone.Id).ParcelInTransfer.Id
                };
                DronesList.Add(blDrone);
            }
        }
        #endregion

        #region StartDroneSimulator
        public void StartDroneSimulator(int id, Action updateDrone, Func<bool> checkStop)
        {
            new Simulator(this, id, updateDrone, checkStop);

        }
        #endregion


        //-----------------------------------ADD-----------------------------------

        #region Station
        /// <summary>
        /// הוספת תחנת בסיס
        /// </summary>
        /// <param name="s">ישות לוגית של תחנה להוספה</param>
        public void AddStation(Station s)
        {
            DO.Station dalStation = new()//יצירת ישות נתונים של תחנה
            {
                Id = s.Id,
                Name = s.Name,
                Longitude = s.Location.Longitude,
                Latitude = s.Location.Latitude,
                AvailableStations = s.AvailableStations
            };
            try
            {
                lock (dalObj)
                {
                    dalObj.AddStation(dalStation);
                }
            }
            catch (Exception ex)
            {
                throw new AlreadyExistException(ex.Message, ex);
            }
        }
        #endregion

        #region Drone
        /// <summary>
        /// הוספת רחפן
        /// </summary>
        /// <param name="d">ישות לוגית של רחפן להוספה</param>
        /// <param name="stationId">מספר תחנת בסיס לטעינה ראשונית</param>
        public void AddDrone(DroneToList d, int stationId)
        {
            DO.Drone dalDrone = new()//יצירת ישות נתונים של רחפן
            {
                Id = d.Id,
                Model = d.Model,
                MaxWeight = (DO.WeightCategories)d.MaxWeight
            };
            try
            {
                lock (dalObj)
                {
                    dalObj.AddDrone(dalDrone);
                }
            }
            catch (Exception ex)
            {
                throw new AlreadyExistException(ex.Message, ex);
            }
            //random.NextDouble() * (maximum - minimum) + minimum
            d.Battery = r.NextDouble() * (40 - 20) + 20;
            d.Status = (StatusDrone)1;
            d.CurrentLocation = GetStation(stationId).Location;
            dalObj.SendingDroneForCharging(d.Id, stationId);
            DronesList.Add(d);
        }
        #endregion

        #region Customer
        /// <summary>
        /// הוספת לקוח
        /// </summary>
        /// <param name="c">ישות לוגית של לקוח להוספה</param>
        public void AddCustomer(Customer c)
        {
            DO.Customer dalCustomer = new()//יצירת ישות נתונים של לקוח
            {
                Id = c.Id,
                Name = c.Name,
                Phone = c.Phone,
                Longitude = c.Location.Longitude,
                Latitude = c.Location.Latitude
            };
            try
            {
                lock (dalObj)
                {
                    dalObj.AddCustomer(dalCustomer);
                }
            }
            catch (Exception ex)
            {
                throw new AlreadyExistException(ex.Message, ex);
            }
        }
        #endregion

        #region Parcel
        /// <summary>
        /// הוספת חבילה
        /// </summary>
        /// <param name="p">ישות לוגית של חבילה להוספה</param>
        /// <returns>מזהה חבילה</returns>
        public int AddParcel(Parcel p)
        {
            DO.Parcel dalParcel = new()//יצירת ישות נתונים של חבילה
            {
                SenderId = p.CustomerInParcelSender.Id,
                TargetId = p.CustomerInParcelRecipient.Id,
                Weight = (DO.WeightCategories)p.Weight,
                Priority = (DO.Priorities)p.Priority
            };
            try
            {
                lock (dalObj)
                {
                    return dalObj.AddParcel(dalParcel);
                }
            }
            catch (Exception ex)
            {
                throw new AlreadyExistException(ex.Message, ex);
            }
        }
        #endregion

        #region User
        public void AddUser(UserToLIst userToLIst)
        {
            DO.User user = new()
            {
                Id = userToLIst.Id,
                Name = userToLIst.Name,
                Password = userToLIst.Password
            };
            try
            {
                dalObj.AddUser(user);
            }
            catch (Exception ex)
            {
                throw new AlreadyExistException(ex.Message, ex);
            }
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
            try
            {
                lock (dalObj)
                {
                    dalObj.UpdateDroneModel(id, model);
                }
            }
            catch (Exception ex)
            {
                throw new DoesntExistException(ex.Message, ex);
            }
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
            try
            {
                lock (dalObj)
                {
                    dalObj.UpdateStation(id, name, totalChargingStations);
                }
            }
            catch (Exception ex)
            {
                throw new DoesntExistException(ex.Message, ex);
            }
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
            try
            {
                lock (dalObj)
                {
                    dalObj.UpdateCustomer(id, name, phone);
                }
            }
            catch (Exception ex)
            {
                throw new DoesntExistException(ex.Message, ex);
            }
        }
        #endregion

        #region Charging
        /// <summary>
        /// פונקצית שליחת רחפן לטעינה
        /// </summary>
        /// <param name="id">מזהה רחפן</param>
        public void SendingDroneForCharging(int id)
        {
            DroneToList drone = DronesList.FirstOrDefault(x => x.Id == id);
            if (drone == null)
                throw new DoesntExistException("This drone doesn't exist");
            if (drone.Status == 0)//אם הרחפן פנוי
            {
                int stationId = findClosestStationWithAvailableChargeSlots(id);
                Location stationLocation = findStationLocation(stationId);
                double distance = getDistance(drone.CurrentLocation, stationLocation);
                double minCharge = getMinCharge(id, distance);
                if (drone.Battery < minCharge)//אם אין מספיק סוללה
                    throw new ActionProblemException
                        ("Can't sending drone for charging, there isn't enough battery");
                try
                {
                    lock (dalObj)
                    {
                        dalObj.SendingDroneForCharging(id, stationId);
                    }
                }
                catch (Exception ex)
                {
                    throw new DoesntExistException(ex.Message, ex);
                }
                drone.Status = StatusDrone.Maintenance;
                drone.Battery -= minCharge;
                drone.CurrentLocation = stationLocation;
            }
            else
                throw new ActionProblemException
                    ("Can't sending drone for charging, only available drone can be sent for charging");
        }
        #endregion

        #region Release
        /// <summary>
        /// פונקציית שחרור רחפן מטעינה
        /// </summary>
        /// <param name="id">מזהה רחפן</param>
        /// <param name="chargingTime">פרק זמן בטעינה</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReleaseDroneFromCharging(int id, TimeSpan chargingTime)//מה זה פרק זמן בטעינה?
        {
            DroneToList drone = DronesList.FirstOrDefault(x => x.Id == id);
            if (drone == null)
                throw new DoesntExistException("This drone doesn't exist");
            if (drone.Status != StatusDrone.Maintenance)
                throw new ActionProblemException
                    ("Can't release drone from charging, only maintenance drone can be released");

            int stationId = dalObj.GetDroneChargesList().ToList().Find(x => x.DroneId == id).StationId;
            try
            {
                lock (dalObj)
                {
                    dalObj.ReleaseDroneFromCharging(id, stationId);
                }
            }
            catch (Exception ex)
            {
                throw new DoesntExistException(ex.Message, ex);
            }
            drone.Battery = 100;
            drone.Status = 0;
        }
        #endregion

        #region Assignment
        /// <summary>
        /// פונקצית שיוך חבילה לרחפן 
        /// </summary>
        /// <param name="idDrone">מזהה רחפן</param>
        public void UpdateDroneToParcel(int idDrone)
        {
            DroneToList drone = DronesList.FirstOrDefault(x => x.Id == idDrone);
            if (drone == null)
                throw new DoesntExistException("This drone doesn't exist");
            if (drone.Status != StatusDrone.Available)
                throw new ActionProblemException
                    ("Can't assignment parcel to drone, only available drone can be assignment");
            DO.Priorities priority = DO.Priorities.Emergency;
            List<DO.Parcel> parcels = getRelevantParcels();
            List<DO.Parcel> highPriorityParcels = getHighPriorityParcels(parcels, priority);
            DO.WeightCategories maxWeight = (DO.WeightCategories)drone.MaxWeight;
            List<DO.Parcel> maxWeightParcels = getMaxWeightParcels(highPriorityParcels, maxWeight);
            while (maxWeightParcels == null && priority != DO.Priorities.Normal)
            {
                priority--;
                highPriorityParcels = getHighPriorityParcels(parcels, priority);
                maxWeightParcels = getMaxWeightParcels(highPriorityParcels, maxWeight);
            }
            DO.Parcel parcel = getClosestParcel(maxWeightParcels, drone.CurrentLocation);
            double power = checkDeliveryDronePowerConsumption(idDrone);
            Location senderLocation = findCustomerLocation(parcel.SenderId);
            double distanceToSender = getDistance(drone.CurrentLocation, senderLocation);
            double minChargeToSender = getMinCharge(idDrone, distanceToSender);
            Location targetLocation = findCustomerLocation(parcel.TargetId);
            double distanceToTarget = getDistance(senderLocation, targetLocation);
            double minChargeToTarget = power * distanceToTarget;
            int stationId = findClosestStationToCustomer(parcel.TargetId);
            Location stationLocation = findStationLocation(stationId);
            double distanceToStation = getDistance(targetLocation, stationLocation);
            double minChargeToStation = getMinCharge(idDrone, distanceToStation);
            double minCharge = minChargeToSender + minChargeToTarget + minChargeToStation;
            if (drone.Battery < minCharge)
                throw new ActionProblemException
                        ("Can't assignment parcel to drone, there isn't enough battery");
            lock (dalObj)
            {
                dalObj.UpdateDroneToParcel(drone.Id, parcel.Id);
            }
            drone.Status = StatusDrone.Delivery;
            drone.ParcelInTransfer = GetDrone(idDrone).ParcelInTransfer;
            drone.ParcelTransferredNumber = drone.ParcelInTransfer.Id;
        }

        #region getRelevantParcels
        /// <summary>
        /// פונקצייה המחזירה רק חבילות שהוגדרו אבל לא שוייכו עדיין
        /// </summary>
        /// <returns>רשימה של חבילות שהוגדרו אך לא שויכו</returns>
        private List<DO.Parcel> getRelevantParcels()
        {
            List<DO.Parcel> parcels = new();
            StatusParcel status;
            lock (dalObj)
            {
                foreach (var item in dalObj.GetParcelList())
                {
                    status = checkParcelStatus(item);
                    if (status == StatusParcel.Defined)
                        parcels.Add(item);
                }
            }
            return parcels;
        }
        #endregion

        #region getMinCharge
        /// <summary>
        /// פונקצייה הבודקת בטרייה מינמלית שהרחפן זקוק לו
        /// </summary>
        /// <param name="id">מזהה רחפן</param>
        /// <param name="distance">מרחק שעל הרחפן לעבור</param>
        /// <returns></returns>
        private double getMinCharge(int id, double distance)
        {
            return checkDronePowerConsumption(id) * distance;
        }
        #endregion

        #region checkDronePowerConsumption
        /// <summary>
        /// בדיקת צריכת חשמל של רחפן
        /// </summary>
        /// <param name="id">מזהה רחפן</param>
        /// <returns>צריכת חשמל בהתאם למצבו ומשקלו</returns>
        private double checkDronePowerConsumption(int id)
        {
            Drone drone = GetDrone(id);
            if (drone.Status == StatusDrone.Available)
                return available;
            else return checkDeliveryDronePowerConsumption(id);
        }
        #endregion

        #region checkDeliveryDronePowerConsumption
        /// <summary>
        /// בדיקת צריכת חשמל של רחפן עבור רחפן שאינו פנוי
        /// </summary>
        /// <param name="id">מזהה רחפן</param>
        /// <returns>צריכת חשמל בהתאם למשקלו</returns>
        private double checkDeliveryDronePowerConsumption(int id)
        {
            Drone drone = GetDrone(id);
            if (drone.MaxWeight == WeightCategories.Light)
                return lightWeight;
            if (drone.MaxWeight == WeightCategories.Medium)
                return mediumWeight;
            return heavyWeight;
        }
        #endregion

        #region getClosestParcel
        /// <summary>
        /// פונקצייה הבודקת מי החבילה הקרובה ביותר לרחפן כלשהו
        /// </summary>
        /// <param name="parcels">רשימת חבילות</param>
        /// <param name="droneLocation">מיקום הרחפן</param>
        /// <returns>החבילה הקרובה ביותר</returns>
        private DO.Parcel getClosestParcel(List<DO.Parcel> parcels, Location droneLocation)
        {
            double distance;
            Location parcelLocation = findCustomerLocation(parcels[0].SenderId);
            double minDistance = getDistance(droneLocation, parcelLocation);
            DO.Parcel parcel = parcels[0];
            parcels.RemoveAt(0);
            foreach (var item in parcels)
            {
                distance = getDistance(droneLocation, parcelLocation);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    parcel = item;
                }
            }
            return parcel;
        }
        #endregion

        #region getMaxWeightParcels
        /// <summary>
        /// פונקצייה הבודקת מיהן החבילות
        /// בעלות המשקל המקסילי שרחפן מסוגל לשאת
        /// </summary>
        /// <param name="parcels">רשימה של חבילות</param>
        /// <param name="maxWeight">משקל מקסימלי שרחפן מסוגל לשאת</param>
        /// <returns>רשימת חבילות בעלות משקל מקסימלי שהרחפן מסוגל לשאת</returns>
        private List<DO.Parcel> getMaxWeightParcels(List<DO.Parcel> parcels, DO.WeightCategories maxWeight)
        {
            while (maxWeight != DO.WeightCategories.Light)//בדיקה האם קיימת חבילה במשקל מקסימלי כמו הרחפן ואם לא אז במשקל נמוך יותר
            {
                if (!parcels.Exists(x => x.Weight == maxWeight))
                    maxWeight--;
                else
                    break;
            }
            return (from item in parcels
                    where item.Weight == maxWeight
                    select item).ToList();
        }
        #endregion

        #region getHighPriorityParcels
        /// <summary>
        /// פונקצייה המחזירה את רשימת החבילות בעלות העדיפות הגבוהה ביותר
        /// </summary>
        /// <param name="parcels">רשימה של חבילות</param>
        /// <param name="priority">עדיפות מקסימלית</param>
        /// <returns>רשימה של חבילות עם העדיפות המקסימלית</returns>
        private List<DO.Parcel> getHighPriorityParcels(List<DO.Parcel> parcels, DO.Priorities priority)
        {
            while (priority != DO.Priorities.Normal)
            {
                if (!parcels.Exists(x => x.Priority == priority))
                    priority--;
                else
                    break;
            }
            return (from item in parcels
                    where item.Priority == priority
                    select item).ToList();
        }
        #endregion

        #endregion

        #region Collection
        /// <summary>
        /// פונקציית איסוף חבילה ע"י רחפן 
        /// </summary>
        /// <param name="idDrone">מזהה רחפן</param>
        public void CollectionParcelFromDrone(int idDrone)
        {
            DroneToList drone = DronesList.FirstOrDefault(x => x.Id == idDrone);
            List<DO.Parcel> parcels = (List<DO.Parcel>)dalObj.GetParcelList();
            if (drone == null)
                throw new DoesntExistException("This drone doesn't exist");
            if (drone.Status != StatusDrone.Delivery)
                throw new ActionProblemException
                ("Can't collection parcel, only delivery drone can collection");
            if (parcels.Exists(x => x.DroneId == idDrone && x.Scheduled != null && x.PickedUp == null))
            {//הרחפן במשלוח
                DO.Parcel parcel = (from item in parcels
                                    where item.DroneId == idDrone &&
                                    item.Scheduled != null && item.PickedUp == null
                                    select item).FirstOrDefault();
                try
                {
                    lock (dalObj)
                    {
                        dalObj.CollectionParcelFromDrone(idDrone, parcel.Id);
                    }
                }
                catch (Exception ex)
                {
                    throw new DoesntExistException(ex.Message, ex);
                }
                Location senderLocation = findCustomerLocation(parcel.SenderId);
                double distance = getDistance(drone.CurrentLocation, senderLocation);
                double minCharge = getMinCharge(idDrone, distance);
                drone.Battery -= minCharge;
                drone.CurrentLocation = senderLocation;
            }
            else
                throw new ActionProblemException("Can't collection parcel");
        }
        #endregion

        #region Delivery
        /// <summary>
        /// אספקת חבילה ע"י רחפן
        /// </summary>
        /// <param name="idDrone">מזהה רחפן</param>
        public void DeliveryParcelByDrone(int idDrone)
        {
            DroneToList drone = DronesList.FirstOrDefault(x => x.Id == idDrone);
            List<DO.Parcel> parcels = (List<DO.Parcel>)dalObj.GetParcelList();
            if (drone == null)
                throw new DoesntExistException("This drone doesn't exist");
            if (drone.Status != StatusDrone.Delivery)
                throw new ActionProblemException
                ("Can't delivery parcel, only delivery drone can delivery");
            if (parcels.Exists(x => x.DroneId == idDrone && x.PickedUp != null && x.Delivered == null))
            {//הרחפן במשלוח
                DO.Parcel parcel = (from item in parcels
                                    where item.DroneId == idDrone &&
                                    item.PickedUp != null && item.Delivered == null
                                    select item).FirstOrDefault();
                try
                {
                    lock (dalObj)
                    {
                        dalObj.DeliveryParcelForCustomer(parcel.TargetId, parcel.Id);
                    }
                }
                catch (Exception ex)
                {
                    throw new DoesntExistException(ex.Message, ex);
                }
                Location senderLocation = findCustomerLocation(parcel.SenderId);
                Location targetLocation = findCustomerLocation(parcel.TargetId);
                double distance = getDistance(senderLocation, targetLocation);
                double minCharge = getMinCharge(idDrone, distance);
                drone.Battery -= minCharge;
                drone.CurrentLocation = targetLocation;
                drone.Status = 0;
            }
            else
                throw new ActionProblemException("Can't delivery parcel");
        }
        #endregion

        #region ChangePassword
        public void ChangePassword(string password, int id)
        {
            try
            {
                dalObj.ChangePassword(password, id);
            }
            catch (Exception ex)
            {
                throw new DoesntExistException(ex.Message, ex);
            }
        }
        #endregion

        //-----------------------------------REQUEST-----------------------------------

        #region Station
        /// <summary>
        /// תצוגת תחנה
        /// </summary>
        /// <param name="id">מזהה תחנה</param>
        /// <returns>ישות לוגית של תחנה</returns>
        public Station GetStation(int id)
        {
            DO.Station dalStation;
            try
            {
                dalStation = dalObj.GetStation(id);
            }
            catch (Exception ex)
            {
                throw new DoesntExistException(ex.Message, ex);
            }
            Station blStation = new()
            {
                Id = dalStation.Id,
                Name = dalStation.Name,
                AvailableStations = dalStation.AvailableStations,
                Location = new() { Longitude = dalStation.Longitude, Latitude = dalStation.Latitude },
                DroneInChargingsList = dalObj.GetDronesInStationId(id)
                     .Select(droneId => new DroneInCharging() { Id = droneId, Battery = getDroneBattery(droneId) }).ToList()
            };
            return blStation;
        }
        #endregion

        #region Drone
        /// <summary>
        /// תצוגת רחפן
        /// </summary>
        /// <param name="id">מזהה רחפן</param>
        /// <returns>ישות לוגית של רחפן</returns>
        public Drone GetDrone(int id)
        {
            DroneToList drone = DronesList.Find(x => x.Id == id);
            DO.Drone dalDrone;
            try
            {
                dalDrone = dalObj.GetDrone(id);

                Drone blDrone = new()
                {
                    Id = dalDrone.Id,
                    Model = dalDrone.Model,
                    MaxWeight = (WeightCategories)dalDrone.MaxWeight,
                    Status = drone.Status,
                    Battery = drone.Battery,
                    CurrentLocation = drone.CurrentLocation
                };
                if (blDrone.Status == StatusDrone.Delivery)
                {
                    DO.Parcel dalParcel = dalObj.GetParcelList().ToList().Find(x => x.DroneId == id && x.Delivered == null);

                    Parcel parcel = GetParcel(dalParcel.Id);

                    blDrone.ParcelInTransfer = new()
                    {
                        Id = parcel.Id,
                        Weight = parcel.Weight,
                        Priority = parcel.Priority,
                        ParcelStatus = checkParcelStatus(dalParcel) == StatusParcel.Collected,// true אם בדרך ליעד
                        CustomerInParcelSender = parcel.CustomerInParcelSender,
                        CustomerInParcelRecipient = parcel.CustomerInParcelRecipient,
                        CollectionLocation = findCustomerLocation(parcel.CustomerInParcelSender.Id),
                        DeliveryDestinationLocation = findCustomerLocation(parcel.CustomerInParcelRecipient.Id)
                    };
                    blDrone.ParcelInTransfer.TransportDistance =
                        getDistance(blDrone.ParcelInTransfer.CollectionLocation, blDrone.ParcelInTransfer.DeliveryDestinationLocation);
                }
                return blDrone;
            }
            catch (Exception ex)
            {
                throw new DoesntExistException(ex.Message, ex);
            }
        }
        #endregion

        #region Customer
        /// <summary>
        /// תצוגת לקוח
        /// </summary>
        /// <param name="id">מזהה לקוח</param>
        /// <returns>ישות לוגית של לקוח</returns>
        public Customer GetCustomer(int id)
        {
            DO.Customer dalCustomer;
            try
            {
                dalCustomer = dalObj.GetCustomer(id);
            }
            catch (Exception ex)
            {
                throw new DoesntExistException(ex.Message, ex);
            }
            Customer blCustomer = new()
            {
                Id = dalCustomer.Id,
                Name = dalCustomer.Name,
                Phone = dalCustomer.Phone,
                Location = new() { Longitude = dalCustomer.Longitude, Latitude = dalCustomer.Latitude },
                ParcelAtCustomerFromCustomer = dalObj.GetSenderParcels(id)
                .Select(parcel => new ParcelAtCustomer()
                {
                    Id = parcel.Id,
                    Weight = (WeightCategories)parcel.Weight,
                    Priority = (Priorities)parcel.Priority,
                    StatusParcel = checkParcelStatus(parcel),
                    CustomerInParcel = new() { Id = parcel.TargetId, Name = dalObj.GetCustomer(parcel.TargetId).Name }
                }).ToList(),
                ParcelAtCustomerToCustomer = dalObj.GetTargetParcels(id)
                .Select(parcel => new ParcelAtCustomer()
                {
                    Id = parcel.Id,
                    Weight = (WeightCategories)parcel.Weight,
                    Priority = (Priorities)parcel.Priority,
                    StatusParcel = checkParcelStatus(parcel),
                    CustomerInParcel = new() { Id = parcel.SenderId, Name = dalObj.GetCustomer(parcel.SenderId).Name }
                }).ToList()
            };
            return blCustomer;
        }
        #endregion

        #region Parcel
        /// <summary>
        /// תצוגת חבילה
        /// </summary>
        /// <param name="id">מזהה חבילה</param>
        /// <returns>ישות לוגית של חבילה</returns>
        public Parcel GetParcel(int id)
        {
            DO.Parcel dalParcel;
            try
            {
                dalParcel = dalObj.GetParcel(id);
            }
            catch (Exception ex)
            {
                throw new DoesntExistException(ex.Message, ex);
            }
            Parcel blParcel = new()
            {
                Id = dalParcel.Id,
                CustomerInParcelSender = new() { Id = dalParcel.SenderId, Name = getCustomerName(dalParcel.SenderId) },
                CustomerInParcelRecipient = new() { Id = dalParcel.TargetId, Name = getCustomerName(dalParcel.TargetId) },
                Weight = (WeightCategories)dalParcel.Weight,
                Priority = (Priorities)dalParcel.Priority
            };
            if (dalParcel.Scheduled != null)
            {
                blParcel.DroneInParcel = new()
                {
                    Id = dalParcel.DroneId,
                    Battery = getDroneBattery(dalParcel.DroneId),
                    CurrentLocation = getDroneLocation(dalParcel.DroneId)
                };
            }
            blParcel.Requested = dalParcel.Requested;
            blParcel.Scheduled = dalParcel.Scheduled;
            blParcel.PickedUp = dalParcel.PickedUp;
            blParcel.Delivered = dalParcel.Delivered;
            return blParcel;
        }
        #endregion

        //-----------------------------------LIST-REQUEST-----------------------------------

        #region Stations
        /// <summary>
        /// פונקציית תצוגת כל התחנות
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StationToList> GetStationList()
        {
            List<StationToList> stations = new();
            foreach (var station in dalObj.GetStationList())
            {
                StationToList blStation = new()
                {
                    Id = station.Id,
                    Name = station.Name,
                    AvailableStations = station.AvailableStations,
                    NotAvailableStations = dalObj.GetDronesInStationId(station.Id).Count()
                };
                stations.Add(blStation);
            }
            return stations;
        }
        #endregion

        #region Drones
        /// <summary>
        /// פונקציית תצוגת כל הרחפנים
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneToList> GetDroneList()
        {
            DO.Parcel dalParcel;
            DroneToList droneInList;
            List<DroneToList> drones = new();
            foreach (var drone in dalObj.GetDroneList())
            {
                droneInList = DronesList.Find(x => x.Id == drone.Id);
                DroneToList blDrone = new()
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = (WeightCategories)drone.MaxWeight,
                    Status =  droneInList.Status,
                    Battery = droneInList.Battery,
                    CurrentLocation =  droneInList.CurrentLocation
                };
                if (blDrone.Status == StatusDrone.Delivery)
                {
                    dalParcel = dalObj.GetParcelList().ToList().Find(x => x.DroneId == blDrone.Id);
                    Parcel parcel = GetParcel(dalParcel.Id);
                    blDrone.ParcelInTransfer = new()
                    {
                        Id = parcel.Id,
                        Weight = parcel.Weight,
                        Priority = parcel.Priority,
                        ParcelStatus = checkParcelStatus(dalParcel) == StatusParcel.Collected,// true אם בדרך ליעד
                        CustomerInParcelSender = parcel.CustomerInParcelSender,
                        CustomerInParcelRecipient = parcel.CustomerInParcelRecipient,
                        CollectionLocation = findCustomerLocation(parcel.CustomerInParcelSender.Id),
                        DeliveryDestinationLocation = findCustomerLocation(parcel.CustomerInParcelRecipient.Id)
                    };
                    blDrone.ParcelInTransfer.TransportDistance =
                        getDistance(blDrone.ParcelInTransfer.CollectionLocation, blDrone.ParcelInTransfer.DeliveryDestinationLocation);
                    blDrone.ParcelTransferredNumber = parcel.Id;
                }
                drones.Add(blDrone);
            }
            return drones;
        }


        #endregion

        #region Customers
        /// <summary>
        /// פונקציית תצוגת כל הלקוחות
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustomerToList> GetCustomerList()
        {
            List<CustomerToList> customers = new();
            foreach (var customer in dalObj.GetCustomerList())
            {
                CustomerToList blCustomer = new()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    NumberOfParcelSentAndDelivered = findNumberOfParcelSentAndDelivered(customer.Id),
                    NumberOfParcelSentButNotYetDelivered =
                    dalObj.GetSenderParcels(customer.Id).Count() - findNumberOfParcelSentAndDelivered(customer.Id),
                    NumberOfParcelReceived = findNumberOfParcelReceived(customer.Id),
                    NumberOfParcelOnTheWayToTheCustomer =
                    dalObj.GetTargetParcels(customer.Id).Count() - findNumberOfParcelReceived(customer.Id)
                };
                customers.Add(blCustomer);
            }
            return customers;
        }



        #endregion

        #region Parcels
        /// <summary>
        /// פונקציית תצוגת רשימת החבילות
        /// </summary>
        /// <returns>רשימת כל החבילות</returns>
        public IEnumerable<ParcelToList> GetParcelList()
        {
            List<ParcelToList> parcels = new();
            //List<Parcel> dalParcels = new();
            //if(predicate==null)
            //{
            //    dalParcels = (List<Parcel>)dalObj.GetParcelList();
            //}
            //else
            //{
            //    dalParcels= (List<Parcel>)dalObj.GetParcelList(predicate);
            //}
            int senderId, targetId;
            foreach (var item in dalObj.GetParcelList())
            {
                senderId = dalObj.GetParcel(item.Id).SenderId;
                targetId = dalObj.GetParcel(item.Id).TargetId;
                ParcelToList blParcel = new()
                {
                    Id = item.Id,
                    SenderName = getCustomerName(senderId),
                    RecipientName = getCustomerName(targetId),
                    Weight = (WeightCategories)item.Weight,
                    Priority = (Priorities)item.Priority,
                    StatusParcel = checkParcelStatus(item)
                };
                parcels.Add(blParcel);
            }
            return parcels;
        }
        #endregion

        #region Parcels no drone
        /// <summary>
        /// פונקציית תצוגת חבילות שעוד לא שויכו לרחפן 
        /// </summary>
        /// <returns>רשימת חבילות שעוד לא שויכו לרחפן</returns>
        public IEnumerable<ParcelToList> GetParcelNoDroneList()
        {
            List<ParcelToList> parcels = new();
            int senderId, targetId;
            foreach (var item in dalObj.GetParcelNoDroneList())
            {
                senderId = dalObj.GetParcel(item.Id).SenderId;
                targetId = dalObj.GetParcel(item.Id).TargetId;
                ParcelToList blParcel = new()
                {
                    Id = item.Id,
                    SenderName = getCustomerName(senderId),
                    RecipientName = getCustomerName(targetId),
                    Weight = (WeightCategories)item.Weight,
                    Priority = (Priorities)item.Priority,
                    StatusParcel = checkParcelStatus(item)
                };
                parcels.Add(blParcel);
            }
            return parcels;
        }
        #endregion

        #region Available charging stations
        /// <summary>
        /// פונקציית תצוגת תחנות עם עמדות טעינה פנויות
        /// </summary>
        /// <returns>רשימת תחנות עם עמדות טעינה פנויות</returns>
        public IEnumerable<StationToList> GetAvailableChargingStationsList()
        {
            List<StationToList> stations = new();
            foreach (var station in dalObj.GetAvailableChargingStationsList())
            {
                StationToList blStation = new()
                {
                    Id = station.Id,
                    Name = station.Name,
                    AvailableStations = station.AvailableStations,
                    NotAvailableStations = dalObj.GetDronesInStationId(station.Id).Count()
                };
                stations.Add(blStation);
            }
            return stations;
        }
        #endregion

        #region DroneCharges
        /// <summary>
        /// תצוגת רחפנים בטעינה
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.DroneCharge> GetDroneChargesList()
        {
            return (from item in dalObj.GetDroneChargesList()
                    select item).ToList();
        }
        #endregion

        #region PartOfDrone
        /// <summary>
        /// פונקצית תצוגת רשימת רחפנים לפי תנאי
        /// </summary>
        /// <param name="p">פרדיקט</param>
        /// <returns>רשימת רחפנים לפי תנאי</returns>
        public IEnumerable<DroneToList> GetPartOfDroneList(Predicate<DroneToList> p)
        {

            return (from item in GetDroneList()
                    where p(item)
                    select item).ToList();
        }
        #endregion

        #region PartOfParcel
        /// <summary>
        /// תצוגת חבילות לפי תנאי
        /// </summary>
        /// <param name="predicate">פרדיקט</param>
        /// <returns>רשימת חבילות לפי תנאי</returns>
        public IEnumerable<ParcelToList> GetParcelList(Predicate<ParcelToList> predicate)
        {
            return (from item in GetParcelList()
                    where predicate(item)
                    select item).ToList();
        }
        #endregion

        #region PartOfStation
        /// <summary>
        /// תצוגת תחנות לפי תנאי
        /// </summary>
        /// <param name="predicate">פרדיקט</param>
        /// <returns>רשימת תחנות לפי תנאי</returns>
        public IEnumerable<StationToList> GetPartOfStationList(Predicate<StationToList> predicate)
        {
            return (from item in GetStationList()
                    where predicate(item)
                    select item).ToList();
        }
        #endregion

        #region PartOfCustomer
        /// <summary>
        /// תצוגת לקוחות לפי תנאי
        /// </summary>
        /// <param name="predicate">פרדיקט</param>
        /// <returns>רשימת לקוחות לפי תנאי</returns>
        public IEnumerable<CustomerToList> GetPartOfCustomerList(Predicate<CustomerToList> predicate)
        {
            return (from item in GetCustomerList()
                    where predicate(item)
                    select item).ToList();
        }
        #endregion

        #region Users
        public IEnumerable<UserToLIst> GetUSList()
        {
            List<UserToLIst> Users = new();
            foreach (var v in dalObj.GetUserList())
            {
                UserToLIst user = new()
                {
                    Id = v.Id,
                    Name = v.Name,
                    Password = v.Password
                };
                Users.Add(user);
            }
            return Users;
        }
        #endregion

        //-----------------------------------HELP-METHODS-----------------------------------

        #region Distance
        /// <summary>
        /// פונקצייה לחישוב מרחק בין 2 מיקומים
        /// </summary>
        /// <param name="location1">מיקום ראשון</param>
        /// <param name="location2">מיקןם שני</param>
        /// <param name="unit">סוג הערך המוחזר, קילומטר ברירת מחדל</param>
        /// <returns>מרחק בין 2 המיקומים</returns>
        private double getDistance(Location location1, Location location2, char unit = 'K')
        {
            double lat1 = location1.Latitude;
            double lon1 = location1.Longitude;
            double lat2 = location2.Latitude;
            double lon2 = location2.Latitude;

            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                 Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                 Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'K': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles 
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }
            return dist;
        }
        #endregion

        #region Closest station with available charge slots
        /// <summary>
        /// מציאת תחנה עם עמדות טעינה פנויות הכי קרובה לרחפן
        /// </summary>
        /// <param name="droneId">מזהה רחפן</param>
        /// <returns>מזהה התחנה הקרובה</returns>
        private int findClosestStationWithAvailableChargeSlots(int droneId)
        {
            double distance;
            Location droneLocation = getDroneLocation(droneId);
            List<DO.Station> stations = (List<DO.Station>)dalObj.GetStationList();
            Location stationLocation = new() { Longitude = stations[0].Longitude, Latitude = stations[0].Latitude };
            int stationId = stations[0].Id;
            double minDistance = getDistance(droneLocation, stationLocation);
            for (int i = 1; i < stations.Count; i++)
            {
                stationLocation.Longitude = stations[i].Longitude;
                stationLocation.Latitude = stations[i].Latitude;
                distance = getDistance(droneLocation, stationLocation);
                if (distance < minDistance && stations[i].AvailableStations > 0)
                {
                    minDistance = distance;
                    stationId = stations[i].Id;
                }
            }
            return stationId;
        }
        #endregion

        #region Closest station to customer
        /// <summary>
        /// מציאת תחנה הכי קרובה לשולח
        /// </summary>
        /// <param name="id">מזהה לקוח</param>
        /// <returns>מזהה התחנה הקרובה</returns>
        private int findClosestStationToCustomer(int id)
        {
            double distance;
            Location customerLocation = findCustomerLocation(id);
            List<DO.Station> stations = (List<DO.Station>)dalObj.GetStationList();
            Location stationLocation = new() { Longitude = stations[0].Longitude, Latitude = stations[0].Latitude };
            int stationId = stations[0].Id;
            double minDistance = getDistance(customerLocation, stationLocation);
            for (int i = 1; i < stations.Count; i++)
            {
                stationLocation.Longitude = stations[i].Longitude;
                stationLocation.Latitude = stations[i].Latitude;
                distance = getDistance(customerLocation, stationLocation);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    stationId = stations[i].Id;
                }
            }
            return stationId;
        }
        #endregion

        #region Drone battery
        /// <summary>
        /// פןנקצייה המחזירה את מצב הבטרייה של רחפן כלשהו
        /// </summary>
        /// <param name="droneId">מזהה רחפן</param>
        /// <returns>מצב הבטרייה</returns>
        private double getDroneBattery(int droneId)
        {
            return DronesList.Find(drone => drone.Id == droneId).Battery;
        }
        #endregion

        #region Customer name
        /// <summary>
        /// מציאת שם לקוח עפ"י מספר מזהה
        /// </summary>
        /// <param name="id">מזהה לקוח</param>
        /// <returns>שם הלקוח</returns>
        private string getCustomerName(int id)
        {
            return dalObj.GetCustomer(id).Name;
        }
        #endregion

        #region Customer location
        /// <summary>
        /// מציאת מיקום לקוח עפ"י מספר מזהה
        /// </summary>
        /// <param name="id">מזהה לקוח</param>
        /// <returns>מיקום הלקוח</returns>
        private Location findCustomerLocation(int id)
        {
            DO.Customer customer = dalObj.GetCustomer(id);
            Location location = new() { Longitude = customer.Longitude, Latitude = customer.Latitude };
            return location;
        }
        #endregion

        #region Random station location 
        /// <summary>
        /// הגרלת מיקום בין התחנות הקיימות
        /// </summary>
        /// <returns>מזהה של תחנה רנדומלית</returns>
        private int getRandomStation()
        {
            List<DO.Station> stations = dalObj.GetStationList().ToList();
            DO.Station rndStation = stations[r.Next(stations.Count)];
            return rndStation.Id;
        }
        #endregion

        #region Parcel status
        /// <summary>
        /// פונקצייה הבודקת סטטוס של חבילה
        /// </summary>
        /// <param name="parcel">חבילה</param>
        /// <returns>סטטוס החבילה</returns>
        private StatusParcel checkParcelStatus(DO.Parcel parcel)
        {
            StatusParcel status;
            if (parcel.Scheduled == null)
                status = StatusParcel.Defined;
            else if (parcel.PickedUp == null)
                status = StatusParcel.Associated;
            else if (parcel.Delivered == null)
                status = StatusParcel.Collected;
            else
                status = StatusParcel.Supplied;
            return status;
        }
        #endregion

        #region Number of parcel sent and delivered
        /// <summary>
        /// פונקצייה המחזירה מספר חבילות שלקוח שלח וסופקו
        /// </summary>
        /// <param name="id">מזהה לקוח</param>
        /// <returns>מספר החבילות ששלח וסופקו</returns>
        private int findNumberOfParcelSentAndDelivered(int id)
        {
            return dalObj.GetSenderParcels(id).Where(x => x.Delivered != null).Count();
        }
        #endregion

        #region Number of parcel received
        /// <summary>
        /// פונקצייה המחזירה מספר חבילות שלקוח קיבל
        /// </summary>
        /// <param name="id">מזהה לקוח</param>
        /// <returns>מספר החבילות שקיבל</returns>
        private int findNumberOfParcelReceived(int id)
        {
            return dalObj.GetTargetParcels(id).Where(x => x.Delivered != null).Count();
        }
        #endregion

        #region findStationLocation
        /// <summary>
        /// מציאת מיקום תחנה
        /// </summary>
        /// <param name="stationId">מזהה תחנה</param>
        /// <returns>מיקום התחנה</returns>
        private Location findStationLocation(int stationId)
        {
            DO.Station station = dalObj.GetStation(stationId);
            Location location = new() { Longitude = station.Longitude, Latitude = station.Latitude };
            return location;
        }
        #endregion

        #region getRandomCustomerLocation
        /// <summary>
        /// מיקום רנדומלי של לקוח
        /// </summary>
        /// <returns>מיקום רנדומלי של לקוח</returns>
        private Location getRandomCustomerLocation()
        {
            List<int> customerId = new();
            foreach (var customer in dalObj.GetCustomerList())
            {
                if (findNumberOfParcelReceived(customer.Id) > 0)
                    customerId.Add(customer.Id);
            }
            Location location = findCustomerLocation(customerId[r.Next(customerId.Count)]);
            return location;
        }
        #endregion

        #region getDroneLocation
        /// <summary>
        /// מציאת מיקום רחפן
        /// </summary>
        /// <param name="droneId">מזהה רחפן</param>
        /// <returns>מיקום הרחפן</returns>
        private Location getDroneLocation(int droneId)
        {
            return DronesList.Find(drone => drone.Id == droneId).CurrentLocation;
        }
        #endregion

        #region checkDroneStatus
        /// <summary>
        /// בדיקת סטטוס רחפן
        /// </summary>
        /// <param name="id">מזהה רחפן</param>
        /// <returns>סטטוס רחפן</returns>
        private StatusDrone checkDroneStatus(int id)
        {
            StatusDrone status;
            if (dalObj.GetDroneChargesList().ToList().Exists(x => x.DroneId == id))
                status = StatusDrone.Maintenance;
            else if (dalObj.GetParcelList().ToList().Exists(x => x.DroneId == id && x.Delivered == null))
                status = StatusDrone.Delivery;
            else
                status = StatusDrone.Available;
            return status;
        }
        #endregion

        #region DeleteParcel
        /// <summary>
        /// מחיקת חבילה 
        /// </summary>
        /// <param name="parcel">חבילה למחיקה</param>
        public void DeleteParcel(Parcel parcel)
        {
            DO.Parcel dalParcel = dalObj.GetParcel(parcel.Id);
            dalObj.DeleteParcel(dalParcel);
        }
        #endregion
       
    }

}
