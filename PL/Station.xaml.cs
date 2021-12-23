using BlApi;
using BO;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for Station.xaml
    /// </summary>
    public partial class Station : Window
    {
        private IBL bl;
        private StationToList selectedItem;
        public event EventHandler RefreshEvent; //שדה בשביל הרענון
        private StationPo stationPo;//שדה בשביל המרת מידע

        public Station(BlApi.IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            AddStationGrid.IsEnabled = true;
            AddStationGrid.Visibility = Visibility.Visible;
        }

        public Station(IBL bl, StationToList selectedItem)
        {
            InitializeComponent();
            this.bl = bl;
            this.selectedItem = selectedItem;
            Actions.IsEnabled = true;
            Actions.Visibility = Visibility.Visible;
            BO.Station station = bl.GetStation(selectedItem.Id);
            stationPo = new()
            {
                Id = station.Id,
                Name = station.Name,
                AvailableStations = station.AvailableStations,
                Longitude = station.Location.Longitude,
                Latitude = station.Location.Latitude,
                DroneInChargingsList=station.DroneInChargingsList
            };
            Actions.DataContext = stationPo;
            
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddNewStationClick(object sender, RoutedEventArgs e)
        {
            double longitude = double.Parse(longitudeText.Text);
            double latitude = double.Parse(latitudeText.Text);
            BO.Station station = new()
            {
                Id = int.Parse(id.Text),
                Name = name.Text,
                Location = new() { Longitude = longitude, Latitude = latitude },
                AvailableStations = int.Parse(chargeSlots.Text)
            };
            try
            {
                bl.AddStation(station);
                MessageBox.Show("sucssesed");
                this.Close();
                RefreshEvent(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            //notEnablFildes();
            //modelText.IsEnabled = true;//עדכון של זמינות המודל
            //drone.Model = modelText.Text;
            //try
            //{
            //    bl.UpdateDroneModel(drone.Id, drone.Model);
            //    convertToPo(drone, bl.GetDrone(drone.Id));
            //    MessageBox.Show("sucssesed");
            //    RefreshEvent(this, EventArgs.Empty);

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
    }
}
