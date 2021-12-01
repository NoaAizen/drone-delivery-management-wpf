using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IBL
{
    namespace BO
    {
        public class ParcelToList//חבילה לרשימה
        {
            public int Id { get; set; }// מספר מזהה חבילה
            public string SenderName { get; set; }//שם לקוח שולח
            public string RecipientName { get; set; }//שם לקוח מקבל
            public WeightCategories Weight { get; set; }// קטגורית משקל
            public Priorities Priority { get; set; } // עדיפות
            public StatusParcel StatusParcel { get; set; } //מצב חבילה -הוגדרה, שויכה, נאספה, סופקה

            public override string ToString()
            {
                return this.ToStringProperty();
            }
        }
    }
}
