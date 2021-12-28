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
    /// Interaction logic for Parcel.xaml
    /// </summary>
    public partial class Parcel : Window
    {
        private IBL bl;
        //private ParcelToList selectedItem;
        private BO.Parcel selectedItem;

        public event EventHandler RefreshEvent; //שדה בשביל הרענון
        private ParcelPo parcelPo;//שדה בשביל המרת מידע

        public Parcel(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            AddParcelGrid.IsEnabled = true;
            AddParcelGrid.Visibility = Visibility.Visible;
            priority.ItemsSource = Enum.GetValues(typeof(Priorities));
            weight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }
        public Parcel(IBL bl, BO.Parcel parcel)
        {
            InitializeComponent();
            this.bl = bl;
            this.selectedItem = parcel;
            Actions.IsEnabled = true;
            Actions.Visibility = Visibility.Visible;
            notEnablFildes();
            //BO.Parcel parcel = bl.GetParcel(selectedItem.Id);
            parcelPo = new()
            {
                Id = parcel.Id,
                CustomerInParcelSender = parcel.CustomerInParcelSender,
                CustomerInParcelRecipient = parcel.CustomerInParcelRecipient,
                Weight = parcel.Weight,
                Priority = parcel.Priority,
                DroneInParcel = parcel.DroneInParcel,
                Requested = parcel.Requested,
                Scheduled = parcel.Scheduled,
                PickedUp = parcel.PickedUp,
                Delivered = parcel.Delivered
            };
            PriorityText.ItemsSource = Enum.GetValues(typeof(Priorities));
            weightText.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            Actions.DataContext = parcelPo;
        }

        //public Parcel(IBL bl, ParcelToList selectedItem)
        //{
        //    InitializeComponent();
        //    this.bl = bl;
        //    this.selectedItem = selectedItem;
        //    Actions.IsEnabled = true;
        //    Actions.Visibility = Visibility.Visible;
        //    notEnablFildes();
        //    BO.Parcel parcel = bl.GetParcel(selectedItem.Id);
        //    parcelPo = new()
        //    {
        //        Id = parcel.Id,
        //        CustomerInParcelSender = parcel.CustomerInParcelSender,
        //        CustomerInParcelRecipient = parcel.CustomerInParcelRecipient,
        //        Weight = parcel.Weight,
        //        Priority = parcel.Priority,
        //        DroneInParcel = parcel.DroneInParcel,
        //        Requested = parcel.Requested,
        //        Scheduled = parcel.Scheduled,
        //        PickedUp = parcel.PickedUp,
        //        Delivered = parcel.Delivered
        //    };
        //    PriorityText.ItemsSource = Enum.GetValues(typeof(Priorities));
        //    weightText.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        //    Actions.DataContext = parcelPo;
        //}

        //public Parcel(IBL bl, BO.Parcel selectedItem1) : this(bl)
        //{
        //    this.selectedItem1 = selectedItem1;
        //}

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddNewParcelClick(object sender, RoutedEventArgs e)
        {
            BO.Parcel parcel = new()
            {
                CustomerInParcelSender = new() { Id= int.Parse(senderId.Text) },
                CustomerInParcelRecipient = new() { Id= int.Parse(recipientId.Text) },
                Weight = (WeightCategories)weight.SelectedItem,
                Priority = (Priorities)priority.SelectedItem
            };
            try
            {
                int id= bl.AddParcel(parcel);
                RefreshEvent(this, EventArgs.Empty);
                MessageBox.Show("sucssesed\nparcel's id: "+id);
                this.Close();
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
            requestedText.IsEnabled = false;
            scheduledText.IsEnabled = false;
            pickedUpText.IsEnabled = false;
            weightText.IsEnabled = false;
            deliveredText.IsEnabled = false;
            PriorityText.IsEnabled = false;
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {

        }

        private void ViewCustomerClick(object sender, RoutedEventArgs e)
        {
            BO.Customer customer = bl.GetCustomer(selectedItem.CustomerInParcelSender.Id);
            new Customer(bl, customer).Show();
        }

        private void ViewDroneClick(object sender, RoutedEventArgs e)
        {
            BO.Drone drone = bl.GetDrone(selectedItem.DroneInParcel.Id);
            DroneToList droneToList = new()
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = drone.MaxWeight,
                Status = drone.Status,
                Battery = drone.Battery,
                ParcelInTransfer = drone.ParcelInTransfer,
                CurrentLocation = drone.CurrentLocation,
                ParcelTransferredNumber = drone.ParcelInTransfer.Id
            };
            //new Drone()
        }
    }
}
