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
using BO;
using System.Collections.ObjectModel;

namespace PL
{
    /// <summary>
    /// Interaction logic for Password.xaml
    /// </summary>
    public partial class Password : Window
    {
        private ObservableCollection <BO.UserToLIst> Users = new();
        private bool flag = false;
        private static int temp = 5;
        private IBL bl;
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
        public Password(string Name, IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            if (Name == "Manager")
            {
                PasswordManager.Visibility = Visibility.Visible;
            }
            else if(Name == "Client")
            {
                PasswordClient.Visibility = Visibility.Visible;
            }
            else if (Name == "Forgot")
            {
                ForgotPassword.Visibility = Visibility.Visible;
            }
        }

  


        //oriya+AA1234
        //noa+AA5678
        //ori+AA8989
        private void LoginClickManager(object sender, RoutedEventArgs e)
        {
            PasswordManager.Visibility = Visibility.Visible;
            if ( (Username.Text == "oriya"&& PasswordM.Password == "AA1234") || (Username.Text == "noa" && PasswordM.Password == "AA5678" )|| (Username.Text == "ori" && PasswordM.Password == "AA8989"))
            {
                MessageBox.Show("succeeded ");
                new ShowLists(bl).Show();

            }
            else
            {
                temp--;
                MessageBox.Show("Incorrect password or username, you have " + temp + " more attempts");
                if (temp == 0)
                {
                    temp = 0;
                    MessageBox.Show("You did not make all the attempts");
                    this.Close();
                }
            }

     }

        private void ClosedClick(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();

        }

        private void LoginClicentClick(object sender, RoutedEventArgs e)
        {
            PasswordClient.Visibility = Visibility.Visible;
            foreach (var item in bl.GetUSList())
            {
                if (item.Name == UsernameClient.Text && item.Id == int.Parse(IdText.Text) && item.Password == PasswordC.Password)
                {
                    flag = true;
                    break;
                }
            }

            if(flag==true)
            {
                MessageBox.Show("succeeded ");
                new Client(bl, IdText.Text).Show();

            }
            else
            {
                temp--;
                MessageBox.Show("Incorrect id or password or username, you have " + temp + " more attempts");
                if (temp == 0)
                {
                    temp = 0;
                    MessageBox.Show("You did not make all the attempts");
                    this.Close();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new Password("Forgot",bl).Show();
        }

        private void ForgotPasswordClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ForgotPassword.Visibility = Visibility.Visible;
                bl.ChangePassword(PasswordF.Password, int.Parse(IdF.Text));
                MessageBox.Show("succeeded ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
}
    }
    }

