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

        private void GetActions(object sender, MouseButtonEventArgs e)
        {
            Station win = new Station(bl, (StationToList)stationsList.SelectedItem);
            win.RefreshEvent += Refresh;
            win.Show();
        }

        private void ShowAddStationWindow(object sender, RoutedEventArgs e)
        {
            Station win = new Station(bl);
            win.RefreshEvent += Refresh;
            win.Show();
        }
        private void Refresh(object sender, EventArgs e)//פןנקצית רענון
        {
            stationsList.ItemsSource = bl.GetStationList();
        }

        private void hhh(object sender, RoutedEventArgs e)
        {
            view = (CollectionView)CollectionViewSource.GetDefaultView(stationsList.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("NotAvailableStations");
            view.GroupDescriptions.Add(groupDescription);

        }
    }
}
