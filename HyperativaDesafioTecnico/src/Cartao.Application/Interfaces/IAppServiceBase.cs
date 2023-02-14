namespace HyperativaDesafio.Application.Interfaces
{
    public interface IAppServiceBase<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void Dispose();
    }
}
