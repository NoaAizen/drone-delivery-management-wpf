using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace BO
{
    /// <summary>
    /// ישות לוגית לקוח בחבילה
    /// </summary>
    public class CustomerInParcel
    {
        /// <summary>
        /// מספר מזהה
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// שם לקוח
        /// </summary>
        public string Name { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }

}


