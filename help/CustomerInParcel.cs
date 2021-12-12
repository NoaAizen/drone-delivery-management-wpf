using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BlApi
{
    namespace BO
    {

        public class CustomerInParcel//לקוח ברשימה
        {
            public int Id { get; set; }// מספר מזהה
            public string Name { get; set; }// שם לקוח

            public override string ToString()
            {
                return this.ToStringProperty();
            }
        }

    }
}

