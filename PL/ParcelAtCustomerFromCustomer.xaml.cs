using BlApi;
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
    /// Interaction logic for ParcelAtCustomerFromCustomer.xaml
    /// </summary>
    public partial class ParcelAtCustomerFromCustomer : Window
    {
        private IBL bl;

        public ParcelAtCustomerFromCustomer()
        {
            InitializeComponent();
        }

        public ParcelAtCustomerFromCustomer(IBL bl,CustomerPo customerpo)
        {
            this.bl = bl;
            InitializeComponent();

            idText.DataContext = drone.ParcelInTransfer;
            senderIdText.DataContext = drone.ParcelInTransfer.CustomerInParcelSender;
            recipientIdText.DataContext = drone.ParcelInTransfer.CustomerInParcelRecipient;

            senderNameText.DataContext = drone.ParcelInTransfer.CustomerInParcelSender;
            recipientNameText.DataContext = drone.ParcelInTransfer.CustomerInParcelRecipient;

            TransportDistanceText.DataContext = drone.ParcelInTransfer;
            senderLongitudeText.DataContext = drone.ParcelInTransfer.CollectionLocation;
            recipientLongitudeText.DataContext = drone.ParcelInTransfer.DeliveryDestinationLocation;
            senderLatitudeText.DataContext = drone.ParcelInTransfer.CollectionLocation;
            recipientLatitudeText.DataContext = drone.ParcelInTransfer.DeliveryDestinationLocation;
            PriorityText.ItemsSource = Enum.GetValues(typeof(Priorities));
            maxWeightText.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            ParcelStatusText.IsChecked = drone.ParcelInTransfer.ParcelStatus;
            maxWeightText.DataContext = drone.ParcelInTransfer;
            PriorityText.DataContext = drone.ParcelInTransfer;
        }
    }
}
