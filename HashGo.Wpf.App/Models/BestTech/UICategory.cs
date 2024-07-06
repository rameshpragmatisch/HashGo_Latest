using CommunityToolkit.Mvvm.ComponentModel;
using HashGo.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Wpf.App.Models.BestTech
{
    public class UICategory : ObservableObject
    {
        string name;
        string categoryImage;

        public string Name { get => name; private set => name = value; }
        public string Code { get; }
        public string CategoryImage { get => categoryImage; private set => categoryImage = value; }
        public double MonthlyQtyLimit { get; }
        public int Id { get; }
        public bool CanShowText { get; set; } = true;

        List<UISubCategory> lstUISubCategories = new List<UISubCategory>();

        public ReadOnlyCollection<UISubCategory> LstUISubCategories { get { return lstUISubCategories.AsReadOnly(); } }

        public UICategory(string name, string code, string categoryImage, double monthlyQtyLimit, int id)
        {
            Name = name;
            Code = code;
            CategoryImage = categoryImage;
            MonthlyQtyLimit = monthlyQtyLimit;
            Id = id;
            if (categoryImage == CommonConstants.DEFAULTIMAGE)
                CanShowText = true;
            else CanShowText = false;
        }

        public void SetSubCategories(List<UISubCategory> subCategories)
        {
            lstUISubCategories = subCategories;
            OnPropertyChanged(nameof(LstUISubCategories));   
        }

    }
}
