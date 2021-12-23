using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BO
{
    /// <summary>
    /// ישות לוגית לקוח לרשימה
    /// </summary>
    public class CustomerToList
    {
        /// <summary>
        /// מספר מזהה
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// שם לקוח
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// מספר טלפון
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// מספר חבילות ששלח וסופקו
        /// </summary>
        public int NumberOfParcelSentAndDelivered { get; set; }
        /// <summary>
        /// מספר חבילות ששלח אך עוד לא סופקו
        /// </summary>
        public int NumberOfParcelSentButNotYetDelivered { get; set; }
        /// <summary>
        /// מספר חבילות שקיבל
        /// </summary>
        public int NumberOfParcelReceived { get; set; }
        /// <summary>
        /// מספר חבילות שבדרך אל הלקוח
        /// </summary>
        public int NumberOfParcelOnTheWayToTheCustomer { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}

