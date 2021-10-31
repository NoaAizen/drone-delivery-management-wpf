using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace DalObject
    {
        public class DalObject
        {
            DalObject()
            {
                DataSource.Config.Initialize();
            }

            public static void AddStation(DAL.IDAL.DO.Station s)
            {
                DataSource.listStations.Add(s);
            }
            public static void AddDrone(DAL.IDAL.DO.Drone d)
            {
                DataSource.listDrones.Add(d);
            }
            public static void AddCustomer(DAL.IDAL.DO.Customer c)
            {
                DataSource.listCustomers.Add(c);
            }
            public static void AddParcel(DAL.IDAL.DO.Parcel p)
            {
                DataSource.Config.CounterForParcels++;
                DataSource.listParcels.Add(p);
            }
            public static void UpdateDroneToParcel(int idDrone, int idParcel) //שיוך חבילה לרחפן
            {
                for (int i = 0; i < DataSource.listParcels.Count; i++)
                {
                    if (DataSource.listParcels[i].Id == idParcel)
                    {
                        DAL.IDAL.DO.Parcel p = DataSource.listParcels[i];
                        p.DroneId = idDrone;
                        p.Scheduled = DateTime.Now;
                        DataSource.listParcels[i] = p;
                    }
                }
            }

            public static void CollectionParcelFromDrone(int idDrone, int idParcel)//איסוף חבילה ע"י רחפן 
            {
                for (int i = 0; i < DataSource.listParcels.Count; i++)
                {
                    if (DataSource.listParcels[i].Id == idParcel)
                    {
                        DAL.IDAL.DO.Parcel p = DataSource.listParcels[i];
                        p.PickedUp = DateTime.Now;
                        DataSource.listParcels[i] = p;
                    }
                }
                for (int i = 0; i < DataSource.listDrones.Count; i++)//עדכון סטטוס
                {

                    if (DataSource.listDrones[i].Id == idDrone)
                    {
                        DAL.IDAL.DO.Drone d = DataSource.listDrones[i];
                        d.Status = (DAL.IDAL.DO.StatusDrone)2;
                        DataSource.listDrones[i] = d;
                    }
                }
            }

            public static void DeliveryParcelForCustomer(int idCustomer, int idParcel)//אספקת חבילה ללקוח 
            {
                int idDrone=0;//לבדוק שלא הציב 0 בif
                for (int i = 0; i < DataSource.listParcels.Count; i++)
                {
                    if (DataSource.listParcels[i].Id == idParcel)
                    {
                        DAL.IDAL.DO.Parcel p = DataSource.listParcels[i];
                        p.Delivered = DateTime.Now;
                        p.TargetId = idCustomer;
                        idDrone = p.DroneId;
                        DataSource.listParcels[i] = p;
                    }
                }
                for (int i = 0; i < DataSource.listDrones.Count; i++)//עדכון סטטוס
                {

                    if (DataSource.listDrones[i].Id == idDrone)//לבדוק שלא הציב 0 בif
                    {
                        DAL.IDAL.DO.Drone d = DataSource.listDrones[i];
                        d.Status = (DAL.IDAL.DO.StatusDrone)0;
                        DataSource.listDrones[i] = d;
                    }
                }
            }

            public static void SendingDroneForCharging(int idDrone, int idStation)// שליחת רחפן לטעינה בתחנת בסיס
            {
                for (int i = 0; i < DataSource.listDrones.Count; i++)//עדכון סטטוס
                {

                    if (DataSource.listDrones[i].Id == idDrone)
                    {
                        DAL.IDAL.DO.Drone d = DataSource.listDrones[i];
                        d.Status = (DAL.IDAL.DO.StatusDrone)1;
                        DataSource.listDrones[i] = d;
                    }
                }
                for (int i = 0; i < DataSource.listStations.Count; i++)//עדכון מספר תחנות הטענה פנויות
                {

                    if (DataSource.listStations[i].Id == idStation)
                    {
                        DAL.IDAL.DO.Station s = DataSource.listStations[i];
                        s.AvailableStations -=1 ;
                        DataSource.listStations[i] = s;
                    }
                }
                DAL.IDAL.DO.DroneCharge dc = new IDAL.DO.DroneCharge(idDrone, idStation);
                DataSource.listDroneCharges.Add(dc);
            }


            public static void ReleaseDroneFromCharging(int idDrone, int idStation)// שחרור רחפן מטעינה בתחנת בסיס
            {
                for (int i = 0; i < DataSource.listDrones.Count; i++)//עדכון סטטוס
                {

                    if (DataSource.listDrones[i].Id == idDrone)
                    {
                        DAL.IDAL.DO.Drone d = DataSource.listDrones[i];
                        d.Status = (DAL.IDAL.DO.StatusDrone)0;
                        d.Battery = 100;
                        DataSource.listDrones[i] = d;
                    }
                }
                for (int i = 0; i < DataSource.listStations.Count; i++)//עדכון מספר תחנות הטענה פנויות
                {

                    if (DataSource.listStations[i].Id == idStation)
                    {
                        DAL.IDAL.DO.Station s = DataSource.listStations[i];
                        s.AvailableStations += 1;
                        DataSource.listStations[i] = s;
                    }
                }
                for (int i = 0; i < DataSource.listDroneCharges.Count; i++)
                {
                    if (DataSource.listDroneCharges[i].StationId == idStation && 
                        DataSource.listDroneCharges[i].DroneId== idDrone)
                    {
                        DAL.IDAL.DO.DroneCharge dc = DataSource.listDroneCharges[i];
                        DataSource.listDroneCharges.Remove(dc);
                    }
                }
            }


            public static IDAL.DO.Station ViewStation(int idStation)//הדפסת תחנה
            {
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
            public static IDAL.DO.Drone ViewDrone(int idDrone)//הדפסת רחפן
            {
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
            
              public static IDAL.DO.Customer ViewCustomer(int idCustomer)//הדפסת לקוח
            {
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
            public static IDAL.DO.Parcel ViewParcel(int idParcel)//הדפסת חבילה
            {
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

            public static List <IDAL.DO.Station> ViewStationList()//הדפסת תחנות
            {
                List<IDAL.DO.Station> temp = new List<IDAL.DO.Station>();

                for (int i = 0; i < DataSource.listStations.Count; i++)
                {
                    
                       temp.Add(DataSource.listStations[i]);
                }
                    return temp;
            }
            public static List<IDAL.DO.Drone> ViewDroneList()//הדפסת רחפנים
            {
                List<IDAL.DO.Drone> temp = new List<IDAL.DO.Drone>();

                for (int i = 0; i < DataSource.listDrones.Count; i++)
                {

                    temp.Add(DataSource.listDrones[i]);
                }
                return temp;
            }
            public static List<IDAL.DO.Customer> ViewCustomerList()//הדפסת לקוחות
            {
                List<IDAL.DO.Customer> temp = new List<IDAL.DO.Customer>();

                for (int i = 0; i < DataSource.listCustomers.Count; i++)
                {

                    temp.Add(DataSource.listCustomers[i]);
                }
                return temp;
            }
            public static List<IDAL.DO.Parcel> ViewParcelList()//הדפסת חבילות
            {
                List<IDAL.DO.Parcel> temp = new List<IDAL.DO.Parcel>();

                for (int i = 0; i < DataSource.listParcels.Count; i++)
                {

                    temp.Add(DataSource.listParcels[i]);
                }
                return temp;
            }
            public static List<IDAL.DO.Parcel> ViewParcelNoDronelList()//הדפסת חבילות שעוד לא שויכו לרחפן
            {
                List<IDAL.DO.Parcel> temp = new List<IDAL.DO.Parcel>();

                for (int i = 0; i < DataSource.listParcels.Count; i++)
                {
                    if(DataSource.listParcels[i].DroneId == 0)
                    temp.Add(DataSource.listParcels[i]);
                }
                return temp;
            }
            public static List<IDAL.DO.Station> ViewAvailableChargingStationslList() // הדפסת תחנות עם עמדות טעינה פנויות
            {
                List<IDAL.DO.Station> temp = new List<IDAL.DO.Station>();

                for (int i = 0; i < DataSource.listStations.Count; i++)
                {
                    if(DataSource.listStations[i].AvailableStations>0)
                    temp.Add(DataSource.listStations[i]);
                }
                return temp;
            }
        }
    }
}

