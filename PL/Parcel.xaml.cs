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
            AddNewParcel.IsEnabled = false;
        }
        public Parcel(IBL bl, BO.Parcel parcel)
        {
            InitializeComponent();
            this.bl = bl;
            this.selectedItem = parcel;
            Actions.IsEnabled = true;
            Actions.Visibility = Visibility.Visible;
            notEnablFildes();
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
            if (parcel.Scheduled == null || parcel.Delivered != null)
                viewDrone.IsEnabled = false;
            if (parcel.Scheduled != null)
                Delete.IsEnabled = false;
        }
        private void moveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
       
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
            bl.DeleteParcel(selectedItem);
            convertToPo(parcelPo, selectedItem);
            RefreshEvent(this, EventArgs.Empty);
            MessageBox.Show("sucssesed");
            this.Close();
        }

        private void ViewSenderClick(object sender, RoutedEventArgs e)
        {

            BO.Customer customer = bl.GetCustomer(selectedItem.CustomerInParcelSender.Id);
            Customer win = new Customer(bl, customer);
            win.RefreshEvent += Refresh;
            win.Show();
        }

        private void Refresh(object sender, EventArgs e)
        {
            CustomerListsShow customerListsShow = new CustomerListsShow(bl);
        }

        private void ViewRecipientClick(object sender, RoutedEventArgs e)
        {
            BO.Customer customer = bl.GetCustomer(selectedItem.CustomerInParcelRecipient.Id);
            Customer win = new Customer(bl, customer);
            win.RefreshEvent += Refresh;
            win.Show();
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
            new Drone(bl, droneToList).Show();//
        }

        /// <summary>
        /// פונקציה של נעילה אירוע תעודת זהות בשביל הוספת נתונים
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SenderIdClick(object sender, TextChangedEventArgs e)
        {
            if (senderId.Text != "" && recipientId.Text != "" && weight.SelectedItem != null && priority.SelectedItem != "")
                AddNewParcel.IsEnabled = true;
            else
                AddNewParcel.IsEnabled = false;
        }
        /// <summary>
        /// פונקציה של נעילה אירוע תעודת זהות בשביל הוספת נתונים
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void recipientIdClick(object sender, TextChangedEventArgs e)
        {
            if (senderId.Text != "" && recipientId.Text != "" && weight.SelectedItem != null && priority.SelectedItem != "")
                AddNewParcel.IsEnabled = true;
            else
                AddNewParcel.IsEnabled = false;
        }

        /// <summary>
        /// פונקציה של נעילה אירוע משקל בשביל הוספת נתונים
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeightClick(object sender, SelectionChangedEventArgs e)
        {
            if (senderId.Text != "" && recipientId.Text != "" && weight.SelectedItem != null && priority.SelectedItem != "")
                AddNewParcel.IsEnabled = true;
            else
                AddNewParcel.IsEnabled = false;
        }
        /// <summary>
        /// פונקציה של נעילה אירוע עדיפות בשביל הוספת נתונים
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PriorityClick(object sender, SelectionChangedEventArgs e)
        {
            if (senderId.Text != "" && recipientId.Text != "" && weight.SelectedItem != null && priority.SelectedItem != "")
                AddNewParcel.IsEnabled = true;
            else
                AddNewParcel.IsEnabled = false;
        }
        public void convertToPo(ParcelPo parcelPo, BO.Parcel parcel)
        {
            parcelPo.CustomerInParcelRecipient = parcel.CustomerInParcelRecipient;
            parcelPo.CustomerInParcelSender = parcel.CustomerInParcelSender;
            parcelPo.Delivered = parcel.Delivered;
            parcelPo.DroneInParcel = parcel.DroneInParcel;
            parcelPo.Id = parcel.Id;
            parcelPo.PickedUp = parcel.PickedUp;
            parcelPo.Priority = parcel.Priority;
            parcelPo.Requested = parcel.Requested;
            parcelPo.Scheduled = parcel.Scheduled;
            parcelPo.Weight = parcel.Weight;
        }
    }
}
