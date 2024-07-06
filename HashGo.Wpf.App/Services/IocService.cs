using HashGo.Core.Contracts.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Wpf.App.Services
{
    public class IocService : IIocService
    {
        private readonly IServiceProvider _serviceProvider;

        public IocService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T GetObject<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }

    }
}
