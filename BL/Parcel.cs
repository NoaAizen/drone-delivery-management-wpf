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
            class Parcel
            {
                public int Id { get; set; }// מספר מזהה חבילה
                public CustomerInParcel CustomerInParcelSender { get; set; }//לקוח בחבילה (השולח)
                public CustomerInParcel CustomerInParcelRecipient { get; set; }//לקוח בחבילה (המקבל)
                public WeightCategories Weight { get; set; }// קטגורית משקל
                public Priorities Priority { get; set; } // עדיפות

            }
        }
    }
}