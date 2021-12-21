using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace BO
{
    /// <summary>
    /// ישות לוגית חבילה לרשימה
    /// </summary>
    public class ParcelToList
    {
        /// <summary>
        /// מספר מזהה חבילה
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// שם לקוח שולח
        /// </summary>
        public string SenderName { get; set; }
        /// <summary>
        /// שם לקוח מקבל
        /// </summary>
        public string RecipientName { get; set; }
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

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}

