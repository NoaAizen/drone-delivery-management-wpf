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
        private IBL.IBL bl;//שדה בשביל גישה לBL
       /// <summary>
       /// בנאי בשביל חלון של הרשימה
       /// </summary>
       /// <param name="b">רחפן של BL</param>
        public DroneLists(IBL.IBL b)//
        {
            InitializeComponent();
            bl = b;
            DroneListsView.ItemsSource = bl.GetDroneList();//
            StatusSelector.ItemsSource = Enum.GetValues(typeof(StatusDrone));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            InitializeComponent();


        }
        /// <summary>
       /// פונקציית פקד של סינון לפני סטטוס
        /// </summary>
        /// <param name="sender">חלן</param>
        /// <param name="e">אירוע</param>
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelector.SelectedItem == null)
            {
                DroneListsView.ItemsSource = bl.GetDroneList();
            }
            else
            {
                StatusDrone status = (StatusDrone)StatusSelector.SelectedItem;
                DroneListsView.ItemsSource = bl.GetPartOfDroneList(x =>x.Status == status);
                
            }
        }
        /// <summary>
        /// פונקציית פקד של לפני סינון משקל
        /// </summary>
        /// <param name="sender">חלון</param>
        /// <param name="e">אירוע</param>
        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)//
        {
            if (WeightSelector.SelectedItem == null)
            {
                DroneListsView.ItemsSource = bl.GetDroneList();
            }
            else
            {
                WeightCategories weight = (WeightCategories)WeightSelector.SelectedItem;
                DroneListsView.ItemsSource = bl.GetPartOfDroneList(x => x.MaxWeight == weight);

            }
        }
        /// <summary>
        /// פונקציית כפתור בשביל הוספת רחפן
        /// </summary>
        /// <param name="sender">חלון</param>
        /// <param name="e">אירוע</param>
        private void ShowAddDroneWindow(object sender, RoutedEventArgs e)//
        {
            Drone ADD= new Drone(bl);
            ADD.RefreshEvent += Refresh;
            ADD.Show();
        }
        /// <summary>
        /// פונקציית כפתור בשביל הפעולות
        /// </summary>
        /// <param name="sender">חלון</param>
        /// <param name="e">אירוע</param>
        private void GetActions(object sender, MouseButtonEventArgs e)//
        {
     
            Drone win= new Drone(bl,(DroneToList)DroneListsView.SelectedItem);
            win.RefreshEvent += Refresh;
            win.Show();
        }
        /// <summary>
        /// פונקצית רענון רשימה
        /// </summary>
        /// <param name="sender">חלון</param>
        /// <param name="e">אירוע</param>
        private void Refresh(object sender, EventArgs e)//פןנקצית רענון
        {
            DroneListsView.ItemsSource = bl.GetDroneList();
            if (StatusSelector.SelectedItem == null)
            {
                DroneListsView.ItemsSource = bl.GetDroneList();
            }

            else
            {
                StatusDrone status = (StatusDrone)StatusSelector.SelectedItem;
                DroneListsView.ItemsSource = bl.GetPartOfDroneList(x => x.Status == status);

            }
            if (WeightSelector.SelectedItem == null)
            {
                DroneListsView.ItemsSource = bl.GetDroneList();
            }
            else
            {
                WeightCategories weight = (WeightCategories)WeightSelector.SelectedItem;
                DroneListsView.ItemsSource = bl.GetPartOfDroneList(x => x.MaxWeight == weight);

            }
        }
        /// <summary>
        /// סגירת חלון של רשימה
        /// </summary>
        /// <param name="sender">חולן</param>
        /// <param name="e">שדה</param>
        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}


