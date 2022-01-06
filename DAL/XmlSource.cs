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
                string droneChargePath = @"..\..\..\..\DAL\xml\DroneCharge.xml";
                var droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
                droneCharges.Clear();// ניקוי רשימת הרחפנים בטעינה ע"מ שלא יהיו בעיות
                XMLTools.SaveListToXMLSerializer(droneCharges, droneChargePath);


                DalApi.IDal D1 = DalApi.DalFactory.GetDal("1");//קריאה לבנאי שמתאחל
                XElement stationRoot;
                XElement configRoot;

                string stationPath = @"..\..\..\..\DAL\xml\Station.xml";
                string customerPath = @"..\..\..\..\DAL\xml\Customer.xml";
                string dronePath = @"..\..\..\..\DAL\xml\Drone.xml";
                string parcelPath = @"..\..\..\..\DAL\xml\Parcel.xml";
                string configPath = @"..\..\..\..\DAL\xml\Config.xml";

                stationRoot = new XElement("stations");
                configRoot = new XElement("Confing");

                foreach (var item in DataSource.listStations)
                {
                    XElement id = new XElement("Id", item.Id);
                    XElement name = new XElement("Name", item.Name);
                    XElement longitude = new XElement("Longitude", item.Longitude);
                    XElement latitude = new XElement("Latitude", item.Latitude);
                    XElement availableStations = new XElement("AvailableStations", item.AvailableStations);
                    XElement station = new XElement("Station", id, name, longitude, latitude, availableStations);
                    stationRoot.Add(station);
                }
                stationRoot.Save(stationPath);
                XElement CounterForParcels = new XElement("CounterForParcels", DataSource.Config.CounterForParcels);
                XElement available = new XElement("available", DataSource.Config.available);
                XElement lightWeight = new XElement("lightWeight", DataSource.Config.lightWeight);
                XElement mediumWeight = new XElement("mediumWeight", DataSource.Config.mediumWeight);
                XElement heavyWeight = new XElement("heavyWeight", DataSource.Config.heavyWeight);
                XElement chargingRate = new XElement("chargingRate", DataSource.Config.chargingRate);
                configRoot.Add(CounterForParcels, available, lightWeight, mediumWeight, heavyWeight, chargingRate);
                configRoot.Save(configPath);

                XMLTools.SaveListToXMLSerializer(DataSource.listCustomers, customerPath);
                XMLTools.SaveListToXMLSerializer(DataSource.listDrones, dronePath);
                XMLTools.SaveListToXMLSerializer(DataSource.listDroneCharges, droneChargePath);
                XMLTools.SaveListToXMLSerializer(DataSource.listParcels, parcelPath);

            }
        }
    }
}
