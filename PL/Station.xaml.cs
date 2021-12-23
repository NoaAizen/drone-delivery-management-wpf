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
    /// Interaction logic for Station.xaml
    /// </summary>
    public partial class Station : Window
    {//oriya
        private IBL bl;
        private StationToList selectedItem;

        public Station(IBL bl, StationToList selectedItem)
        {
            InitializeComponent();
            this.bl = bl;
            this.selectedItem = selectedItem;
        }
    }
}
