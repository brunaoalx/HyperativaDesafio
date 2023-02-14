namespace HyperativaDesafio.Domain.Interfaces.Repositories
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        public void Add(TEntity entity, string query);

        public void Add(TEntity entity);

        void Dispose();
    }
}
