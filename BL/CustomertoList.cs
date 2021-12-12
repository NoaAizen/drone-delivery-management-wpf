using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BO
{
    public class CustomertoList//לקוח לרשימה
    {
        public int Id { get; set; }// מספר מזהה
        public string Name { get; set; }// שם לקוח
        public string Phone { get; set; }// מספר טלפון
        public int NumberOfParcelSentAndDelivered { get; set; }//מספר חבילות ששלח וסופקו
        public int NumberOfParcelSentButNotYetDelivered { get; set; }//מספר חבילות ששלח אך עוד לא סופקו
        public int NumberOfParcelReceived { get; set; }//מספר חבילות שקיבל
        public int NumberOfParcelOnTheWayToTheCustomer { get; set; }//מספר חבילות שבדרך אל הלקוח

        public override string ToString()
        {
            return this.ToStringProperty();
        }


    }
}

