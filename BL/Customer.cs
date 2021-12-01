using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IBL
{
    namespace BO
    {
        public class Customer
        {
            public int Id { get; set; }// מספר מזהה
            public string Name { get; set; }// שם לקוח
            public string Phone { get; set; }// מספר טלפון
            public Location Location { get; set; }//מיקום
            public List<ParcelAtCustomer> ParcelAtCustomerFromCustomer { get; set; }//רשימת חבילות אצל לקוח מהלקוח
            public List<ParcelAtCustomer> ParcelAtCustomerToCustomer { get; set; }//רשימת חבילות אצל לקוח אל הלקוח

            public override string ToString()
            {
                return this.ToStringProperty();
            }
        }

    }
}
