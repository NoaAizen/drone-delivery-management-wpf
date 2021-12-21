using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace BO
{
    /// <summary>
    /// ישות לוגית תחנה לרשימה
    /// </summary>
    public class StationToList//תחנה לרשימה
    {
        /// <summary>
        /// מספר מזהה
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// שם תחנה
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// מספר עמדות טעינה פנויות
        /// </summary>
        public int AvailableStations { get; set; }
        /// <summary>
        /// מספר עמדות טעינה תפוסות
        /// </summary>
        public int NotAvailableStations { get; set; } 

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}

