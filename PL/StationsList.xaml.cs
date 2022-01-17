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
using System.Collections.ObjectModel;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationsList.xaml
    /// </summary>
    public partial class StationsList : Window
    {
        private IBL bl;
        private ObservableCollection<StationToList> stations = new();
        private CollectionView view;
        private CollectionView CollectionView;
        private bool temp = false;
        public StationsList(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            foreach (var item in bl.GetStationList())
            {
                stations.Add(item);
            }
            stationsList.DataContext = stations;
        }
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

        private void GetActions(object sender, MouseButtonEventArgs e)
        {
            BO.Station station=bl.GetStation(((StationToList)stationsList.SelectedItem).Id);
            Station win = new Station(bl, station);
            win.RefreshEvent += Refresh;
            win.Show();
        }

        private void ShowAddStationWindow(object sender, RoutedEventArgs e)
        {
            Station win = new Station(bl);
            win.RefreshEvent += Refresh;
            Grouping.IsEnabled = true;
            GroupingNotAvailableStations.IsEnabled = true;
            win.Show();

        }
        private void Refresh(object sender, EventArgs e)//פןנקצית רענון
        {
            stationsList.ItemsSource = bl.GetStationList();
        }

        private void GroupingClick(object sender, RoutedEventArgs e)
        {
            view = (CollectionView)CollectionViewSource.GetDefaultView(stationsList.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("AvailableStations");
            view.GroupDescriptions.Add(groupDescription);
            temp = true;
            if (view.GroupDescriptions.Count >= 1 )
            {
                Grouping.IsEnabled = false;
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

        private void GroupinNotAvailableStationsgClick(object sender, RoutedEventArgs e)
        {
            CollectionView = (CollectionView)CollectionViewSource.GetDefaultView(stationsList.ItemsSource);
            PropertyGroupDescription groupDescriptionName = new PropertyGroupDescription("NotAvailableStations");
            CollectionView.GroupDescriptions.Add(groupDescriptionName);
            if (CollectionView.GroupDescriptions.Count >= 1)
            {
                GroupingNotAvailableStations.IsEnabled = false;
            }

        }
    }
}


