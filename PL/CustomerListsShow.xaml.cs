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
        private CollectionView view;//שדה בשביל הגרופינג
        private IBL bl;
        private ObservableCollection<BO.CustomerToList> customers = new();//רשימת לקחות

        public event EventHandler RefreshEvent; //שדה בשביל הרענון
        /// <summary>
        /// בנאי 
        /// </summary>
        /// <param name="bl"></param>
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
        /// פונקציה בשביל סגירת חלון פעולות
        /// </summary>
        /// <param name="sender">חלון</param>
        /// <param name="e">אירוע</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// פונקציה לפתית חלון פעולות
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetActionsCustomer(object sender, MouseButtonEventArgs e)
        {
            BO.Customer customer = bl.GetCustomer(((BO.CustomerToList)customerlist.SelectedItem).Id);
            Customer win = new(bl, customer);
            win.RefreshEvent += Refresh;
            win.Show();

        }
   
        /// <summary>
        /// פונקציה להוספת לקוח
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCustomerClick(object sender, RoutedEventArgs e)
        {
            Customer ADD = new Customer(bl);
            ADD.RefreshEvent += Refresh;
            ADD.Show();
        }

        /// <summary>
        /// פונקציה לסגירת חלון של הוספה
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
        /// <summary>
        /// פונקצית רענון
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refresh(object sender, EventArgs e)
        {
            customerlist.ItemsSource = bl.GetCustomerList();

        }
        /// <summary>
        /// פונקציה שעושה גרופניג
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberOfParcelReceivedClick(object sender, RoutedEventArgs e)
        {
            view = (CollectionView)CollectionViewSource.GetDefaultView(customerlist.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("NumberOfParcelReceived");
            view.GroupDescriptions.Add(groupDescription);
            if (view.GroupDescriptions.Count >=1)
            {
                NumberOfParcelReceivedGroing.IsEnabled = false;
            }

        }
   
       
    }

   
}