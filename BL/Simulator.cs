using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Threading;
using static BL.BL;


namespace BL
{
    class Simulator
    {
        const int TIMER = 1000;//מילי שניות
        const double SPEED = 100;//ק"מ לשנייה


        public Simulator(BL bl, int id, Action updateDrone, Func<bool> checkStop)
        {
            Drone drone = bl.GetDrone(id);
            while(!checkStop())
            {
                switch(drone.Status)
                {
                    case StatusDrone.Available:
                        //לתקן לוגיקה
                        //if (drone.Battery >= 20)
                        //{
                            try
                            {
                                bl.UpdateDroneToParcel(id);
                                updateDrone();
                                //Thread.Sleep(TIMER);
                            }
                            catch
                            {
                                if (drone.Battery < 100) 
                                {
                                    bl.SendingDroneForCharging(id);
                                    updateDrone();
                                    Thread.Sleep(TIMER);
                                }
                            }
                        //}
                        //else
                        //{
                            
                        //}
                        break;
                    case StatusDrone.Delivery:
                        try
                        {
                            
                            bl.CollectionParcelFromDrone(id);
                            updateDrone();
                            Thread.Sleep(TIMER);
                            bl.DeliveryParcelByDrone(id);
                            updateDrone();
                            Thread.Sleep(TIMER);

                        }
                        catch (Exception ex)
                        {
                            Thread.Sleep(TIMER);
                        }
                        break;
                    case StatusDrone.Maintenance:
                        while(drone.Battery<100)
                        {
                            drone.Battery += 3;
                            if (drone.Battery > 100)
                                drone.Battery = 100;
                            updateDrone();
                            Thread.Sleep(TIMER);
                        }
                        bl.ReleaseDroneFromCharging(id, new(1, 0, 0));//למחוק פרמטר שני
                        Thread.Sleep(TIMER);
                        break;
                }
            }
        }
    }
}
