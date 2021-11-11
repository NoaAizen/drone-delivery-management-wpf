using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{

    namespace IBL
    {
        namespace BO
        {/// <summary>
        /// enums
        /// </summary>
            public enum WeightCategories {Light, Medium, Heavy }; // קטגוריית משקל- קל, ביניים, כבד

            public enum StatusDrone {Available, Maintenance, Delivery}; // מצב רחפן- פנוי, תחזוקה, משלוח

            public enum Priorities { Normal,Fast, Emergency }; // עדיפות- רגיל, מהיר, חירום

            public enum StatusParcel { Defined, Associated, Collected, Supplied }; // מצב חבילה -הוגדרה, שויכה, נאספה, סופקה

        }
    }
}