using System;
using BO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;


namespace BL
{
    internal sealed class BL : BlApi.IBL
    {
        static readonly BL instance = new BL();//שדה פרטי סטטי 
        internal static BL Instance { get => instance; }
        private List<DroneToList> DronesList = new List<DroneToList>();//רשימת רחפנים
        IDal dalObj;
        private static Random r;

        static BL() { }
        
        BL() 
        {
            r = new Random();
            dalObj = DalFactory.GetDal("1");
            DO.Parcel parcel = new();
            StatusDrone status = 0;
            Location location = new();
            double battery = 0;
            int stationId=0;
            List<DO.Drone> drones = (List<DO.Drone>)dalObj.GetDroneList();
            List<DO.Parcel> parcels = (List<DO.Parcel>)dalObj.GetParcelList();
            foreach (var drone in drones)
            {
                if (parcels.Exists(x => x.DroneId == drone.Id))
                {//הרחפן במשלוח
                    parcel = (from item in parcels
                              where item.DroneId == drone.Id
                              select item).FirstOrDefault();
                    //לברר איך אפשר לעשות את הבדיקה
                    if (/*parcel!=null &&*/ parcel.Scheduled != null && parcel.Delivered == null)//חבילה שעוד לא סופקה אך הרחפן כבר שויך
                    {
                        status = (StatusDrone)2;
                        if (parcel.PickedUp == null)//החבילה שויכה אך לא נאספה
                        {
                            stationId = findClosestStationToCustomer(parcel.SenderId);
                            location = /*GetStation(stationId).Location*/ findStationLocation(stationId);
                        }
                        else
                        {
                            location = findCustomerLocation(parcel.SenderId);
                        }
                        //random.NextDouble() * (maximum - minimum) + minimum
                        battery = r.NextDouble() * (100 - 50) + 50;//הגרלת סוללה בין 50 ל100
                    }
                    else
                    {//הרחפן לא במשלוח
                     //if(parcel == null) כנ"ל
                        status = (StatusDrone)r.Next(0, 2);
                        if (status == 0)//הרחפן פנוי
                        {
                            location = getRandomCustomerLocation(); //לסדר את הבעייה
                            battery = r.NextDouble() * (100 - 50) + 50;//הגרלת סוללה בין 50 ל100
                        }
                        else//הרחפן בתחזוקה
                        {
                            location = getRandomStationLocation();
                            battery = r.NextDouble() * (20 - 0) + 0;
                        }
                    }
                }
                else
                {//הרחפן לא במשלוח
                    //if(parcel == null) כנ"ל
                    status = (StatusDrone)r.Next(0, 2);
                    if (status == 0)//הרחפן פנוי
                    {
                        location = getRandomCustomerLocation(); //לסדר את הבעייה
                        battery = r.NextDouble() * (100 - 50) + 50;//הגרלת סוללה בין 50 ל100
                    }
                    else//הרחפן בתחזוקה
                    {
                        location = getRandomStationLocation();
                        battery = r.NextDouble() * (20 - 0) + 0;
                    }
                }
                DroneToList blDrone = new()
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = (WeightCategories)drone.MaxWeight,
                    Status = status,
                    Battery = battery,
                    //ParcelInTransfer
                    CurrentLocation = location,
                    //ParcelTransferredNumber=...
                };
                DronesList.Add(blDrone);
            }
        }


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
                dalObj.AddStation(dalStation);
            }
            catch(Exception ex)
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
                dalObj.AddDrone(dalDrone);
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
                dalObj.AddCustomer(dalCustomer);
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
                return dalObj.AddParcel(dalParcel);
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
                dalObj.UpdateDroneModel(id, model);
            }
            catch(Exception ex)
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
                dalObj.UpdateStation(id, name, totalChargingStations);
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
                dalObj.UpdateCustomer(id, name, phone);
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
            DroneToList drone = DronesList.Find(x => x.Id == id);
            if (drone.Status == 0 && drone.Battery >= 20)//אם הרחפן פנוי ויש מספיק סוללה
            {
                int stationId = findClosestStationWithAvailableChargeSlots(id);
                try
                {
                    dalObj.SendingDroneForCharging(id, stationId);
                }
                catch (Exception ex)
                {
                    throw new DoesntExistException(ex.Message, ex);
                }
                for (int i = 0; i < DronesList.Count; i++)//עדכון נתוני הרחפן
                {
                    if (DronesList[i].Id == id)
                    {
                        DroneToList d = DronesList[i];
                        d.Status = (StatusDrone)1;
                        d.Battery -= 5;
                        d.CurrentLocation = GetStation(stationId).Location;
                        DronesList[i] = d;
                        break;
                    }
                }
            }
            else
                throw new ActionProblemException("Can't sending drone to charging");
        }
        #endregion

        #region Release
        /// <summary>
        /// פונקציית שחרור רחפן מטעינה
        /// </summary>
        /// <param name="id">מזהה רחפן</param>
        /// <param name="chargingTime">פרק זמן בטעינה</param>
        public void ReleaseDroneFromCharging(int id, TimeSpan chargingTime)//מה זה פרק זמן בטעינה?
        {
            DroneToList drone = DronesList.Find(x => x.Id == id);
            if (drone == null || drone.Status != StatusDrone.Maintenance)
                throw new ActionProblemException("Can't release drone from charging");
            int stationId = dalObj.GetDroneChargesList().ToList().Find(x => x.DroneId == id).StationId;
            try
            {
                dalObj.ReleaseDroneFromCharging(id, stationId);
            }
            catch (Exception ex)
            {
                throw new DoesntExistException(ex.Message, ex);
            }
            DronesList.Remove(drone);
            drone.Battery = 100;
            drone.Status = 0;
            DronesList.Add(drone);
        }
        #endregion

        #region Assignment
        /// <summary>
        /// פונקצית שיוך חבילה לרחפן 
        /// </summary>
        /// <param name="idDrone">מזהה רחפן</param>
        public void UpdateDroneToParcel(int idDrone)
        {
            DroneToList drone = DronesList.Find(x => x.Id == idDrone);
            if (drone == null || drone.Status != StatusDrone.Available)
                //לשנות חריגה
                throw new Exception("Error");

        }
        #endregion

        #region Collection
        /// <summary>
        /// פונקציית איסוף חבילה ע"י רחפן 
        /// </summary>
        /// <param name="idDrone">מזהה רחפן</param>
        public void CollectionParcelFromDrone(int idDrone)
        {
            DroneToList drone = DronesList.Find(x => x.Id == idDrone);
            List<DO.Parcel> parcels = (List<DO.Parcel>)dalObj.GetParcelList();
            if (drone!=null && parcels.Exists(x => x.DroneId == idDrone))
            {//הרחפן במשלוח
                DO.Parcel parcel = (from item in parcels
                                         where item.DroneId == idDrone
                                         select item).FirstOrDefault();
                if (parcel.Scheduled != null && parcel.PickedUp == null)//החבילה שויכה אך לא נאספה
                {
                    try
                    {
                        dalObj.CollectionParcelFromDrone(idDrone, parcel.Id);
                    }
                    catch (Exception ex)
                    {
                        throw new DoesntExistException(ex.Message, ex);
                    }
                    DronesList.Remove(drone);
                    drone.Battery -= 20;
                    drone.CurrentLocation=findCustomerLocation(parcel.SenderId);
                    DronesList.Add(drone);
                }
                //else
                //    throw new.... לא יכול לבצע פעולה
            }
            //else
            //    throw new.... לא קיים
        }
        #endregion

        #region Delivery
        /// <summary>
        /// אספקת חבילה ע"י רחפן
        /// </summary>
        /// <param name="idDrone">מזהה רחפן</param>
        public void DeliveryParcelByDrone(int idDrone)
        {
            DroneToList drone = DronesList.Find(x => x.Id == idDrone);
            List<DO.Parcel> parcels = (List<DO.Parcel>)dalObj.GetParcelList();
            if (drone != null && parcels.Exists(x => x.DroneId == idDrone))
            {//הרחפן במשלוח
                DO.Parcel parcel = (from item in parcels
                                         where item.DroneId == idDrone
                                         select item).FirstOrDefault();
                if (parcel.PickedUp != null && parcel.Delivered == null)//החבילה נאספה אך לא סופקה
                {
                    try
                    {
                        dalObj.DeliveryParcelForCustomer(parcel.TargetId, parcel.Id);
                    }
                    catch (Exception ex)
                    {
                        throw new DoesntExistException(ex.Message, ex);
                    }
                    DronesList.Remove(drone);
                    drone.Battery -= 20;
                    drone.CurrentLocation = findCustomerLocation(parcel.TargetId);
                    drone.Status = 0;
                    DronesList.Add(drone);
                }
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
                //DroneInChargingsList = null
                ////יש פה שגיאה בזמן ריצה
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
            }
            catch (Exception ex)
            {
                throw new DoesntExistException(ex.Message, ex);
            }
            Drone blDrone = new()
            {
                Id = dalDrone.Id,
                Model = dalDrone.Model,
                MaxWeight = (WeightCategories)dalDrone.MaxWeight,
                Status = drone.Status,
                Battery=drone.Battery,
                CurrentLocation=drone.CurrentLocation
            };
            if(blDrone.Status == StatusDrone.Delivery)
            {
                DO.Parcel dalParcel = dalObj.GetParcelList().ToList().Find(x => x.DroneId == id);
                Parcel parcel = GetParcel(dalParcel.Id);
                blDrone.ParcelInTransfer = new()
                {
                    Id=parcel.Id,
                    Weight=parcel.Weight,
                    Priority=parcel.Priority,
                    ParcelStatus = checkParcelStatus(dalParcel) == StatusParcel.Collected,// true אם בדרך ליעד
                    CustomerInParcelSender=parcel.CustomerInParcelSender,
                    CustomerInParcelRecipient=parcel.CustomerInParcelRecipient,
                    CollectionLocation= findCustomerLocation(parcel.CustomerInParcelSender.Id),
                    DeliveryDestinationLocation =findCustomerLocation(parcel.CustomerInParcelRecipient.Id)
                };
                blDrone.ParcelInTransfer.TransportDistance =
                    getDistance(blDrone.ParcelInTransfer.CollectionLocation, blDrone.ParcelInTransfer.DeliveryDestinationLocation);
            }
            return blDrone;
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
            //Console.WriteLine(blCustomer.Name);
            //Console.WriteLine(blCustomer.Phone);
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
            foreach(var station in dalObj.GetStationList())
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
                droneInList = DronesList.Find(x => x.Id ==drone.Id);
                DroneToList blDrone = new()
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = (WeightCategories)drone.MaxWeight,
                    Status = /*checkDroneStatus(drone.Id)*/ droneInList.Status,
                    Battery = droneInList.Battery,
                    CurrentLocation = /*getDroneLocation(drone.Id)*/ droneInList.CurrentLocation
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
        public IEnumerable<CustomertoList> GetCustomerList()
        {
            List<CustomertoList> customers = new();
            foreach(var customer in dalObj.GetCustomerList())
            {
                CustomertoList blCustomer = new()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    NumberOfParcelSentAndDelivered= findNumberOfParcelSentAndDelivered(customer.Id),
                    NumberOfParcelSentButNotYetDelivered= 
                    dalObj.GetSenderParcels(customer.Id).Count()- findNumberOfParcelSentAndDelivered(customer.Id),
                    NumberOfParcelReceived= findNumberOfParcelReceived(customer.Id),
                    NumberOfParcelOnTheWayToTheCustomer=
                    dalObj.GetTargetParcels(customer.Id).Count()- findNumberOfParcelReceived(customer.Id)
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

        public IEnumerable<DO.DroneCharge> GetDroneChargesList()
        {
            return (from item in  dalObj.GetDroneChargesList()
                   select item).ToList();
        }


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
            //var coord1 = new GeoCoordinate(location1.Longitude, location1.Latitude);
            //var coord2 = new GeoCoordinate(location2.Longitude, location2.Latitude);
            //return coord1.GetDistanceTo(coord2);
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
            Location stationLocation=new() { Longitude = stations[0].Longitude, Latitude= stations[0].Latitude };
            int stationId= stations[0].Id;
            double minDistance = getDistance(droneLocation, stationLocation);
            for (int i = 1; i < stations.Count; i++)
            {
                stationLocation.Longitude = stations[i].Longitude;
                stationLocation.Latitude = stations[i].Latitude ;
                distance = getDistance(droneLocation, stationLocation);
                //אפשר לייעל ובמקום התנאי לסנן רק מתוך רשימת תחנות עם עמדות טעינה פנויות לפי הפונקצייה
                if(distance < minDistance && stations[i].AvailableStations > 0)
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
        /// <returns>מיקום של תחנה רנדומלית</returns>
        private Location getRandomStationLocation()
        {
            List<DO.Station> stations = dalObj.GetStationList().ToList();
            DO.Station rndStation = stations[r.Next(stations.Count)];
            Location location = new() { Longitude = rndStation.Longitude, Latitude = rndStation.Latitude };
            return location;
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

        private Location findStationLocation(int stationId)
        {
            DO.Station station = dalObj.GetStation(stationId);
            Location location = new() { Longitude = station.Longitude, Latitude = station.Latitude };
            return location;
        }

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


        private Location getDroneLocation(int droneId)
        {
            return DronesList.Find(drone => drone.Id == droneId).CurrentLocation;
        }

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
       

        //--------------------------------------- לממש!!!

        //לעדכון:
        //Drone d = DataSource.listDrones.Find(x => x.Id == id);
        //DataSource.listDrones.Remove(d);
        //    d.Model = model;
        //    DataSource.listDrones.Add(d);
    }


}
