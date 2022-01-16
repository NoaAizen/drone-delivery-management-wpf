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
using BO;
using System.ComponentModel;


namespace PL
{
    /// <summary>
    /// Interaction logic for Drone.xaml
    /// </summary>
    public partial class Drone : Window
    {
        public event EventHandler RefreshEvent; //שדה בשביל הרענון
        private DronePO drone;//שדה בשביל המרת מידע 
        private BlApi.IBL bl;//שדה בשביל שימוש הנתונים בBL
        private DroneToList selectedItem;//rjpi
        TimeSpan t;
        BackgroundWorker worker;
        private bool start;
        



        /// <summary>
        /// בנאי של הוספת חלון
        /// </summary>
        /// <param name="bl">מקבל את רחפן של BL</param>
        public Drone(BlApi.IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            AddDroneGrid.IsEnabled = true;
            AddDroneGrid.Visibility = Visibility.Visible;
            Maxweight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            AddNewDrone.IsEnabled = false;



        }
        private void moveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        /// <summary>
        /// בנאי של פעולות
        /// </summary>
        /// <param name="bl">רחפן של IB</param>
        /// <param name="selectedItem">חלון הקודם </param>
        public Drone(BlApi.IBL bl, DroneToList selectedItem)//
        {
            this.bl = bl;
            this.selectedItem = selectedItem;
            InitializeComponent();
            Actions.IsEnabled = true;
            Actions.Visibility = Visibility.Visible;
            drone = new();
            drone.Battery = selectedItem.Battery;
            drone.Id = selectedItem.Id;
            drone.ParcelInTransfer = selectedItem.ParcelInTransfer;
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
            if (drone.Status == StatusDrone.Delivery)
            {
                ParcelTransfer.IsEnabled = true;
            }

        }
        /// <summary>
        /// פונקציית הוספת חרפן
        /// </summary>
        /// <param name="sender">חלון</param>
        /// <param name="e">אירוע</param>
        private void AddNewDroneClick(object sender, RoutedEventArgs e)//
        {
            int stationNumber = int.Parse(stationId.Text);
            DroneToList drone = new DroneToList()
            {
                Id = int.Parse(id.Text),
                Model = model.Text,
                MaxWeight = (WeightCategories)Maxweight.SelectedItem
            };
            try
            {
                bl.AddDrone(drone, stationNumber);
                MessageBox.Show("sucssesed");
                this.Close();
                RefreshEvent(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //////פונקציות בשביל הפעולות על הרחפן
        /// <summary>
        /// פונקמיה בשביל כפתור לעדכון מודל
        /// </summary>
        /// <param name="sender">חלון</param>
        /// <param name="e">אירוע</param>
        private void UpdateModelClick(object sender, RoutedEventArgs e)
        {
            notEnablFildes();
            modelText.IsEnabled = true;//עדכון של זמינות המודל
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


        /// <summary>
        /// פונקציב בשביל כפתור לשליחת רחפן לטעינה
        /// </summary>
        /// <param name="sender">חלון</param>
        /// <param name="e"></param>
        private void ChargingClick(object sender, RoutedEventArgs e)
        {
            try
            {
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
        /// <summary>
        ///פונקציית בשביל כפתור לשחרור רחפן מטעינה
        /// </summary>
        /// <param name="sender">חלון</param>
        /// <param name="e">אירוע</param>
        private void ReleaseClick(object sender, RoutedEventArgs e)//
        {
            try
            {
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
        /// <summary>
        /// פונקציה בשביל כפתור של איסוף חבילה
        /// </summary>
        /// <param name="sender">חלון</param>
        /// <param name="e">אירוע</param>
        private void CollectionClick(object sender, RoutedEventArgs e)//
        {
            try
            {

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
        /// <summary>
        /// פונקציית בשביל כפתור לשיוך חבילה
        /// </summary>
        /// <param name="sender">חלון</param>
        /// <param name="e">אירוע</param>
        private void AssignmentClick(object sender, RoutedEventArgs e)
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
        /// <summary>
        ///פונקצית בשביל כפתור אספקת חבילה
        /// </summary>
        /// <param name="sender">חלון המתאים</param>
        /// <param name="e">אירוע</param>
        private void DeliveryClick(object sender, RoutedEventArgs e)
        {
            try
            {
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
        /// <summary>
        /// /פונקציה שעושה המרה בשביל הזרימת מידע
        /// </summary>
        /// <param name="dronePo">רחפן של PL</param>
        /// <param name="d">רחפן של BO</param>
        public void convertToPo(DronePO dronePo, BO.Drone d)
        {
            dronePo.Battery = d.Battery;
            dronePo.Id = d.Id;
            dronePo.Status = d.Status;
            dronePo.MaxWeight = d.MaxWeight;
            dronePo.Model = d.Model;
            //dronePo.ParcelTransferredNumber = d.ParcelInTransfer.Id;
            dronePo.Latitude = d.CurrentLocation.Latitude;
            dronePo.Longitude = d.CurrentLocation.Longitude;
            //if (dronePo.Status == StatusDrone.Delivery)
            //{
            //    new ParcelInTransferWindow(bl, dronePo).Show();
            //}
        }
        /// <summary>
        /// פונקציה להפעלת שדות להיות לא זמינים 
        /// </summary>
        public void notEnablFildes()//
        {
            idText.IsEnabled = false;
            statusText.IsEnabled = false;
            parcelNumberText.IsEnabled = false;
            batteryText.IsEnabled = false;
            maxWeightText.IsEnabled = false;
            longitudeText.IsEnabled = false;
            latitudeText.IsEnabled = false;
            ParcelTransfer.IsEnabled = false;
        }
        /// <summary>
        /// פונקציה לסגירת חלון הוספה
        /// </summary>
        /// <param name="sender" >חלון</param>
        /// <param name="e">אירוע</param>
        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
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
        /// מעבר לחלון של חבילה בהעברה
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParcelInTransferClick(object sender, RoutedEventArgs e)
        {
            new ParcelInTransferWindow(bl, drone).Show();
        }
        /// <summary>
        /// פונקציה של נעילה אירוע תעודת זהות בשביל הסופת נתונים
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IdClick(object sender, TextChangedEventArgs e)
        {
            if (id.Text != "" && model.Text != "" && Maxweight.SelectedItem != null && stationId.Text != "")
                AddNewDrone.IsEnabled = true;
            else
                AddNewDrone.IsEnabled = false;
        }
        /// <summary>
        /// פונקציה של נעילה אירוע מודל בשביל הסופת נתונים
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modelClick(object sender, TextChangedEventArgs e)
        {

            if (id.Text != "" && model.Text != "" && Maxweight.SelectedItem != null && stationId.Text != "")
                AddNewDrone.IsEnabled = true;
            else
                AddNewDrone.IsEnabled = false;
        }
        /// <summary>
        /// פונקציה של נעילה אירוע משקל בשביל הסופת נתונים
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void maxclick(object sender, SelectionChangedEventArgs e)
        {

            if (id.Text != "" && model.Text != "" && Maxweight.SelectedItem != null && stationId.Text != "")
                AddNewDrone.IsEnabled = true;
            else
                AddNewDrone.IsEnabled = false;
        }
        /// <summary>
        /// פונקציה של נעילה אירוע תחנה בשביל הסופת נתונים
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stationidclick(object sender, TextChangedEventArgs e)
        {
            if (id.Text != "" && model.Text != "" && Maxweight.SelectedItem != null && stationId.Text != "")
                AddNewDrone.IsEnabled = true;
            else
                AddNewDrone.IsEnabled = false;
        }

        private void AutomaticClick(object sender, RoutedEventArgs e)
        {
            ParcelTransfer.Visibility = Visibility.Hidden;
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            start = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void updateDrone() => worker.ReportProgress(0);
        private bool checkStop() => worker.CancellationPending;

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            bl.StartDroneSimulator(drone.Id, updateDrone, checkStop);

        }
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                convertToPo(drone, bl.GetDrone(drone.Id));
                RefreshEvent(this, EventArgs.Empty);
                MessageBox.Show(drone.Status.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //int progress = e.ProgressPercentage;
        }
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            object result = e.Result;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (worker.WorkerSupportsCancellation == true)
                // Cancel the asynchronous operation.
                worker.CancelAsync();
        }



    }
}
