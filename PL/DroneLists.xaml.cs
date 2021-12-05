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
        public DroneLists()
        {
            InitializeComponent();
        }
        public DroneLists(IBL.IBL bl)//בנאי שמקל פרמטר ,עשינו כך בשביל שלא יהיה קריאה נוספת לבנאי של BL
        {

        }
    }
}
