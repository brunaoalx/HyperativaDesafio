using HyperativaDesafio.Application.Interfaces;
using HyperativaDesafio.Domain.Interfaces.Services;

namespace HyperativaDesafio.Application
{
    public class AppServiceBase<TEntity> : IDisposable, IAppServiceBase<TEntity> where TEntity : class
    {

        private readonly IServiceBase<TEntity> _serviceBase;


        public AppServiceBase(IServiceBase<TEntity> serviceBase)
        {
            _serviceBase = serviceBase;
        }

        public void Add(TEntity entity)
        {
            _serviceBase.Add(entity);
        }

        public void Dispose()
        {
            _serviceBase?.Dispose();
        }
    }
}
