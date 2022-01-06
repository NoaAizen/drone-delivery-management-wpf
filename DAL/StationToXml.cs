using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DO;


namespace DAL
{
    public class StationToXml
    {
        static XElement stationRoot;
        static string FPath = @"..\..\..\..\DAL\Station.xml";

        public static void SaveStationList(List<Station> stationList)
        {
            stationRoot = new XElement("stations");
           
            foreach (var item in stationList)
            {
                XElement id = new XElement("id", item.Id);
                XElement name = new XElement("name", item.Name);
                XElement longitude = new XElement("longitude", item.Longitude);
                XElement latitude = new XElement("latitude", item.Latitude);
                XElement availableStations = new XElement("availableStations", item.AvailableStations);
                XElement station = new XElement("station", id, name, longitude, latitude, availableStations);
                stationRoot.Add(station);
            }
            stationRoot.Save(FPath);

        }
    }
}
