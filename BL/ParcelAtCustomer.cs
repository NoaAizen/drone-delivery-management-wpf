using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IBL
{
    namespace BO
    {
        public class ParcelAtCustomer//חבילה אצל לקוח
        {
            public int Id { get; set; }// מספר מזהה חבילה
            public WeightCategories Weight { get; set; }// קטגורית משקל
            public Priorities Priority { get; set; } // עדיפות
            public StatusParcel StatusParcel { get; set; } //מצב חבילה -הוגדרה, שויכה, נאספה, סופקה
            public CustomerInParcel CustomerInParcel { get; set; }// לקוח בחבילה - המקור\היעד (הצד השני של המשלוחחבילה - המקבל עבור השולח והשולח עבור המקבל)

            public override string ToString()
            {
                return this.ToStringProperty();
            }
        }
    }
}

