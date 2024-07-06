using CommunityToolkit.Mvvm.ComponentModel;

namespace HashGo.Domain.Models.Base
{
    public partial class GenericBaseModel<T> : BaseModel
        where T : class
    {
        [ObservableProperty]
        T data;

        public GenericBaseModel(T data)
        {
            this.Data = data;
        }
    }
}
