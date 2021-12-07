using BL;
using IBL.BO;
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
    /// Interaction logic for DroneLists.xaml
    /// </summary>
    public partial class DroneLists : Window
    {
        private IBL.IBL bl;

        public DroneLists(IBL.IBL b)//בנאי שמקל פרמטר ,עשינו כך בשביל שלא יהיה קריאה נוספת לבנאי של BL
        {
            InitializeComponent();
            bl = b;
            DroneListsView.ItemsSource = bl.GetDroneList();//
            StatusSelector.ItemsSource = Enum.GetValues(typeof(StatusDrone));
            InitializeComponent();

        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelector.SelectedItem == null)
            {
                DroneListsView.ItemsSource = bl.GetDroneList();
            }
            else
            {
                StatusDrone status = (StatusDrone)StatusSelector.SelectedItem;
                DroneListsView.ItemsSource = bl.GetDroneList(Drone =>Drone.status == status);

            }
        }
    }
}


