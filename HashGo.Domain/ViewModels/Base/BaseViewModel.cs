using CommunityToolkit.Mvvm.ComponentModel;

namespace HashGo.Domain.ViewModels.Base
{
    public partial class BaseViewModel : ObservableObject, IBaseViewModel
    {
        public virtual void ViewLoaded()
        {

        }

        public virtual void ViewUnloaded()
        {

        }
    }

    public interface IBaseViewModel
    {
        void ViewLoaded();
        void ViewUnloaded();
    }
}