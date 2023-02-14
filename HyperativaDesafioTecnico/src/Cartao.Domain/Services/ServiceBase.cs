using HyperativaDesafio.Domain.Interfaces.Repositories;
using HyperativaDesafio.Domain.Interfaces.Services;

namespace HyperativaDesafio.Domain.Services
{
    public abstract class ServiceBase<TEntity> : IDisposable, IServiceBase<TEntity> where TEntity : class
    {
        private readonly IRepositoryBase<TEntity> _repository;
        public ServiceBase(IRepositoryBase<TEntity> repository)
        {
            _repository = repository;
        }

        public void Add(TEntity entity)
        {
            _repository.Add(entity);
        }

        public void Add(TEntity entity, string query)
        {
            try
            {
                _repository.Add(entity, query);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
