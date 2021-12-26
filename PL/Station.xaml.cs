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
            notEnablFildes();
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
            stationPo.Name = nameText.Text;
            int chargeSlots = int.Parse(chargeSlotsText.Text);
            try
            {
                bl.UpdateStation(stationPo.Id, stationPo.Name, chargeSlots);
                RefreshEvent(this, EventArgs.Empty);
                convertToPo(stationPo, bl.GetStation(stationPo.Id));
                MessageBox.Show("sucssesed");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// פונקציה להפעלת שדות להיות לא זמינים 
        /// </summary>
        public void notEnablFildes()//
        {
            idText.IsEnabled = false;
            longitude.IsEnabled = false;
            latitude.IsEnabled = false;
        }
        /// <summary>
        /// /פונקציה שעושה המרה בשביל הזרימת מידע
        /// </summary>
        /// <param name="dronePo">רחפן של PL</param>
        /// <param name="d">רחפן של BO</param>
        public void convertToPo(StationPo stationPo, BO.Station s)
        {
            stationPo.Id = s.Id;
            stationPo.Name = s.Name;
            stationPo.AvailableStations = s.AvailableStations;
            stationPo.Latitude = s.Location.Latitude;
            stationPo.Longitude = s.Location.Longitude;
        }
    }
}
