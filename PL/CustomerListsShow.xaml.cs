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
using System.Collections.ObjectModel;
using System.Globalization;
namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerListsShow.xaml
    /// </summary>
    public partial class CustomerListsShow : Window
    {
        private IBL bl;
        private ObservableCollection<BO.CustomerToList> customers = new();
        private CollectionView view;

        public CustomerListsShow(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            foreach (var item in bl.GetCustomerList())
            {
                customers.Add(item);
            }
            customerlist.DataContext = customers;
        }

        private void GetActionsCustomer(object sender, MouseButtonEventArgs e)
        {
            BO.Customer customer = bl.GetCustomer(((BO.CustomerToList)customerlist.SelectedItem).Id);
            Customer win = new(bl, customer);
            win.RefreshEvent += Refresh;
            win.Show();
        }
        private void Refresh(object sender, EventArgs e)//פןנקצית רענון
        {
            customerlist.ItemsSource = bl.GetCustomerList();

        }

        private void AddCustomerClick(object sender, RoutedEventArgs e)
        {
            Customer ADD = new Customer(bl);
            ADD.RefreshEvent += Refresh;
            ADD.Show();
        }

      
        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void group(object sender, RoutedEventArgs e)
        {
            view = (CollectionView)CollectionViewSource.GetDefaultView(customerlist.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("NumberOfParcelReceived");
            view.GroupDescriptions.Add(groupDescription);
        }
    }

    public class convertToInt : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 1; //int.Parse(value);
        }
    }
}