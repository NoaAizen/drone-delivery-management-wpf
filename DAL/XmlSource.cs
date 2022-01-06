using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DO;

namespace DalObject
{
    static class XmlSource
    {
        internal class Config
        {
            public static void Initialize()
            {
                //DalApi.IDal D1 = DalApi.DalFactory.GetDal("1");//קריאה לבנאי שמתאחל
                //XElement stationRoot;
                ////string FPath = @"DAL\xml\Station.xml";
                //string stationPath = @"C:\Users\User\source\repos\OriyaAharoni\dotNet5782_3394_8965\DAL\xml\Station.xml";
                //string customerPath = @"C:\Users\User\source\repos\OriyaAharoni\dotNet5782_3394_8965\DAL\xml\Customer.xml";
                //string dronePath = @"C:\Users\User\source\repos\OriyaAharoni\dotNet5782_3394_8965\DAL\xml\Drone.xml";
                //string droneChargePath = @"C:\Users\User\source\repos\OriyaAharoni\dotNet5782_3394_8965\DAL\xml\DroneCharge.xml";
                //stationRoot = new XElement("stations");

                //foreach (var item in DataSource.listStations)
                //{
                //    XElement id = new XElement("Id", item.Id);
                //    XElement name = new XElement("Name", item.Name);
                //    XElement longitude = new XElement("Longitude", item.Longitude);
                //    XElement latitude = new XElement("Latitude", item.Latitude);
                //    XElement availableStations = new XElement("AvailableStations", item.AvailableStations);
                //    XElement station = new XElement("Station", id, name, longitude, latitude, availableStations);
                //    stationRoot.Add(station);
                //}
                //stationRoot.Save(stationPath);

                //XMLTools.SaveListToXMLSerializer(DataSource.listCustomers, customerPath);
                //XMLTools.SaveListToXMLSerializer(DataSource.listDrones, dronePath);
                //XMLTools.SaveListToXMLSerializer(DataSource.listDroneCharges, droneChargePath);
            }
        }
    }
}
