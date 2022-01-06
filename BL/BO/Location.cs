using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace BO
{
    /// <summary>
    /// ישות לוגית של מיקום
    /// </summary>
    public class Location
    {
        /// <summary>
        /// קו אורך
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// קו רוחב
        /// </summary>
        public double Latitude { get; set; } 

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}

