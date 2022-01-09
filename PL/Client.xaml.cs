using BlApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        private IBL bl;
        private BO.Customer customer=new();
        public event EventHandler RefreshEvent;

        public Client(IBL bl, string idText)
        {
            this.bl = bl;
            InitializeComponent();
            customer= bl.GetCustomer(int.Parse(idText));
            ParcelAtCustomerFromCustomerID.ItemsSource = customer.ParcelAtCustomerFromCustomer;
            ParcelAtCustomerToCustomerID.ItemsSource = customer.ParcelAtCustomerToCustomer;
        }


        private void AddParcelClick(object sender, RoutedEventArgs e)
         {
            new Parcel(bl).Show();
        }

        private void ParcelAtCustomerFromCustomerID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BO.Parcel p = bl.GetParcel(customer.Id);
            Parcel win = new Parcel(bl, p);
            win.RefreshEvent += Refresh;
            win.Show();
        }
        private void Refresh(object sender, EventArgs e)//פןנקצית רענון
        {
            ParcelAtCustomerFromCustomerID.ItemsSource = customer.ParcelAtCustomerFromCustomer;
            ParcelAtCustomerToCustomerID.ItemsSource = customer.ParcelAtCustomerToCustomer;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)//להחליט מה לשים פה איזה קישור!
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.youtube.com/watch?v=ZZFKyCJmWZI&list=RDHdB1F-u0d4Y&index=10",
                UseShellExecute = true
            }); 

        }

        //מה זה אומרת-תתאפשר אישור איסוף ואישור קבלת חבילה
    }
}
