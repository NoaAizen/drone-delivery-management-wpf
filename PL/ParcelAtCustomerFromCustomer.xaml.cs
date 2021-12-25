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
    /// Interaction logic for ParcelAtCustomerFromCustomer.xaml
    /// </summary>
    public partial class ParcelAtCustomerFromCustomer : Window
    {
        private IBL bl;
         private BO.Customer temp;
        int idtemp;
        public ParcelAtCustomerFromCustomer(IBL bl, CustomerPo Customerpo)
        {
            InitializeComponent();
            this.bl = bl;
            maxWeightText.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            PriorityText.ItemsSource = Enum.GetValues(typeof(Priorities));
            StatusParcelText.ItemsSource = Enum.GetValues(typeof(StatusParcel));
            ParcelAtCustomerFromCustomers.DataContext = Customerpo;
            StatusParcelText.DataContext = Customerpo.ParcelAtCustomerFromCustomer;
            maxWeightText.DataContext = Customerpo.ParcelAtCustomerFromCustomer;
            PriorityText.DataContext = Customerpo.ParcelAtCustomerFromCustomer;

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

