using HashGo.Core.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HashGo.Core.Models;
using Prism.Events;

namespace HashGo.Core.Models
{
    public class MenuItem : INotifyPropertyChanged, IEquatable<MenuItem>
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public int ProductType { get; set; }
        public string Name { get; set; }
        public string AliasCode { get; set; }
        public string AliasName { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public string DisplayName
        {
            get { return Name is null? string.Empty : Name.ToUpper(); }
        }
        public string SubMenuTag { get; set; }
        public bool IsFavorite { get; set; }
        public int ConfirmationType { get; set; } = -1;
        public int SortOrder { get; set; }
        public string DownloadImage { get; set; }
        public int LocationId { get; set; }
        public int CategorieId { get; set; }
        public bool RequiresAuth { get; set; }
        public MenuPortion[] MenuPortions { get; set; }
        public MenuItemSchedule[] MenuItemSchedules { get; set; }
        public bool ParentCategoriesIsVisible { get; set; }

        public NutriGradeRating? MenuItemNutriGradeRating { get; set; }
        public int? MenuItemNutriGradeSugar { get; set; }
        public string MenuItemNutriGradeSugarString
        {
            get
            {
                if (MenuItemNutriGradeSugar.HasValue)
                {
                    return MenuItemNutriGradeSugar.Value.ToString();
                }
                return "";
            }
        }
        public string NutriGradeRatingString
        {
            get
            {
                if (MenuItemNutriGradeRating.HasValue)
                {
                    return MenuItemNutriGradeRating.Value.ToString();
                }
                return "";
            }
        }

        public string MenuItemNutriGradeColor
        {
            get
            {
                if (MenuItemNutriGradeRating.HasValue)
                {
                    if (MenuItemNutriGradeRating.Value == NutriGradeRating.A)
                    {
                        return "#06763b";
                    }
                    if (MenuItemNutriGradeRating.Value == NutriGradeRating.B)
                    {
                        return "#85c62e";
                    }
                    if (MenuItemNutriGradeRating.Value == NutriGradeRating.C)
                    {
                        return "#f7a707";
                    }
                    if (MenuItemNutriGradeRating.Value == NutriGradeRating.D)
                    {
                        return "#bf0000";
                    }
                }
                return "";
            }
        }

        public bool IsShowNutrition
        {
            get
            {
                return MenuItemNutriGradeSugar.HasValue || MenuItemNutriGradeRating.HasValue;
            }
        }
        public string TitleName => Regex.Replace(Name, @"\r\n?|\n", " ");

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

        public bool IsShowCustomize
        {
            get
            {
                if (OrderTags?.Count > 0)
                {
                    foreach (OrderTag? item in OrderTags)
                    {
                        if (item.Tags.Any())
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        private bool _isVisible = true;
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _enable = true;
        public bool Enable
        {
            get => _enable;
            set
            {
                _enable = value;
                OnPropertyChanged();
            }
        }

        private bool _isSoldOut;
        public bool IsSoldOut
        {
            get => _isSoldOut;
            set
            {
                _isSoldOut = value;
                OnPropertyChanged();
            }
        }

        private Combo _combo;
        public Combo Combo
        {
            get => _combo;
            set
            {
                _combo = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsShowComboText));
            }
        }

        private string _tempName;
        public string TempName
        {
            get => _tempName;
            set
            {
                _tempName = value;
                OnPropertyChanged();
            }
        }

        private string _GroupName;
        public string GroupName
        {
            get => _GroupName;
            set
            {
                _GroupName = value;
                OnPropertyChanged();
            }
        }

        private decimal _tempPrice;
        public decimal TempPrice
        {
            get => _tempPrice;
            set
            {
                _tempPrice = value;
                OnPropertyChanged();
            }
        }

        public bool IsShowComboText => Combo == null ? false : Combo.ComboGroups?.Count > 0 ? true : false;

        public MenuPortion? CheckedMenuPortion => MenuPortions != null && MenuPortions.Any(x => x.IsSelected) ? MenuPortions.FirstOrDefault(x => x.IsSelected) : null;

        [JsonIgnore]
        [IgnoreDataMember]
        public MenuItemSchedule[] MenuItemSplitDaysSchedules
        {
            get
            {
                List<MenuItemSchedule> misList = new List<MenuItemSchedule>();
                if (MenuItemSchedules != null)
                {
                    foreach (var mis in MenuItemSchedules)
                    {
                        if (mis.Days != null && mis.Days.Contains(","))
                        {
                            foreach (var day in mis.Days.Split(","))
                            {
                                var deepCloneSchedule = JsonConvert.DeserializeObject<MenuItemSchedule>(JsonConvert.SerializeObject(mis));
                                if (deepCloneSchedule != null)
                                {
                                    deepCloneSchedule.Days = day;
                                    misList.Add(deepCloneSchedule);
                                }
                            }
                        }
                        else if (mis.Days == null)
                        {
                            foreach (var day in new int[] { 1, 2, 3, 4, 5, 6, 7 })
                            {
                                var deepCloneSchedule = JsonConvert.DeserializeObject<MenuItemSchedule>(JsonConvert.SerializeObject(mis));
                                if (deepCloneSchedule != null)
                                {
                                    deepCloneSchedule.Days = day.ToString();
                                    misList.Add(deepCloneSchedule);
                                }
                            }
                        }
                    }
                }
                return misList.ToArray();
            }
        }

        [JsonIgnore]
        [IgnoreDataMember]
        public MenuPortion? NormalPortion
        {
            get
            {
                if (MenuPortions != null && MenuPortions.Any())
                {
                    var myFortPortion = MenuPortions.FirstOrDefault();
                    return myFortPortion;
                }
                return new MenuPortion() {};
            }
        }

        private ObservableCollection<UpMenuItem> _upMenuItems;
        public ObservableCollection<UpMenuItem> UpMenuItems
        {
            get
            {
                if (_upMenuItems == null)
                {
                    _upMenuItems = new ObservableCollection<UpMenuItem>();
                }
                return _upMenuItems;
            }
            set
            {
                _upMenuItems = value;
                OnPropertyChanged();
            }
        }

        private UpMenuItem _selectedUpMenuItem;
        public UpMenuItem SelectedUpMenuItem
        {
            get => _selectedUpMenuItem;
            set
            {
                _selectedUpMenuItem = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<OrderTag> _orderTags;

        [JsonIgnore]
        public ObservableCollection<OrderTag> OrderTags
        {
            get
            {
                if (_orderTags == null)
                {
                    _orderTags = new ObservableCollection<OrderTag>();
                    _orderTags.CollectionChanged += OrderTagsCollectionChanged;
                }
                return _orderTags;
            }
            set
            {
                _orderTags = value;
                OnPropertyChanged();
            }
        }

        public void SetOrderTags(List<OrderTag> orderTags)
        {
            _orderTags = new ObservableCollection<OrderTag>(orderTags);
        }

        private ObservableCollection<MenuItem> _checkedComboItems;
        public ObservableCollection<MenuItem> CheckedComboItems
        {
            get
            {
                if (_checkedComboItems == null)
                {
                    _checkedComboItems = new ObservableCollection<MenuItem>();
                }
                return _checkedComboItems;
            }
            set
            {
                _checkedComboItems = value;
                OnPropertyChanged();
            }
        }

        public void FindAllUpSelling()
        {
            if (UpMenuItems?.Count > 0)
            {
                foreach (UpMenuItem? item in UpMenuItems.Where(x => x.IsSelected))
                {
                    MenuPortion? portion = new MenuPortion { Price = item.Price, PortionName = "UpSelling", IsSelected = true };
                    MenuItem? upItem = JsonConvert.DeserializeObject<MenuItem>(JsonConvert.SerializeObject(item.MenuItem));
                    upItem.MenuPortions = new MenuPortion[] { portion };
                    upItem.ChooseQuantity = item.ChooseQuantity;
                    upItem.TempPrice = item.Price;

                    CheckedComboItems.Add(upItem);

                    FindAllCombo(upItem);
                }
            }
        }

        public void FindAllCombo(MenuItem menuItem)
        {
            if (menuItem?.Combo?.ComboGroups != null)
            {
                foreach (ComboGroup? item in menuItem.Combo.ComboGroups)
                {
                    foreach (ComboItem? cb in item.ComboItems.Where(x => x.IsSelected))
                    {
                        MenuItem? menu = new MenuItem();
                        menu.Name = cb.MenuItem.Name;
                        menu.TempName = cb.Name;
                        menu.TempPrice = cb.Price;
                        menu.MenuPortions = cb.MenuItem.MenuPortions;
                        MenuPortion? findPortion = menu.MenuPortions.FirstOrDefault(x => x.Id == cb.MenuItemPortionId);
                        if (findPortion != null)
                        {
                            findPortion.IsSelected = true;
                        }
                        menu.ChooseQuantity = cb.ChooseQuantity;
                        menu.OrderTags = cb.MenuItem.OrderTags;

                        CheckedComboItems.Add(menu);

                        FindAllCombo(cb.MenuItem);
                    }
                }
            }
        }



        private void OrderTagsCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(IsShowCustomize));
        }

        public ObservableCollection<Tag> CheckedTags
        {
            get
            {
                ObservableCollection<Tag>? items = new ObservableCollection<Tag>();
                foreach (OrderTag? item in OrderTags)
                {
                    foreach (Tag? ot in item.Tags.Where(x => x.IsSelected))
                    {
                        items.Add(ot);
                    }
                }
                return items;
            }
        }

        private decimal _totalPrice = 0;
        public decimal TotalPrice
        {
            get => _totalPrice;
            set
            {
                _totalPrice = value;
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

        public string BarCode { get; set; }

        public void AddQuantityCommand()
        {
            ChooseQuantity++;
            Refresh();
        }

        public void SubtractQuantityCommand()
        {
            ChooseQuantity--;
            Refresh();
        }

        private void Refresh()
        {
            CalculatePrice();
            //_eventAggregator.Publish(new CartMenuChangeEvent());
        }

        public void Reset()
        {
            TotalPrice = 0;
            ChooseQuantity = 1;

            if (Combo?.ComboGroups != null)
            {
                foreach (ComboGroup? item in Combo.ComboGroups)
                {
                    item.ComboQueue.Clear();
                    foreach (ComboItem? co in item.ComboItems)
                    {
                        co.IsSelected = false;
                        co.ChooseQuantity = 1;
                    }
                }
            }

            foreach (OrderTag? item in OrderTags)
            {
                item.TagQueue.Clear();
                foreach (Tag? tag in item.Tags)
                {
                    tag.IsSelected = false;
                    tag.ChooseQuantity = 1;
                }
            }
            OrderTags.Clear();

            if (MenuPortions != null)
            {
                foreach (MenuPortion? item in MenuPortions)
                {
                    item.IsSelected = false;
                }
            }

            CheckedComboItems.Clear();
            CheckedTags.Clear();
        }

        public void CheckSchedule()
        {
            if (MenuItemSplitDaysSchedules != null && MenuItemSplitDaysSchedules.Length > 0)
            {
                var dayOfWeek = (int)DateTime.Now.DayOfWeek;
                if (MenuItemSplitDaysSchedules.Select(x => x.DayOfWeek).Contains(dayOfWeek))
                {
                    foreach (var timeInterval in MenuItemSplitDaysSchedules.Where(x => x.DayOfWeek == dayOfWeek))
                    {
                        var startTime = DateTime.Parse($"{timeInterval.StartHour.ToString().PadLeft(2, '0')}:{timeInterval.StartMinute.ToString().PadLeft(2, '0')}:00");
                        var endTime = DateTime.Parse($"{timeInterval.EndHour.ToString().PadLeft(2, '0')}:{timeInterval.EndMinute.ToString().PadLeft(2, '0')}:00");
                        var nowDate = DateTime.Now;
                        if (startTime <= nowDate && nowDate <= endTime)
                        {
                            IsVisible = true;
                            return;
                        }
                    }
                }
                IsVisible = false;
            }
        }

        public void CalculatePrice()
        {
            decimal total = 0.0M;

            if (MenuPortions.Any(x => x.IsSelected))
            {
                total += MenuPortions.First(x => x.IsSelected).Price * ChooseQuantity;
            }

            foreach (Tag? ct in CheckedTags)
            {
                ct.TotalQuantity = ct.ChooseQuantity * ChooseQuantity;
                total += ct.Price * ct.TotalQuantity;
            }

            foreach (MenuItem? cci in CheckedComboItems)
            {
                cci.TotalQuantity = cci.ChooseQuantity * ChooseQuantity;
                total += cci.TempPrice * cci.TotalQuantity;

                foreach (Tag? ct in cci.CheckedTags)
                {
                    ct.TotalQuantity = ct.ChooseQuantity * ChooseQuantity;
                    total += ct.Price * ct.TotalQuantity;
                }
            }

            TotalPrice = total;
        }


        //private readonly IEventAggregator _eventAggregator = Locator.Current.GetService<IEventAggregator>();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public bool Equals(MenuItem? other)
        {
            try
            {
                if (other == null)
                    return false;

                if (MenuItemId != other.MenuItemId)
                    return false;

                if (CheckedMenuPortion?.Id != other.CheckedMenuPortion?.Id)
                    return false;

                for (int i = 0; i < CheckedTags.Count; i++)
                {
                    if (!CheckedTags[i].Equals(other.CheckedTags[i]))
                        return false;
                }

                for (int i = 0; i < CheckedComboItems.Count; i++)
                {
                    if (!CheckedComboItems[i].Equals(other.CheckedComboItems[i]))
                        return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
