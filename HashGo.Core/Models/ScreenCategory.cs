using HashGo.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HashGo.Core.Models
{
    public class ScreenCategory : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public MenuItem[] MenuItems { get; set; }
        public int SortOrder { get; set; }
        public string HomeImagePath { get; set; }
        public ProductGroup ProductGroup { get; set; }
        public string ProductGroupId { get; set; }
        public bool MostUsedItemsCategory { get; set; }
        public string ScreenMenuCategorySchedule { get; set; }
        public CategorySchedule[] Schedule { get; set; }

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

        private bool _isVisible = true;
        public bool IsVisible
        {
            get => _isVisible; 
            set
            {
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        private void SetIsVisible(bool value)
        {
            if (_isVisible != value)
            {
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public bool IsShowTagButton
        {
            get
            {
                if (TagMenuTuples.Count == 1 && TagMenuTuples.Any(x => x.Item1 == "All"))
                {
                    return false;
                }
                return true;
            }
        }


        public bool CheckSchedule()
        {
            try
            {
                if (!string.IsNullOrEmpty(ScreenMenuCategorySchedule))
                {
                    Schedule = JsonConvert.DeserializeObject<CategorySchedule[]>(ScreenMenuCategorySchedule);
                }
                if (Schedule != null && Schedule.Length > 0)
                {
                    foreach (var s in Schedule)
                    {
                        if (s.Start <= DateTime.Now && DateTime.Now <= s.End)
                        {
                            var dayOfWeek = (int)DateTime.Now.DayOfWeek;
                            var day = (int)DateTime.Now.Day;

                            if (s.AllDays.Select(x => x.ValueOfInt).Contains(dayOfWeek) || s.AllMonthDays.Select(x => x.ValueOfInt).Contains(day)
                                || (!s.AllDays.Any() && !s.AllMonthDays.Any()))
                            {
                                var startTime = DateTime.Parse($"{s.StartHour.PadLeft(2, '0')}:{s.StartMinute.PadLeft(2, '0')}:00");
                                var endTime = DateTime.Parse($"{s.EndHour.PadLeft(2, '0')}:{s.EndMinute.PadLeft(2, '0')}:00");
                                var nowDate = DateTime.Now;
                                if (startTime <= nowDate && nowDate <= endTime)
                                {
                                    SetIsVisible(true);
                                    return IsVisible;
                                }
                            }
                        }
                    }
                    SetIsVisible(false);
                    return IsVisible;
                }
                return IsVisible;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private ObservableCollection<Tuple<string, ObservableCollection<MenuItem>>> _tagMenuTuples;
        public ObservableCollection<Tuple<string, ObservableCollection<MenuItem>>> TagMenuTuples
        {
            get
            {
                if (_tagMenuTuples == null)
                {
                    _tagMenuTuples = new ObservableCollection<Tuple<string, ObservableCollection<MenuItem>>>();
                    _tagMenuTuples.Add(new Tuple<string, ObservableCollection<MenuItem>>("All", new ObservableCollection<MenuItem>(MenuItems)));
                    IEnumerable<IGrouping<string, MenuItem>>? groupItems = MenuItems.Where(x => !string.IsNullOrEmpty(x.SubMenuTag)).GroupBy(x => x.SubMenuTag);
                    if (groupItems?.Count() > 0)
                    {
                        foreach (IGrouping<string, MenuItem>? item in groupItems)
                        {
                            _tagMenuTuples.Add(new Tuple<string, ObservableCollection<MenuItem>>(
                                item.Key,
                                new ObservableCollection<MenuItem>(item.ToArray())
                            ));
                        }
                    }
                }
                return _tagMenuTuples;
            }
            set
            {
                _tagMenuTuples = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
