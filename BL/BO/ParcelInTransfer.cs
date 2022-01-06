using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BO
{
    /// <summary>
    /// ישות לוגית חבילה בהעברה
    /// </summary>
    public class ParcelInTransfer
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
        /// מצב משלוח חבילה- ממתין לאיסוף \ בדרך ליעד
        /// </summary>
        public bool ParcelStatus { get; set; }
        /// <summary>
        /// לקוח בחבילה- השולח
        /// </summary>
        public CustomerInParcel CustomerInParcelSender { get; set; }
        /// <summary>
        /// לקוח בחבילה -המקבל
        /// </summary>
        public CustomerInParcel CustomerInParcelRecipient { get; set; }
        /// <summary>
        /// מיקום איסוף
        /// </summary>
        public Location CollectionLocation { get; set; }
        /// <summary>
        /// מיקום יעד אספקה
        /// </summary>
        public Location DeliveryDestinationLocation { get; set; }
        /// <summary>
        /// מרחק הובלה
        /// </summary>
        public double TransportDistance { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}


