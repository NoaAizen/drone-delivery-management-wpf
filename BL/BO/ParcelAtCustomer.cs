using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace BO
{
    /// <summary>
    /// ישות לוגית חבילה אצל לקוח
    /// </summary>
    public class ParcelAtCustomer
    {
        /// <summary>
        /// מספר מזהה חבילה
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// קטגורית משקל
        /// </summary>
        public WeightCategories Weight { get; set; }
        /// <summary>
        /// עדיפות
        /// </summary>
        public Priorities Priority { get; set; }
        /// <summary>
        /// מצב חבילה -הוגדרה, שויכה, נאספה, סופקה
        /// </summary>
        public StatusParcel StatusParcel { get; set; }
        /// <summary>
        ///  לקוח בחבילה - המקור\היעד : הצד השני של המשלוח - המקבל עבור השולח והשולח עבור המקבל
        /// </summary>
        public CustomerInParcel CustomerInParcel { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}


