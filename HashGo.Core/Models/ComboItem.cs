using HashGo.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class ComboItem : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MenuItemId { get; set; }
        public int MenuItemPortionId { get; set; }
        public bool AutoSelect { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public bool AddSeperately { get; set; }
        public string ButtonColor { get; set; }
        public int SortOrder { get; set; }

        public string Files { get; set; }
        public decimal Total => Price * ChooseQuantity;


        private MenuItem _menuItem;
        public MenuItem MenuItem
        {
            get => _menuItem;
            set
            {
                _menuItem = value;
                OnPropertyChanged();
            }
        }


        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                if (!value)
                {
                    ChooseQuantity = 1;
                }
                OnPropertyChanged();
            }
        }

        private int _chooseQuantity = 1;
        public int ChooseQuantity
        {
            get => _chooseQuantity;
            set
            {
                _chooseQuantity = value < 1 ? 1 : value;
                OnPropertyChanged();
            }
        }

        public void AddQuantityCommand(ComboGroup comboGroup)
        {
            int count = comboGroup.ComboItems.Where(x => x.IsSelected).Sum(x => x.ChooseQuantity);
            if (count == comboGroup.Maximum)
            {
                if (comboGroup.ComboQueue.Count > 1)
                {
                    ComboItem? first = comboGroup.ComboQueue.Where(x => x.Id != Id).First();
                    if (first.ChooseQuantity - 1 == 0)
                    {
                        first.IsSelected = false;
                        comboGroup.ComboQueue.Remove(first);
                    }
                    else
                    {
                        first.ChooseQuantity--;
                    }
                }

                if (comboGroup.ComboItems.Where(x => x.IsSelected).Sum(x => x.ChooseQuantity) + 1 <= comboGroup.Maximum)
                {
                    ChooseQuantity++;
                }
            }
            else if (count < comboGroup.Maximum || comboGroup.Maximum == 0)
            {
                ChooseQuantity++;
            }
        }

        public void SubtractQuantityCommand(ComboGroup comboGroup)
        {
            IEnumerable<ComboItem>? findCombos = comboGroup.ComboItems.Where(x => x.IsSelected);
            if (findCombos.Sum(x => x.ChooseQuantity) - 1 >= comboGroup.Minimum)
            {
                if (ChooseQuantity == 1)
                {
                    if (comboGroup.Minimum == 0 ||
                        (comboGroup.Minimum > 0 && comboGroup.ComboItems.Where(x => x.Id != Id && x.IsSelected).Sum(x => x.ChooseQuantity) >= comboGroup.Minimum))
                    {
                        IsSelected = false;
                        comboGroup.ComboQueue.Remove(this);
                    }
                }
                ChooseQuantity--;
            }
        }

        public void ComboCommand(IList<object> values)
        {
            try
            {
                if (values.Count > 1 && values[0] is ComboGroup comboGroup && values[1] is ComboItem modifyComboItem)
                {
                    if (modifyComboItem?.MenuItem?.Combo?.ComboGroups?.Count > 0)
                    {

                    }

                    if (modifyComboItem.IsSelected)
                    {
                        modifyComboItem.AddQuantityCommand(comboGroup);
                    }
                    else
                    {
                        int count = comboGroup.ComboItems.Where(x => x.IsSelected).Sum(x => x.ChooseQuantity);
                        if (count + 1 > comboGroup.Maximum && comboGroup.Maximum > 0)
                        {
                            if (comboGroup.ComboQueue[0].ChooseQuantity > 1)
                            {
                                comboGroup.ComboQueue[0].ChooseQuantity--;
                            }
                            else
                            {
                                comboGroup.ComboQueue[0].IsSelected = false;
                                comboGroup.ComboQueue.RemoveAt(0);
                            }
                        }
                        modifyComboItem.IsSelected = true;
                        comboGroup.ComboQueue.Add(modifyComboItem);
                    }
                }
            }
            catch (Exception ex)
            {
                //NLogger.Error(ex);
            }
        }

        //public void CustomizeCommand()
        //{
        //    _eventAggregator.Publish(new OrderTagAddEvent(MenuItem.OrderTags.ToList()));
        //}

        //private readonly IEventAggregator _eventAggregator = Locator.Current.GetService<IEventAggregator>();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
