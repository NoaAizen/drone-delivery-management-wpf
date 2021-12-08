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
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for Drone.xaml
    /// </summary>
    public partial class Drone : Window
    {
        private IBL.IBL bl;
       private DroneToList selectedItem;

        public Drone(IBL.IBL bl)//הוספה
        {
            this.bl = bl;
            InitializeComponent();
            AddDroneGrid.IsEnabled = true;
            AddDroneGrid.Visibility = Visibility.Visible;

        }

        public Drone(IBL.IBL bl, DroneToList selectedItem)//פעולות
        {
            this.bl = bl;
            this.selectedItem = selectedItem;
            InitializeComponent();
            Actions.IsEnabled = true;
            Actions.Visibility =Visibility.Visible;
        }

        private void AddNewDroneClick(object sender, RoutedEventArgs e)
        {
            int stationNumber = int.Parse(stationId.Text);
            int temp= int.Parse(maxWeight.Text);
            DroneToList drone = new()
            {
                Id= int.Parse(id.Text),
                Model=model.Text,
                MaxWeight = (WeightCategories)temp
            };
            bl.AddDrone(drone,stationNumber);
        }

        private void main(object sender, RoutedEventArgs e)
        {
            new DroneLists(bl).Show();
        }
    }
}
