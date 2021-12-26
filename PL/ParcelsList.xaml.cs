using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ParcelsList.xaml
    /// </summary>
    public partial class ParcelsList : Window
    {
        private IBL bl;
        private ObservableCollection<ParcelToList> parcels = new();

        public ParcelsList(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            foreach (var item in bl.GetParcelList())
            {
                parcels.Add(item);
            }
            parcelsList.DataContext = parcels;
        }

        private void ShowAddParcelWindow(object sender, RoutedEventArgs e)
        {
            Parcel win = new Parcel(bl);
            win.RefreshEvent += Refresh;
            win.Show();
        }

        private void GetActions(object sender, MouseButtonEventArgs e)
        {
            Parcel win = new Parcel(bl, (ParcelToList)parcelsList.SelectedItem);
            win.RefreshEvent += Refresh;
            win.Show();
        }
        private void Refresh(object sender, EventArgs e)//פןנקצית רענון
        {
            parcelsList.ItemsSource = bl.GetParcelList();
        }
    }
}
