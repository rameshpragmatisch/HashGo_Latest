using HashGo.Core.Contracts.Services;

namespace HashGo.Domain.ViewModels
{
    public abstract class BaseDataProviderViewModel<T> : BaseHashGoViewModel
    {
        private readonly T _brandService;
        private readonly ILoggingService logger;

        protected BaseDataProviderViewModel(ILoggingService loggingService, T brandService)
        {
            this._brandService = brandService;
            this.logger = loggingService;
        }

        protected T BrandService { get { return _brandService; } }

        protected ILoggingService Logger { get { return logger; } }
    }
}
