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
using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for ShowLists.xaml
    /// </summary>
    public partial class ShowLists : Window
    {
        private IBL bl;
        /// <summary>
        /// סגירת חלון
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
        //בנאי
        public ShowLists(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
        }

        /// <summary>
        /// פונקציה בשביל לבגיע לכפתור של רחפנים
        /// </summary>
        /// <param name="sender">חלון</param>
        /// <param name="e">אירוע</param>
        private void ShowDronesButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneLists(bl).Show();
        }
        /// <summary>
        /// מעבר לחלון של תחנות
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowStationsListClick(object sender, RoutedEventArgs e)
        {
            new StationsList(bl).Show();
        }
        /// <summary>
        /// מעבר לחלון של לקוחות
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomerListClick(object sender, RoutedEventArgs e)
        {
            new CustomerListsShow(bl).Show();
        }
        /// <summary>
        /// מעבר לחלון של חבילות
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowParcelsListClick(object sender, RoutedEventArgs e)
        {
            new ParcelsList(bl).Show();
        }
    }
}
