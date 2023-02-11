using HyperativaDesafio.Domain.Entities;
using HyperativaDesafio.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperativaDesafio.Infra.Data.Repositories
{
    public class LoteRepository : RepositoryBase<Lote>, ILoteRepository
    {

        public LoteRepository(string connectioString)
            :base(connectioString)
        {

        }
    }
}
