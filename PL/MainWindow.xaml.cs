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

        private void ManagerClick(object sender, RoutedEventArgs e)
        {
           string temp = "Manager";
            new Password(temp, bl).Show();
            Close();

        }


        private void ClientClick(object sender, RoutedEventArgs e)
        {
             string temp = "Client";
             new Password(temp,bl).Show();
        }

        private void NewClicentClick(object sender, RoutedEventArgs e)
        {
            //new NewClicent(bl).Show();

        }


    }
}