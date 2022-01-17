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
        IBL bl = BlFactory.GetBl();//
        /// <summary>
        /// בנאי
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// כפתור מנהל
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManagerClick(object sender, RoutedEventArgs e)
        {
           string temp = "Manager";
            new Password(temp, bl).Show();
            Close();

        }
        /// <summary>
        /// כפתור סגירה
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
        /// <summary>
        /// כפתור לקוח
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientClick(object sender, RoutedEventArgs e)
        {
             string temp = "Client";
             new Password(temp,bl).Show();
            Close();

        }
        /// <summary>
        /// כפתור ללקוח חדש
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewClicentClick(object sender, RoutedEventArgs e)
        {
            new Password("NewClicent",bl).Show();
            Close();

        }


    }
}