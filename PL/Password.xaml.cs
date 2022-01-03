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
    /// Interaction logic for Password.xaml
    /// </summary>
    public partial class Password : Window
    {
        public Password()
        {
            InitializeComponent();
            PasswordManager.Visibility = Visibility.Visible;

        }
        private static int temp = 5;
        //oriya+AA1234
        //noa+AA5678
        //ori+AA8989
        private void LoginClick(object sender, RoutedEventArgs e)
        {

            if ( (Username.Text == "oriya"&& PasswordM.Password == "AA1234") || (Username.Text == "noa" && PasswordM.Password == "AA5678" )|| (Username.Text == "ori" && PasswordM.Password == "AA8989"))
            {
                MessageBox.Show("succeeded ");
                new ShowLists().Show();

            }
            else
            {
                temp--;
                MessageBox.Show("Incorrect password or username, you have " + temp + " more attempts");
                if (temp == 0)
                {
                    MessageBox.Show("You did not make all the attempts");
                    this.Close();
                }
            }

     }

        private void ClosedClick(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
    }
    }

