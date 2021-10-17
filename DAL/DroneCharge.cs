using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace IDAL
    {
        namespace DO
        {

             public struct DroneCharge// טעינת סוללת רחפן 

            {
                public int DroneId { get; set; }// מזהה רחפן 
                public int StationId { get; set; }//  מזהה תחנת-בסיס 

                public override string ToString()
                {
                    return String.Format("DroneCharge- DroneId: {0}, StationId: {1}"
                        , DroneId, StationId);
                }

            }
        }
    }
}
