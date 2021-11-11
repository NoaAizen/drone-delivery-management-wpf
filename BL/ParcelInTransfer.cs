using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    namespace IBL
    {
        namespace BO
        {
            class ParcelInTransfer//חבילה בהעברה
            {
                public int Id { get; set; }// מספר מזהה חבילה
                public WeightCategories Weight { get; set; }// קטגורית משקל
                public Priorities Priority { get; set; } // עדיפות
                public bool ParcelStatus { get; set; }//מצב משלוח חבילה
                public CustomerInParcel CustomerInParcelSender { get; set; }//לקוח בחבילה- השולח
                public CustomerInParcel CustomerInParcelRecipient { get; set; }//לקוח בחבילה -המקבל
                public Location CollectionLocation { get; set; }// מיקום איסוף
                public Location DeliveryDestinationLocation { get; set; }// מיקום יעד אספקה
                public double TransportDistance{ get; set; }//מרחק הובלה
            }
        }
    }
}
