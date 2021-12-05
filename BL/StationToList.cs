using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IBL
{
    namespace BO
    {
        public class StationToList//תחנה לרשימה
        {
            public int Id { get; set; }// מספר מזהה
            public string Name { get; set; }// שם תחנה
            public int AvailableStations { get; set; }// מספר עמדות טעינה פנויות
            public int NotAvailableStations { get; set; }// מספר עמדות טעינה תפוסות

            public override string ToString()
            {
                return this.ToStringProperty();
            }
        }
    }
}
