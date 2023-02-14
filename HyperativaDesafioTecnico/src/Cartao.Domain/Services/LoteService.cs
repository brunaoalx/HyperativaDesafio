using HyperativaDesafio.Domain.Entities;
using HyperativaDesafio.Domain.Interfaces.Repositories;
using HyperativaDesafio.Domain.Interfaces.Services;

namespace HyperativaDesafio.Domain.Services
{
    public class LoteService : ServiceBase<Lote>, ILoteService
    {

        private readonly ILoteRepository _loteRepository;

        public LoteService(ILoteRepository loteRepository)
            : base(loteRepository)
        {
            _loteRepository = loteRepository;
        }

    }
}
