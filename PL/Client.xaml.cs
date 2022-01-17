using BlApi;
using BO;
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
        private BO.Customer customer=new();//שדה של לקוח
        public event EventHandler RefreshEvent;//אירוע בשביל לעשות רענון
        /// <summary>
        /// בנאי לפתיחת החלון של לקוח במערכת
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="idText"></param>
        public Client(IBL bl, string idText)
        {
            this.bl = bl;
            InitializeComponent();
            customer= bl.GetCustomer(int.Parse(idText));
            ParcelAtCustomerFromCustomerID.ItemsSource = customer.ParcelAtCustomerFromCustomer;
            ParcelAtCustomerToCustomerID.ItemsSource = customer.ParcelAtCustomerToCustomer;
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
        /// <summary>
        /// פונקצית להוספת חבילה-כפתור
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddParcelClick(object sender, RoutedEventArgs e)
         {
            new Parcel(bl, "Client").Show();
        }
        /// <summary>
        /// כפתור בשביל מעבר לפעולות של הרשימות
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParcelAtCustomerFromCustomerID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            new ActionsClinent(bl,(ParcelAtCustomer)ParcelAtCustomerFromCustomerID.SelectedItem).Show();
        }
        /// <summary>
        /// פונקציה בשביל רענון הנתונים
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refresh(object sender, EventArgs e)//פןנקצית רענון
        {
            CustomerListsShow customerListsShow = new CustomerListsShow(bl);
            ParcelAtCustomerFromCustomerID.ItemsSource = customer.ParcelAtCustomerFromCustomer;
            ParcelAtCustomerToCustomerID.ItemsSource = customer.ParcelAtCustomerToCustomer;
        }
        /// <summary>
        /// פונקציה בשביל כפתור ליצירת קשר
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)//
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://docs.google.com/forms/d/e/1FAIpQLSeI0z1QwnzIBkCv3Zel2og0Ie1AC7ULlO3jrR6cKI00a7CJLw/viewform?usp=sf_link",
                UseShellExecute = true
            }); 

        }
        /// <summary>
        /// פונקציה לסגירת החלון(לחיצה בעזרת כפתור)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// כפתור בשביל מעבר לפעולות של הרשימות
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParcelAtCustomerToCustomerID_SekectionChanged(object sender, SelectionChangedEventArgs e)
        {
            new ActionsClinent(bl, (ParcelAtCustomer)ParcelAtCustomerToCustomerID.SelectedItem).Show();

        }
        /// <summary>
        /// פונקציה לעדכון פרטים של הלקוח
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)//עדכון פרטים
        {
            string Client = "Client";
            Customer win = new Customer(bl, customer, Client);
            win.RefreshEvent += Refresh;
            win.Show();
        }

    }
}
