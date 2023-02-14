using HyperativaDesafio.Domain.Entities;

namespace HyperativaDesafio.Domain.Interfaces.Repositories
{
    public interface ILoteRepository : IRepositoryBase<Lote>
    {
        public Lote ObtemLotePorParametros(string tipoLote, string dataProcessamento);
    }
}
