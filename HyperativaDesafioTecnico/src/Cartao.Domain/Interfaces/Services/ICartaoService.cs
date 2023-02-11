using HyperativaDesafio.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperativaDesafio.Domain.Interfaces.Services
{
    internal interface ICartaoService : IServiceBase<Cartao>
    {
        string GerarHashNumeroCartao(string numeroCartao);
        string GerarMascaraNumeroCartao(string numeroCartao);
        bool ValidarNumeroCartao(string numeroCartao);
    }
}
