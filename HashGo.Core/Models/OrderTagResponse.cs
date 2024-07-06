using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class OrderTagResponse
    {
        public OrderTag[] Items;
    }

    public class OrderTag : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }
        public int MaxSelectedItems { get; set; }
        public int MinSelectedItems { get; set; }
        public int SortOrder { get; set; }
        public bool AddTagPriceToOrderPrice { get; set; }
        public bool SaveFreeTags { get; set; }
        public bool FreeTagging { get; set; }
        public bool TaxFree { get; set; }
        public string Departments { get; set; }

        public List<DisplayValue> DepartmentsList
        {
            get
            {
                return JsonConvert.DeserializeObject<List<DisplayValue>>(Departments);
            }
        }
        public Map[] Maps { get; set; }
        public string Files { get; set; }

        //public string QuantitySelectPromptText => $"{Name} ( {Localizer.Instance["Min"]} {MinSelectedItems}, {Localizer.Instance["Max"]} {MaxSelectedItems} )";

        [System.Text.Json.Serialization.JsonIgnore]
        [IgnoreDataMember]
        public List<Tag> TagQueue = new List<Tag>();

        private ObservableCollection<Tag> _tags;
        public ObservableCollection<Tag> Tags
        {
            get
            {
                if (_tags == null)
                {
                    _tags = new ObservableCollection<Tag>();
                }
                return _tags;
            }
            set
            {
                _tags = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class Tag : INotifyPropertyChanged, IEquatable<Tag>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AlternateName { get; set; }
        public string GroupName { get; set; }
        public int GroupMaxSelection { get; set; }
        public int SortOrder { get; set; }
        public decimal Price { get; set; }
        public int MaxQuantity { get; set; }

        private string _files;
        public string Files
        {
            get => _files;
            set
            {
                _files = value;
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

        public void AddQuantityCommand(OrderTag orderTag)
        {
            int count = orderTag.Tags.Where(x => x.IsSelected).Sum(x => x.ChooseQuantity);
            if (count == orderTag.MaxSelectedItems)
            {
                if (orderTag.TagQueue.Count > 1)
                {
                    Tag? first = orderTag.TagQueue.Where(x => x.Id != Id).First();
                    if (first.ChooseQuantity - 1 == 0)
                    {
                        first.IsSelected = false;
                        orderTag.TagQueue.Remove(first);
                    }
                    else
                    {
                        first.ChooseQuantity--;
                    }
                }

                if (orderTag.Tags.Where(x => x.IsSelected).Sum(x => x.ChooseQuantity) + 1 <= orderTag.MaxSelectedItems)
                {
                    ChooseQuantity++;
                }
            }
            else if (count < orderTag.MaxSelectedItems || orderTag.MaxSelectedItems == 0)
            {
                ChooseQuantity++;
            }
        }

        public void SubtractQuantityCommand(OrderTag orderTag)
        {
            IEnumerable<Tag>? findCombos = orderTag.Tags.Where(x => x.IsSelected);
            if (findCombos.Sum(x => x.ChooseQuantity) - 1 >= orderTag.MinSelectedItems)
            {
                if (ChooseQuantity == 1)
                {
                    if (orderTag.MinSelectedItems == 0 ||
                        (orderTag.MinSelectedItems > 0 && orderTag.Tags.Where(x => x.Id != Id && x.IsSelected).Sum(x => x.ChooseQuantity) >= orderTag.MinSelectedItems))
                    {
                        IsSelected = false;
                        orderTag.TagQueue.Remove(this);
                    }
                }
                ChooseQuantity--;
            }
        }

        public void TagCommand(IList<object> values)
        {
            try
            {
                if (values.Count > 1 && values[0] is OrderTag orderTag && values[1] is Tag modifyTag)
                {
                    if (modifyTag.IsSelected)
                    {
                        modifyTag.AddQuantityCommand(orderTag);
                    }
                    else
                    {
                        int count = orderTag.Tags.Where(x => x.IsSelected).Sum(x => x.ChooseQuantity);
                        if (count + 1 > orderTag.MaxSelectedItems && orderTag.MaxSelectedItems > 0)
                        {
                            if (orderTag.TagQueue[0].ChooseQuantity > 1)
                            {
                                orderTag.TagQueue[0].ChooseQuantity--;
                            }
                            else
                            {
                                orderTag.TagQueue[0].IsSelected = false;
                                orderTag.TagQueue.RemoveAt(0);
                            }
                        }
                        modifyTag.IsSelected = true;
                        orderTag.TagQueue.Add(modifyTag);
                    }
                }
            }
            catch (Exception ex)
            {
                //NLogger.Error(ex);
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

        private int _totalQuantity = 0;
        public int TotalQuantity
        {
            get => _totalQuantity;
            set
            {
                _totalQuantity = value;
                OnPropertyChanged();
            }
        }


        public bool Equals(Tag? other)
        {
            return Id == other?.Id && ChooseQuantity == other?.ChooseQuantity;
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class Map
    {
        public int Id { get; set; }
        public string CategoryId { get; set; }
        public string MenuItemId { get; set; }
    }
}
