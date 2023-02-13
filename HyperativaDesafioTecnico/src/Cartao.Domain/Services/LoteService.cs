using HyperativaDesafio.Domain.Entities;
using HyperativaDesafio.Domain.Interfaces.Repositories;
using HyperativaDesafio.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
