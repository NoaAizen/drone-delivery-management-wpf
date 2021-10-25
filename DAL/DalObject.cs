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
            public static void UpdateDroneToParcel(int idDrone, int idParcel)
            {
                for (int i = 0; i < DataSource.listParcels.Capacity; i++)
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

            public static void CollectionParcelFromDrone(int idDrone, int idParcel)//איסוף חבילה לרחפן 
            {
                for (int i = 0; i < DataSource.listParcels.Capacity; i++)
                {
                    if (DataSource.listParcels[i].Id == idParcel)
                    {
                        DAL.IDAL.DO.Parcel p = DataSource.listParcels[i];
                        p.PickedUp = DateTime.Now;
                        DataSource.listParcels[i] = p;
                    }
                }
                for (int i = 0; i < DataSource.listDrones.Capacity; i++)//עדכון סטטוס
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
                for (int i = 0; i < DataSource.listParcels.Capacity; i++)
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
                for (int i = 0; i < DataSource.listDrones.Capacity; i++)//עדכון סטטוס
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
                for (int i = 0; i < DataSource.listDrones.Capacity; i++)//עדכון סטטוס
                {

                    if (DataSource.listDrones[i].Id == idDrone)
                    {
                        DAL.IDAL.DO.Drone d = DataSource.listDrones[i];
                        d.Status = (DAL.IDAL.DO.StatusDrone)1;
                        DataSource.listDrones[i] = d;
                    }
                }
                for (int i = 0; i < DataSource.listStations.Capacity; i++)//עדכון מספר תחנות הטענה פנויות
                {

                    if (DataSource.listStations[i].Id == idStation)
                    {
                        DAL.IDAL.DO.Station s = DataSource.listStations[i];
                        s.AvailableStations -=1 ;
                        DataSource.listStations[i] = s;
                    }
                }
                DAL.IDAL.DO.DroneCharge dc = new IDAL.DO.DroneCharge(idDrone, idStation);
            }


            public static void ReleaseDroneFromCharging(int idDrone, int idStation)// שחרור רחפן מטעינה בתחנת בסיס
            {
                for (int i = 0; i < DataSource.listDrones.Capacity; i++)//עדכון סטטוס
                {

                    if (DataSource.listDrones[i].Id == idDrone)
                    {
                        DAL.IDAL.DO.Drone d = DataSource.listDrones[i];
                        d.Status = (DAL.IDAL.DO.StatusDrone)0;
                        d.Battery = 100;
                        DataSource.listDrones[i] = d;
                    }
                }
                for (int i = 0; i < DataSource.listStations.Capacity; i++)//עדכון מספר תחנות הטענה פנויות
                {

                    if (DataSource.listStations[i].Id == idStation)
                    {
                        DAL.IDAL.DO.Station s = DataSource.listStations[i];
                        s.AvailableStations += 1;
                        DataSource.listStations[i] = s;
                    }
                }
            }


            public static IDAL.DO.Station ViewStation(int idStation)
            {
                IDAL.DO.Station s = new IDAL.DO.Station();
                for (int i = 0; i < DataSource.listStations.Capacity; i++)//עדכון מספר תחנות הטענה פנויות
                {
                    if (DataSource.listStations[i].Id == idStation)
                    {
                        s = DataSource.listStations[i];
                        return s;
                        
                    }
                   
                }
                return s;
            }


        }
    }
}

