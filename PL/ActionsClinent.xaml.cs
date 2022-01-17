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
    /// Interaction logic for ActionsClinent.xaml
    /// </summary>
    public partial class ActionsClinent : Window
    {
        public event EventHandler RefreshEvent; //שדה בשביל הרענון
        private IBL bl;//ממשק bl
        private BO.Parcel parcel;
        private ParcelPo parcelPo;//שדה בשביל המרת מידע
        private ParcelAtCustomer selectedItem;
        private BO.Drone drone;
        private DronePO dronePO=new() ;//שדה בשביל המרת מידע 
        /// <summary>
        /// פתיחת של חלון עם הרישמות בשביל לעדכן
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="selectedItem"></param>
        public ActionsClinent(IBL bl, ParcelAtCustomer selectedItem)
        {
            this.bl = bl;
            this.selectedItem = selectedItem;
            InitializeComponent();
            this.bl = bl;
            parcel = bl.GetParcel(selectedItem.Id);
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
            if (parcel.DroneInParcel==null)
            {
                Collection.IsEnabled = false;
                Delivery.IsEnabled = false;

            }
            else
            {
                drone = bl.GetDrone(parcel.DroneInParcel.Id);
            }
        }
        /// <summary>
        /// פונקצית איסוף
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectionClick(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.CollectionParcelFromDrone(drone.Id);
                convertToPo(parcelPo, bl.GetParcel(parcelPo.Id));
                MessageBox.Show("sucssesed");
                DroneLists win = new DroneLists(bl);
                win.RefreshEvent += Refresh;

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
                convertToPo(parcelPo, bl.GetParcel(selectedItem.Id));
                MessageBox.Show("sucssesed");
                DroneLists win = new DroneLists(bl);
                win.RefreshEvent += Refresh;
    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        /// <summary>
        /// פונקצית רענון
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refresh(object sender, EventArgs e)
        {
            DroneLists DroneLists = new DroneLists(bl);


        }
        /// <summary>
        /// /פונקציה שעושה המרה בשביל הזרימת מידע
        /// </summary>
        /// <param name="dronePo">רחפן של PL</param>
        /// <param name="d">רחפן של BO</param>

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
        /// <summary>
        /// פונקציה להפעלת שדות להיות לא זמינים 
        /// </summary>
        public void notEnablFildes()//
        {
            idText.IsEnabled = false;
            requestedText.IsEnabled = false;
            scheduledText.IsEnabled = false;
            pickedUpText.IsEnabled = false;
            deliveredText.IsEnabled = false;
            weightText.IsEnabled = false;
            PriorityText.IsEnabled = false;
        }
        /// <summary>
        /// פונקציה לסגירת חלון
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
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
    }
}
