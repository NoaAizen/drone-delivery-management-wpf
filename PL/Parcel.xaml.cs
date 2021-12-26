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

namespace PL
{
    /// <summary>
    /// Interaction logic for Parcel.xaml
    /// </summary>
    public partial class Parcel : Window
    {
        private IBL bl;
        private ParcelToList selectedItem;
        public event EventHandler RefreshEvent; //שדה בשביל הרענון
        //private ParcelPo parcelPo;//שדה בשביל המרת מידע

        public Parcel(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            AddParcelGrid.IsEnabled = true;
            AddParcelGrid.Visibility = Visibility.Visible;
            priority.ItemsSource = Enum.GetValues(typeof(Priorities));
            weight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        public Parcel(IBL bl, ParcelToList selectedItem)
        {
            InitializeComponent();
            this.bl = bl;
            this.selectedItem = selectedItem;
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddNewParcelClick(object sender, RoutedEventArgs e)
        {
            BO.Parcel parcel = new()
            {
                CustomerInParcelSender = new() { Id= int.Parse(senderId.Text) },
                CustomerInParcelRecipient = new() { Id= int.Parse(recipientId.Text) },
                Weight = (WeightCategories)weight.SelectedItem,
                Priority = (Priorities)priority.SelectedItem
            };
            try
            {
                int id= bl.AddParcel(parcel);
                RefreshEvent(this, EventArgs.Empty);
                MessageBox.Show("sucssesed\nparcel's id: "+id);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
