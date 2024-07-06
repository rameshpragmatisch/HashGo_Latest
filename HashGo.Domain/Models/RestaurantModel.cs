using CommunityToolkit.Mvvm.ComponentModel;
using HashGo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Domain.Models
{
    public partial class RestaurantModel : Base.GenericBaseModel<RestaurantBrand>
    {
        [ObservableProperty]
        int column;
        
        [ObservableProperty]
        int row;

        public RestaurantModel(RestaurantBrand data) : base(data)
        {
        }
    }
}
