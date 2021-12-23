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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL bl = BlFactory.GetBl();//שדה בשביח להגיע לאחפנים שבBL
        /// <summary>
        /// בנאי
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// פונקציה בשביל לבגיע לכפתור של הרשימות
        /// </summary>
        /// <param name="sender">חלון</param>
        /// <param name="e">אירוע</param>
        private void ShowDronesButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneLists(bl).Show();
        }

        private void ShowStationsListClick(object sender, RoutedEventArgs e)
        {
            new StationsList(bl).Show();
        }

        private void CustomerListClick(object sender, RoutedEventArgs e)
        {
            new CustomerList(bl).Show();//noa
        }
    }
}