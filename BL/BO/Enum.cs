using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace BO
{
    /// <summary>
    /// enums
    /// </summary>
    /// 
    ///

    /// <summary>
    ///   קטגוריית משקל- קל, ביניים, כבד
    /// </summary
    public enum WeightCategories { Light, Medium, Heavy }; // 

    /// <summary>
    /// מצב רחפן- פנוי, תחזוקה, משלוח
    /// </summary>
    public enum StatusDrone { Available, Maintenance, Delivery }; // 
    /// <summary>
    /// עדיפות- רגיל, מהיר, חירום
    /// </summary>
    public enum Priorities { Normal, Fast, Emergency }; // 
    /// <summary>
    /// מצב חבילה -הוגדרה, שויכה, נאספה, סופקה
    /// </summary>
    public enum StatusParcel { Defined, Associated, Collected, Supplied }; 

}

