using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace IDAL
    {
       public interface IDal
        {
            /// <summary>
            /// פונקצית  הוספת רחפן לרשימת הרחפנים הקיימים 
            /// </summary>
            /// <param name="s"></param>
            void AddStation(DAL.IDAL.DO.Station s);
            /// <summary>
            /// פונקצית הוספת רחפן לרשימת רחפנים 
            /// </summary>
            /// <param name="d"></param>
            void AddDrone(DAL.IDAL.DO.Drone d);
            /// <summary>
            ///  פונקציית קליטת לקוח חדש לרשימת הלקוחות 
            /// </summary>
            /// <param name="c"></param>
            void AddCustomer(DAL.IDAL.DO.Customer c);

            /// <summary>
            ///  פונקציית קליטת חבילה למשלוח
            /// </summary>
            /// <param name="p"></param>
            void AddParcel(DAL.IDAL.DO.Parcel p);

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
            IDAL.DO.Station ViewStation(int idStation);//

            /// <summary>
            /// פונקציית להדפסת רחפן אחת
            /// </summary>
            /// <param name="idDrone"></param>
            /// <returns></returns>
            IDAL.DO.Drone ViewDrone(int idDrone);

            /// <summary>
            /// פונקציית הדפסת לקוח אחד
            /// </summary>
            /// <param name="idCustomer"></param>
            /// <returns></returns>
            IDAL.DO.Customer ViewCustomer(int idCustomer);

            /// <summary>
            /// הדפסת חבילה אחת
            /// </summary>
            /// <param name="idParcel"></param>
            /// <returns></returns>
            IDAL.DO.Parcel ViewParcel(int idParcel);//הדפסת חבילה

            /// <summary>
            /// פונמיתת הדפסת כל התחנות
            /// </summary>
            /// <returns></returns>
            IEnumerable<IDAL.DO.Station> ViewStationList();//

            /// <summary>
            /// פונקציית הדפסת כל הרפנים
            /// </summary>
            /// <returns></returns>
            IEnumerable<IDAL.DO.Drone> ViewDroneList();

            /// <summary>
            /// פונקציית הדפסת כל לקוחות
            /// </summary>
            /// <returns></returns>
            IEnumerable<IDAL.DO.Customer> ViewCustomerList();

            /// <summary>
            /// פונקציית הדפסת כל חבילות
            /// </summary>
            /// <returns></returns>
            IEnumerable<IDAL.DO.Parcel> ViewParcelList();

            /// <summary>
            /// פונקציית הדפסת  חבילות שעוד לא שויכו לרחפן 
            /// </summary>
            /// <returns></returns>
            IEnumerable<IDAL.DO.Parcel> ViewParcelNoDronelList();

            /// <summary>
            /// פונקציית הדפסת תחנות עם עמדות טעינה פנויות
            /// </summary>
            /// <returns></returns>
            IEnumerable<IDAL.DO.Station> ViewAvailableChargingStationslList();
            //double[] PowerRequestToDrone();//צריכת חשמל ע"י רחפן 
        }
    }
}
