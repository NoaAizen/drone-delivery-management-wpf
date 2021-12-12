using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace PL
{
    public class DronePO : INotifyPropertyChanged//(מחלקת עזר להזרמת מידע (נלמד בשיעור)
    {
        private int _Id;
        public int Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Id"));
                }
            }
        }
        private string _Model;
        public string Model
        {
            get { return _Model; }
            set
            {
                _Model = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Model"));
                }
            }
        }
        private BO.WeightCategories _MaxWeight;
        public BO.WeightCategories MaxWeight
        {
            get { return _MaxWeight; }
            set
            {
                _MaxWeight = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MaxWeight"));
                }
            }
        }
        private double _Longitude;
        public double Longitude
        {
            get { return _Longitude; }
            set
            {
                _Longitude = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Longitude"));
                }
            }
        }
        private double _Latitude;
        public double Latitude
        {
            get { return _Latitude; }
            set
            {
                _Latitude = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Latitude"));
                }
            }
        }
        private double _Battery;
        public double Battery
        {
            get { return _Battery; }
            set
            {
                _Battery = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Battery"));
                }
            }
        }
        private BO.StatusDrone _Status;
        public BO.StatusDrone Status
        {
            get { return _Status; }
            set
            {
                _Status = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("State"));
                }
            }
        }
        public int _ParcelTransferredNumber;

        public int ParcelTransferredNumber
        {
            get { return _ParcelTransferredNumber; }
            set
            {
                _ParcelTransferredNumber = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("NumberOfParcel"));
                }
            }
        }

        public BO.ParcelInTransfer _ParcelInTransfer;

        public BO.ParcelInTransfer ParcelInTransfer
        {
            get { return _ParcelInTransfer; }
            set
            {
                _ParcelInTransfer = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ParcelInTransfer"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
