using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PL
{
    public class CustomerPo : INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Id"));
                }
            }
        }
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }
        private string phone;
        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Phone"));
                }
            }
        }
        private double longitude;
        public double Longitude
        {
            get { return longitude; }
            set
            {
                longitude = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Longitude"));
                }
            }
        }
        private double latitude;
        public double Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Latitude"));
                }
            }
        }
        private List<BO.ParcelAtCustomer> parcelAtCustomerFromCustomer;
        public List<BO.ParcelAtCustomer> ParcelAtCustomerFromCustomer
        {
            get { return parcelAtCustomerFromCustomer; }
            set
            {
                parcelAtCustomerFromCustomer = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ParcelAtCustomerFromCustomer"));
                }
            }
        }
        private List<BO.ParcelAtCustomer> parcelAtCustomerToCustomer;
        public List<BO.ParcelAtCustomer> ParcelAtCustomerToCustomer
        {
            get { return parcelAtCustomerToCustomer; }
            set
            {
                parcelAtCustomerToCustomer = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ParcelAtCustomerToCustomer"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

}
