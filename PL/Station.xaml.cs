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
        private BO.Station selectedItem;
        public event EventHandler RefreshEvent; //שדה בשביל הרענון
        private StationPo stationPo;//שדה בשביל המרת מידע
        /// <summary>
        /// פונקציה לביטול הלחצנים הרגילים של סגירה והגדלה
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void moveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        /// <summary>
        /// בנאי הוספת חלון
        /// </summary>
        /// <param name="bl"></param>
        public Station(BlApi.IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            AddStationGrid.IsEnabled = true;
            AddStationGrid.Visibility = Visibility.Visible;
            AddNewStation.IsEnabled = false;
        }
        /// <summary>
        /// בנאי של פעןלןת
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="station"></param>
        public Station(IBL bl, BO.Station station)
        {
            
            InitializeComponent();
            this.bl = bl;
            this.selectedItem = station;
            Actions.IsEnabled = true;
            Actions.Visibility = Visibility.Visible;
            notEnablFildes();
            //BO.Station station = bl.GetStation(selectedItem.Id);
            stationPo = new()
            {
                Id = station.Id,
                Name = station.Name,
                AvailableStations = station.AvailableStations,
                Longitude = station.Location.Longitude,
                Latitude = station.Location.Latitude,
                DroneInChargingsList = station.DroneInChargingsList
            };
            Actions.DataContext = stationPo;
           

        }
       /// <summary>
       /// סגירת חלון
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// הוספת תחנה
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// עכדכון תחנה פונקציה
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// פונקציה בשביל סגירת חלון פעולות
        /// </summary>
        /// <param name="sender">חלון</param>
        /// <param name="e">אירוע</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// פונקציות של נעילה כפתור הוספה עד שלא מוספים את כל הנתונים
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IdClick(object sender, TextChangedEventArgs e)
        {
            if (id.Text != "" && longitudeText.Text != "" && name.Text != "" && latitudeText.Text != "" && chargeSlots.Text != "")
                AddNewStation.IsEnabled = true;
            else
                AddNewStation.IsEnabled = false;
        }

        private void NameClick(object sender, TextChangedEventArgs e)
        {
            if (id.Text != "" && longitudeText.Text != "" && name.Text != "" && latitudeText.Text != "" && chargeSlots.Text != "")
                AddNewStation.IsEnabled = true;
            else
                AddNewStation.IsEnabled = false;
        }

        private void longitudeTextClick(object sender, TextChangedEventArgs e)
        {
            if (id.Text != "" && longitudeText.Text != "" && name.Text != "" && latitudeText.Text != "" && chargeSlots.Text != "")
                AddNewStation.IsEnabled = true;
            else
                AddNewStation.IsEnabled = false;
        }

        private void latitudeTextClick(object sender, TextChangedEventArgs e)
        {
            if (id.Text != "" && longitudeText.Text != "" && name.Text != "" && latitudeText.Text != "" && chargeSlots.Text != "")
                AddNewStation.IsEnabled = true;
            else
                AddNewStation.IsEnabled = false;
        }

        private void ChargeSlotsClick(object sender, TextChangedEventArgs e)
        {
            if (id.Text != "" && longitudeText.Text != "" && name.Text != "" && latitudeText.Text != "" && chargeSlots.Text != "")
                AddNewStation.IsEnabled = true;
            else
                AddNewStation.IsEnabled = false;
        }
        /// <summary>
        /// המעבר לחלון רחפנים
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DroneChageClick(object sender, MouseButtonEventArgs e)
        {
            if(DroneCharge.SelectedItem!=null)
            {
                int id = ((BO.DroneInCharging)DroneCharge.SelectedItem).Id;
                BO.Drone drone = bl.GetDrone(id);
                DroneToList droneToList = new()
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = drone.MaxWeight,
                    Status = drone.Status,
                    Battery = drone.Battery,
                    ParcelInTransfer = drone.ParcelInTransfer,
                    CurrentLocation = drone.CurrentLocation,
                    //ParcelTransferredNumber = drone.ParcelInTransfer.Id
                };
                new Drone(bl, droneToList).Show();
            }
            
        }
        
    }
}
