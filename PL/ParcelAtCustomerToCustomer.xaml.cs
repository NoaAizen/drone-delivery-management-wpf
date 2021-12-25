using BlApi;
using System;
using BO;
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
    /// Interaction logic for ParcelAtCustomerToCustomer.xaml
    /// </summary>
    public partial class ParcelAtCustomerToCustomer : Window
    {
        private IBL bl;

       

        public ParcelAtCustomerToCustomer(IBL bl, CustomerPo customerpo)
        {
            InitializeComponent();
            this.bl = bl;
            PriorityText.ItemsSource = Enum.GetValues(typeof(Priorities));
            StatusParcelText.ItemsSource = Enum.GetValues(typeof(StatusParcel));
            maxWeightText.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            ParcelAtCustomerTOCustomers.DataContext = customerpo;
            StatusParcelText.DataContext = customerpo.ParcelAtCustomerToCustomer;
            maxWeightText.DataContext = customerpo.ParcelAtCustomerToCustomer;
            PriorityText.DataContext = customerpo.ParcelAtCustomerToCustomer;
            close.IsEnabled = true;
            StatusParcelText.IsEnabled = false;
            maxWeightText.IsEnabled = false;
            idText.IsEnabled = false;
            PriorityText.IsEnabled = false;
            preclIdText.IsEnabled = false;
            preclnameText.IsEnabled = false;
            //preclIdText.DataContext =bl.GetCustomer(Customerpo c)
            //    ;//לראות איך עושים את זה!


        }

        private void CcloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
