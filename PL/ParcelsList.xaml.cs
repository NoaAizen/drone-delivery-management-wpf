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
        private CollectionView view;//בשביל גרופניג
        private IBL bl;//ממשק
        private ObservableCollection<ParcelToList> parcels = new();//רשימת חבילות
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
        /// פונקציה לסגירת החלון
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// בנאי של פתיחת החלון 
        /// </summary>
        /// <param name="bl"></param>
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
        /// <summary>
        /// פונקציה לכפתור של הוספת חלון
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAddParcelWindow(object sender, RoutedEventArgs e)
        {
            Parcel win = new Parcel(bl);
            win.RefreshEvent += Refresh;
            Grouping.IsEnabled = true;
            win.Show();
        }
       /// <summary>
       /// פונקציה לכפתור של פתיחת חלון פעולות
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void GetActions(object sender, MouseButtonEventArgs e)
        {
            BO.Parcel parcel = bl.GetParcel(((ParcelToList)parcelsList.SelectedItem).Id);
            Parcel win = new Parcel(bl, parcel);
            win.RefreshEvent += Refresh;
            win.Show();
        }
        /// <summary>
        /// פונקצית רענון
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refresh(object sender, EventArgs e)//פןנקצית רענון
        {
            parcelsList.ItemsSource = bl.GetParcelList();
        }
        /// <summary>
        /// פונקצית סינון לפני סטוטוס
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
       /// <summary>
       /// פונקצית של כפתור לפני גרופניג
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
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
