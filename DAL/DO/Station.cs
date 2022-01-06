using System;

namespace DO
{
    /// <summary>
    /// מידע על התחנה
    /// </summary>
    public struct Station
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
        /// קו אורך
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// קו רוחב
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// מספר עמודות הטענה פנויות
        /// </summary>
        public int AvailableStations { get; set; } 

        /// <summary>
        /// פונקצית טוסטרניג(Tostring)- בשביל הדפסה
        /// </summary>
        /// <returns>מחרוזת עם שדות התחנה</returns>
        public override string ToString()
        {
            return String.Format("Station- Id: {0}, Name: {1}, Longitude: {2}, Latitude: {3}, AvailableStations: {4}"
                , Id, Name, Longitude, Latitude, AvailableStations);
        }
    }
}


