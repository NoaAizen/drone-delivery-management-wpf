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
    /// Interaction logic for Customer.xaml
    /// </summary>
    public partial class Customer : Window
    {
        public event EventHandler RefreshEvent;
        private IBL bl;
        private BO.Customer selectedItem;
        BO.Customer c = new();
        BO.Location L = new();
        private CustomerPo Customerpo;
        ///// <summary>
        /////פתיחה חלון של עדכון (על ידי רשימות).
        ///// </summary>
        ///// <param name="bl"></param>
        ///// <param name="selectedItem"></param>
        //public Customer(IBL bl, CustomerToList selectedItem)
        //{
        //    InitializeComponent();

        //    this.bl = bl;
        //    this.selectedItem = selectedItem;
        //    c = bl.GetCustomer(selectedItem.Id);
        //    Actions.IsEnabled = true;
        //    Actions.Visibility = Visibility.Visible;

        //    Customerpo = new()
        //    {
        //        Id = c.Id,
        //        Name = c.Name,
        //        Phone = c.Phone,
        //        Latitude = c.Location.Latitude,
        //        Longitude = c.Location.Longitude,
        //        ParcelAtCustomerFromCustomer = c.ParcelAtCustomerFromCustomer,
        //        ParcelAtCustomerToCustomer = c.ParcelAtCustomerToCustomer,
        //    };
            
        //    Actions.DataContext = Customerpo;
        //    notEnablFildes();
        //    NameText.IsEnabled = true;//עדכון של זמינות המודל
        //    PhoneText.IsEnabled = true;
        //}
        /// <summary>
        ///פתיחה חלון של עדכון (על ידי רשימות).
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="selectedItem"></param>
        public Customer(IBL bl, BO.Customer c)
        {
            InitializeComponent();

            this.bl = bl;
            this.selectedItem = c;
            //c = bl.GetCustomer(selectedItem.Id);
            Actions.IsEnabled = true;
            Actions.Visibility = Visibility.Visible;

            Customerpo = new()
            {
                Id = c.Id,
                Name = c.Name,
                Phone = c.Phone,
                Latitude = c.Location.Latitude,
                Longitude = c.Location.Longitude,
                ParcelAtCustomerFromCustomer = c.ParcelAtCustomerFromCustomer,
                ParcelAtCustomerToCustomer = c.ParcelAtCustomerToCustomer,
            };

            Actions.DataContext = Customerpo;
            notEnablFildes();
            NameText.IsEnabled = true;//עדכון של זמינות המודל
            PhoneText.IsEnabled = true;
        }
        /// <summary>
        /// פונקציה של פתחית חלון הוספה
        /// </summary>
        /// <param name="bl"></param>
        public Customer(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            AddCustomerGrid.Visibility = Visibility.Visible;
            AddNewCustomer.IsEnabled = false;
        }

        public Customer()
        {
        }


        /// <summary>
        /// הוספת נתונים ללקוח (הוספה נתונים)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewCustomerClick(object sender, RoutedEventArgs e)
        {
            int tempid = int.Parse(idteaxt.Text);
            L.Latitude = double.Parse(longitudetext.Text);
            L.Longitude = double.Parse(latitudeteaxt.Text);
            c = new BO.Customer
            {
                Id = tempid,
                Name = nameteaxt.Text,
                Phone = phoneeteaxt.Text,
                Location = L,
            };
            try
            {
                bl.AddCustomer(c);
                MessageBox.Show("sucssesed");
                this.Close();
                RefreshEvent(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// לא לקבל נתונים בהוספת לקוח
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void IdClick(object sender, TextChangedEventArgs e)
        {
            if (idteaxt.Text != "" && phoneeteaxt.Text != "" && nameteaxt.Text != "" && longitudetext.Text != "" && latitudeteaxt.Text != "")
                AddNewCustomer.IsEnabled = true;
            else
                AddNewCustomer.IsEnabled = false;
        }

        private void NameClick(object sender, TextChangedEventArgs e)
        {
            if (idteaxt.Text != "" && phoneeteaxt.Text != "" && nameteaxt.Text != "" && longitudetext.Text != "" && latitudeteaxt.Text != "")
                AddNewCustomer.IsEnabled = true;
            else
                AddNewCustomer.IsEnabled = false;
        }

        private void LatitudetClick(object sender, TextChangedEventArgs e)
        {

            if (idteaxt.Text != "" && phoneeteaxt.Text != "" && nameteaxt.Text != "" && longitudetext.Text != "" && latitudeteaxt.Text != "")
                AddNewCustomer.IsEnabled = true;
            else
                AddNewCustomer.IsEnabled = false;
        }

        private void PhoneClick(object sender, TextChangedEventArgs e)
        {

            if (idteaxt.Text != "" && phoneeteaxt.Text != "" && nameteaxt.Text != "" && longitudetext.Text != "" && latitudeteaxt.Text != "")
                AddNewCustomer.IsEnabled = true;
            else
                AddNewCustomer.IsEnabled = false;
        }

        private void LongitudClick(object sender, TextChangedEventArgs e)
        {
            if (idteaxt.Text != "" && phoneeteaxt.Text != "" && nameteaxt.Text != "" && longitudetext.Text != "" && latitudeteaxt.Text != "")
                AddNewCustomer.IsEnabled = true;
            else
                AddNewCustomer.IsEnabled = false;
        }
        /// <summary>
        /// כפתור עדכון
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdatClick(object sender, RoutedEventArgs e)
        {
            NameText.IsEnabled = true;//עדכון של זמינות המודל
            idText.IsEnabled = false;
            PhoneText.IsEnabled = true;
            Customerpo.Id = int.Parse(idText.Text);
            Customerpo.Name = NameText.Text;
            Customerpo.Phone = PhoneText.Text;

            try
            {
                bl.UpdateCustomer(Customerpo.Id, Customerpo.Name, Customerpo.Phone);//האם מותר  לשנות ID
                                                                                                     //    convertToPo(drone, bl.GetDrone(drone.Id));
               MessageBox.Show("sucssesed");
              RefreshEvent(this, EventArgs.Empty);//יש בבעיה אחרי כניסה מחבילה

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void notEnablFildes()//
        {
            idText.IsEnabled = false;
            longitudeText.IsEnabled = false;
            latitudeText.IsEnabled = false;
        }

        private void ParcelAtCustomerFromCustomerClick(object sender, MouseButtonEventArgs e)
        {
            BO.Parcel p = bl.GetParcel(selectedItem.Id);
            Parcel win = new Parcel(bl,p);
            win.RefreshEvent += Refresh;
            win.Show();
        }

        private void Refresh(object sender, EventArgs e)//פןנקצית רענון
        {
            CustomerListsShow customerListsShow = new CustomerListsShow(bl);
        }
        private void ParcelAtCustomerToCustomerClick(object sender, MouseButtonEventArgs e)
        {
            BO.Parcel p = bl.GetParcel(selectedItem.Id);
            Parcel win = new Parcel(bl, p);
            win.RefreshEvent += Refresh;
            win.Show();
        }
    }
}
