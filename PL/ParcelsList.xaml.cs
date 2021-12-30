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
        private CollectionView view;
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
            StatusSelector.ItemsSource = Enum.GetValues(typeof(StatusParcel));
        }

        private void ShowAddParcelWindow(object sender, RoutedEventArgs e)
        {
            Parcel win = new Parcel(bl);
            win.RefreshEvent += Refresh;
            Grouping.IsEnabled = true;
            win.Show();
        }

        private void GetActions(object sender, MouseButtonEventArgs e)
        {
            BO.Parcel parcel = bl.GetParcel(((ParcelToList)parcelsList.SelectedItem).Id);
            Parcel win = new Parcel(bl, parcel);
            win.RefreshEvent += Refresh;
            win.Show();
        }
        private void Refresh(object sender, EventArgs e)//פןנקצית רענון
        {
            parcelsList.ItemsSource = bl.GetParcelList();
        }

        private void StatusSelectorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelector.SelectedItem == null)
            {
                parcelsList.ItemsSource = bl.GetParcelList();
            }
            else
            {
                StatusParcel status = (StatusParcel)StatusSelector.SelectedItem;
                parcelsList.ItemsSource = bl.GetParcelList(x => x.StatusParcel == status);
            }

        }
        private void GroupingClick(object sender, RoutedEventArgs e)
        {
            view = (CollectionView)CollectionViewSource.GetDefaultView(parcelsList.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("SenderName");
            view.GroupDescriptions.Add(groupDescription);
            if (view.GroupDescriptions.Count == 1)
            {
                Grouping.IsEnabled = false;
            }

        }
    }
}
