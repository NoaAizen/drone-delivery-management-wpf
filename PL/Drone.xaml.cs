using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for Drone.xaml
    /// </summary>
    public partial class Drone : Window
    {
        private DronePO drone;
        private IBL.IBL bl;
        private DroneToList selectedItem;
        TimeSpan t;
        public Drone(IBL.IBL bl)//הוספה
        {
            this.bl = bl;
            InitializeComponent();
            AddDroneGrid.IsEnabled = true;
            AddDroneGrid.Visibility = Visibility.Visible;

        }

        public Drone(IBL.IBL bl, DroneToList selectedItem)//פעולות
        {
            this.bl = bl;
            this.selectedItem = selectedItem;
            InitializeComponent();
            Actions.IsEnabled = true;
            Actions.Visibility = Visibility.Visible;
            drone = new();
            drone.Battery = selectedItem.Battery;
            drone.Id = selectedItem.Id;
            drone.Status = selectedItem.Status;
            drone.MaxWeight = selectedItem.MaxWeight;
            drone.Model = selectedItem.Model;
            drone.Longitude = selectedItem.CurrentLocation.Longitude;
            drone.Latitude = selectedItem.CurrentLocation.Latitude;
            drone.ParcelTransferredNumber = selectedItem.ParcelTransferredNumber;
            statusText.DataContext = drone;
            idText.DataContext = drone;
            parcelNumberText.DataContext = drone;
            modelText.DataContext = drone;
            batteryText.DataContext = drone;
            maxWeightText.DataContext = drone;
            latitudeText.DataContext = drone;
            longitudeText.DataContext = drone;
        }

        private void AddNewDroneClick(object sender, RoutedEventArgs e)
        {
            int stationNumber = int.Parse(stationId.Text);
            int temp = int.Parse(maxWeight.Text);
            DroneToList drone = new()
            {
                Id = int.Parse(id.Text),
                Model = model.Text,
                MaxWeight = (WeightCategories)temp
            };
            bl.AddDrone(drone, stationNumber);
        }


        private void main(object sender, RoutedEventArgs e)
        {
            new DroneLists(bl).Show();
        }

        private void UpdateModelClick(object sender, RoutedEventArgs e)//עדכון מודל
        {
            drone.Model = modelText.Text;
            bl.UpdateDroneModel(drone.Id, drone.Model);

        }

        private void Button_Click(object sender, RoutedEventArgs e)//רשימה
        {
            new DroneLists(bl).Show();
        }

        private void ChargingClick(object sender, RoutedEventArgs e)//שליחת רחפן לטעינה
        {
            bl.SendingDroneForCharging(drone.Id);
            convertToPo(drone, bl.GetDrone(drone.Id));

        }

        private void ReleaseClick(object sender, RoutedEventArgs e)//שחרור רחפן מטעינה
        {
            bl.ReleaseDroneFromCharging(drone.Id, t);
            convertToPo(drone, bl.GetDrone(drone.Id));

        }

        private void CollectionClick(object sender, RoutedEventArgs e)//איסוף חבילה
        {
            bl.CollectionParcelFromDrone(drone.Id);
            convertToPo(drone, bl.GetDrone(drone.Id));

        }
        private void DeliveryClick(object sender, RoutedEventArgs e)//אספקת חבילה
        {
            bl.DeliveryParcelByDrone(drone.Id);
            convertToPo(drone, bl.GetDrone(drone.Id));

        }
        public void convertToPo(DronePO dronePo, IBL.BO.Drone d)//פונקציה שעושה המרה בשביל הזרימת מידע
        {
            dronePo.Battery = d.Battery;
            dronePo.Id = d.Id;
            dronePo.Status = d.Status;
            dronePo.MaxWeight = d.MaxWeight;
            dronePo.Model = d.Model;
            //dronePo.ParcelTransferredNumber = d.ParcelInTransfer.Id;
            dronePo.Latitude = d.CurrentLocation.Latitude;
            dronePo.Longitude = d.CurrentLocation.Longitude;
        }


    }
}
