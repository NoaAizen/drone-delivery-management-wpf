using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace BO
{
    /// <summary>
    /// ישות לוגית של לקוח
    /// </summary>
    public class Customer
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
        /// מיקום
        /// </summary>
        public Location Location { get; set; }
        /// <summary>
        /// רשימת חבילות אצל לקוח מהלקוח
        /// </summary>
        public List<ParcelAtCustomer> ParcelAtCustomerFromCustomer { get; set; }
        /// <summary>
        /// רשימת חבילות אצל לקוח אל הלקוח
        /// </summary>
        public List<ParcelAtCustomer> ParcelAtCustomerToCustomer { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }

}

