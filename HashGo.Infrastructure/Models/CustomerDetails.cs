
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Infrastructure.Models
{
    public class CustomerDetails : INotifyPropertyChanged
    {
        string name;

        public string Name { get => name; set { name = value; OnPropertyChanged(); } }

        string contactNumber;
        public string ContactNumber { get => contactNumber; set { contactNumber = value; OnPropertyChanged(); } }

        int? postalCode;
        public int? PostalCode
        {
            get => postalCode; set { postalCode = value; OnPropertyChanged(); }
        }

        string unitNo;
        public string UnitNo { get => unitNo; set { unitNo = value; OnPropertyChanged(); } }

        string floorNo;
        public string FloorNo { get => floorNo; set { floorNo = value; OnPropertyChanged(); } }

        public string Remarks { get => remarks; set { remarks = value; OnPropertyChanged(); } }

        

        string remarks;

        string addressLine1;
        public string AddressLine1 { get => addressLine1; set { addressLine1 = value; OnPropertyChanged(); } }

        string addressLine2;
        public string AddressLine2 { get => addressLine2; set { addressLine2 = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }
    }
}
