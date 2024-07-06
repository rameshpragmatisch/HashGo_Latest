using CommunityToolkit.Mvvm.ComponentModel;
using HashGo.Domain.Models.Base;
using HashGo.Wpf.App.BestTech.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Wpf.App.Models.BestTech
{
    public class UISubCategory :ObservableObject
    {
        string name;
        public string Name { get; }

        string code;
        public string Code { get; }

        int categoryId;
        public int CategoryId { get; }

        string categoryName;
        public string CategoryName { get; }
        int id;
        public int Id { get; }

        public UISubCategory(string name,string code,int categoryId,  string categoryName, int id)
        {
            Name = name;
            Code = code;
            CategoryId = categoryId;
            CategoryName = categoryName;
            Id = id;
        }

        List<Unit> lstUnits = new List<Unit>();
        public ReadOnlyCollection<Unit> LstUnits { get => lstUnits.AsReadOnly(); }

        public void SetProducts(List<Unit> lstUnts)
        {
            lstUnits = lstUnts;
            OnPropertyChanged(nameof(LstUnits));
        }
    }
}
