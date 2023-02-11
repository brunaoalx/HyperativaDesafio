using Dapper;
using HyperativaDesafio.Domain.Entities;
using HyperativaDesafio.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperativaDesafio.Infra.Data.Repositories
{
    public class CartaoRepository : RepositoryBase<Cartao>, ICartaoRepository
    {
        public CartaoRepository(string connectioString)
            : base(connectioString)
        {

        }
        public IEnumerable<Cartao> ObterCartaoPorNumero(string hashNumber)
        {

            var cartoes = DbContext.Connection.Query<Cartao>($"select * from cartao where numeroHash = '{hashNumber}'");

            return cartoes;
        }
    }
}
