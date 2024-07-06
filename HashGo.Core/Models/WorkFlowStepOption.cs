using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class WorkFlowStepOption : NameIdBase, INotifyPropertyChanged
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        public string Amount { get; set; }

        public long WorkFlowStepId {  get; set; }

        public bool IsSelected { get; set; }

        public Tag OrderTagItem { get; set; }
        public MenuItem MenuItem { get; set; }
        public int Quantity { get; set; }

        private bool _CanAddQuantity;

        public bool CanAddQuantity
        {
            get { return _CanAddQuantity; }
            set
            {
                _CanAddQuantity = value;
                RaisePropertyChange("CanAddQuantity");

            }
        }


        private int _TotalQuantity;

        public int TotalQuantity
        {
            get { return _TotalQuantity; }
            set 
            { 
                _TotalQuantity = value;
                RaisePropertyChange("TotalQuantity");

            }
        }


        public void RaiseSelectedTagsWithQuantities()
        {
            RaisePropertyChange("TotalQuantity");
        }

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
