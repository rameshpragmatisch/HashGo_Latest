using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class TagWithQuantity : INotifyPropertyChanged
    {
        private Tag _OrderTagItem;
        public Tag OrderTagItem
        {
            get
            {
                return _OrderTagItem;
            }
            set
            {
                _OrderTagItem = value;
                RaisePropertyChange("OrderTagItem");
            }
        }

        private MenuItem _MenuItem;
        public MenuItem MenuItem
        {
            get
            {
                return _MenuItem;
            }
            set
            {
                _MenuItem = value;
                RaisePropertyChange("MenuItem");
            }
        }

        private int quantity;
        public int Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                quantity = value;
                RaisePropertyChange("Quantity");
            }
        }

        private string resturantUniqueId;
        public string ResturantUniqueId
        {
            get
            {
                return resturantUniqueId;
            }
            set
            {
                resturantUniqueId = value;
                RaisePropertyChange("ResturantUniqueId");
            }
        }

        private int totalQuantity;
        public int TotalQuantity
        {
            get
            {
                return totalQuantity;
            }
            set
            {
                totalQuantity = value;
                RaisePropertyChange("TotalQuantity");
            }
        }

        private bool tagGroupDisplayMerged;
        public bool TagGroupDisplayMerged
        {
            get
            {
                return tagGroupDisplayMerged;
            }
            set
            {
                tagGroupDisplayMerged = value;
                RaisePropertyChange("TagGroupDisplayMerged");
            }
        }

        private string displayValue;
        public string DisplayValue
        {
            get
            {
                if (String.IsNullOrEmpty(GroupDisplayName) && !tagGroupDisplayMerged)
                    return displayValue;
                else
                    return "  " + displayValue;
            }
            set
            {
                displayValue = value;
                RaisePropertyChange("DisplayValue");
            }
        }

        public string GroupName;
        public string GroupDisplayName
        {
            get
            {
                if(!string.IsNullOrEmpty(GroupName))
                    return "- " + GroupName;
                else
                    return string.Empty;
            }
            set
            {
                GroupName = value;
                RaisePropertyChange("GroupDisplayName");
            }
        }


        private string workFlowName;
        public string WorkFlowName
        {
            get
            {
                return workFlowName;
            }
            set
            {
                workFlowName = value;
                RaisePropertyChange("WorkFlowName");
            }
        }


        private decimal totalPrice;
        public decimal TotalPrice
        {
            get
            {
                return totalPrice;
            }
            set
            {
                totalPrice = value;
                RaisePropertyChange("TotalPrice");
            }
        }

        public long OrderId { get; set; }

        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
        #endregion
    }
}
