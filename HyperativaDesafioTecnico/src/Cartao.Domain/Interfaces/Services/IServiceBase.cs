namespace HyperativaDesafio.Domain.Interfaces.Services
{
    public interface IServiceBase<Tentity> where Tentity : class
    {
        void Add(Tentity entity, string query);
        void Add(Tentity entity);
        void Dispose();
    }
}
