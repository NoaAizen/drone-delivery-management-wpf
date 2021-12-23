using BlApi;
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
        private ObservableCollection<BO.StationToList> stations = new () ;
       
            public StationsList(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            foreach(var item in bl.GetStationList())
            {
                stations.Add(item);
            }
            list.DataContext = stations;
        }

    }
}
