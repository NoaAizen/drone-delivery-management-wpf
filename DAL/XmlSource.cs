using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DalObject
{
    static class XmlSource
    {
        internal class Config
        {
            public static void Initialize()
            {
                XElement stationRoot;
                string FPath = @"C:\Users\User\source\repos\OriyaAharoni\dotNet5782_3394_8965\DAL\Station.xml";
                stationRoot = new XElement("stations");

                foreach (var item in DataSource.listStations)
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
}
