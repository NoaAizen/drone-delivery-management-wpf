using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;





namespace BO
{
    /// <summary>
    /// ישות לוגית של חבילה
    /// </summary>
    public class Parcel
    {
        /// <summary>
        /// מספר מזהה חבילה
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// לקוח בחבילה- השולח
        /// </summary>
        public CustomerInParcel CustomerInParcelSender { get; set; }
        /// <summary>
        /// לקוח בחבילה -המקבל
        /// </summary>
        public CustomerInParcel CustomerInParcelRecipient { get; set; }
        /// <summary>
        /// קטגורית משקל
        /// </summary>
        public WeightCategories Weight { get; set; }
        /// <summary>
        /// עדיפות
        /// </summary>
        public Priorities Priority { get; set; }
        /// <summary>
        /// רחפן בחבילה
        /// </summary>
        public DroneInParcel DroneInParcel { get; set; }
        /// <summary>
        /// זמן יצירת חבילה למשלוח
        /// </summary>
        public DateTime? Requested { get; set; }
        /// <summary>
        /// זמן שיוך החבילה לרחפן
        /// </summary>
        public DateTime? Scheduled { get; set; }
        /// <summary>
        /// זמן איסוף חבילה מהשולח
        /// </summary>
        public DateTime? PickedUp { get; set; }
        /// <summary>
        /// זמן הגעת החבילה למקבל
        /// </summary>
        public DateTime? Delivered { get; set; } 

        public override string ToString()
        {
            return this.ToStringProperty();
        }

    }
}

