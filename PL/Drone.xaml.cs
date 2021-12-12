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
        public event EventHandler RefreshEvent; 
        public PL.DroneLists d;
        private DronePO drone;
        private IBL.IBL bl;
        private DroneToList selectedItem;
        TimeSpan t;
        public Drone(IBL.IBL bl)//הוספה
        {
            InitializeComponent();
            this.bl = bl;
            AddDroneGrid.IsEnabled = true;
            AddDroneGrid.Visibility = Visibility.Visible;
            maxWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));

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
            statusText.ItemsSource = Enum.GetValues(typeof(StatusDrone));
            maxWeightText.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            notEnablFildes();


        }

        private void AddNewDroneClick(object sender, RoutedEventArgs e)//פונקציית הוספת חרפן
        {
            int stationNumber = int.Parse(stationId.Text);
            DroneToList drone = new DroneToList()
            {
                Id = int.Parse(id.Text),
                Model = model.Text,
                MaxWeight= (WeightCategories)maxWeight.SelectedItem
            };
            try
            {
                bl.AddDrone(drone, stationNumber);
                MessageBox.Show("sucssesed");
                this.Close();
                RefreshEvent(this, EventArgs.Empty);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateModelClick(object sender, RoutedEventArgs e)//עדכון מודל
        {
            notEnablFildes();
            modelText.IsEnabled = true;
            drone.Model = modelText.Text;
            try
            {
                bl.UpdateDroneModel(drone.Id, drone.Model);
                convertToPo(drone, bl.GetDrone(drone.Id));
                MessageBox.Show("sucssesed");
                RefreshEvent(this, EventArgs.Empty);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)//רשימה
        {
            this.Close();
        }

        private void ChargingClick(object sender, RoutedEventArgs e)//שליחת רחפן לטעינה
        {
            try { 
            bl.SendingDroneForCharging(drone.Id);
            convertToPo(drone, bl.GetDrone(drone.Id));
                MessageBox.Show("sucssesed");
                RefreshEvent(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void ReleaseClick(object sender, RoutedEventArgs e)//שחרור רחפן מטעינה
        {
           try{
            bl.ReleaseDroneFromCharging(drone.Id, t);
            convertToPo(drone, bl.GetDrone(drone.Id));
                MessageBox.Show("sucssesed");
                RefreshEvent(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CollectionClick(object sender, RoutedEventArgs e)//איסוף חבילה
        {
           try{
            bl.CollectionParcelFromDrone(drone.Id);
            convertToPo(drone, bl.GetDrone(drone.Id));
                MessageBox.Show("sucssesed");
                RefreshEvent(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void AssignmentClick(object sender, RoutedEventArgs e)//פונקציית איסוף חבילה
        {
            try
            {
                bl.UpdateDroneToParcel(drone.Id);
                convertToPo(drone, bl.GetDrone(drone.Id));
                MessageBox.Show("sucssesed");
                RefreshEvent(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void DeliveryClick(object sender, RoutedEventArgs e)//אספקת חבילה
        {
          try{
            bl.DeliveryParcelByDrone(drone.Id);
            convertToPo(drone, bl.GetDrone(drone.Id));
                MessageBox.Show("sucssesed");
                RefreshEvent(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

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

      public void notEnablFildes()//בלי עדכון מודל
        {
            idText.IsEnabled = false;
            statusText.IsEnabled = false;
            parcelNumberText.IsEnabled = false;
            batteryText.IsEnabled = false;
            maxWeightText.IsEnabled = false;
            longitudeText.IsEnabled = false;
            latitudeText.IsEnabled = false;

        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       
    }
}
