using System;
using BO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PL
{
    public class ParcelPo : INotifyPropertyChanged//(מחלקת עזר להזרמת מידע (נלמד בשיעור
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
        private CustomerInParcel customerInParcelSender;
        public CustomerInParcel CustomerInParcelSender
        {
            get { return customerInParcelSender; }
            set
            {
                customerInParcelSender = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CustomerInParcelSender"));
                }
            }
        }
        private CustomerInParcel customerInParcelRecipient;
        public CustomerInParcel CustomerInParcelRecipient
        {
            get { return customerInParcelRecipient; }
            set
            {
                customerInParcelRecipient = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CustomerInParcelRecipient"));
                }
            }
        }
        private BO.WeightCategories weight;
        public BO.WeightCategories Weight
        {
            get { return weight; }
            set
            {
                weight = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Weight"));
                }
            }
        }
        private BO.Priorities priority;
        public BO.Priorities Priority
        {
            get { return priority; }
            set
            {
                priority = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Priority"));
                }
            }
        }
        private BO.DroneInParcel droneInParcel;
        public BO.DroneInParcel DroneInParcel
        {
            get { return droneInParcel; }
            set
            {
                droneInParcel = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("DroneInParcel"));
                }
            }
        }
        private DateTime? requested;
        public DateTime? Requested
        {
            get { return requested; }
            set
            {
                requested = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Requested"));
                }
            }
        }
        private DateTime? scheduled;
        public DateTime? Scheduled
        {
            get { return scheduled; }
            set
            {
                scheduled = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Scheduled"));
                }
            }
        }
        private DateTime? pickedUp;
        public DateTime? PickedUp
        {
            get { return pickedUp; }
            set
            {
                pickedUp = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PickedUp"));
                }
            }
        }
        private DateTime? delivered;
        public DateTime? Delivered
        {
            get { return delivered; }
            set
            {
                delivered = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Delivered"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
