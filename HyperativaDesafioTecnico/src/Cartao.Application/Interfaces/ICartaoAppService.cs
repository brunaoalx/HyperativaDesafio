using HyperativaDesafio.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperativaDesafio.Application.Interfaces
{
    public interface ICartaoAppService :IAppServiceBase<Cartao>
    {
        string GerarHashNumeroCartao(string numeroCartao);
        string GerarMascaraNumeroCartao(string numeroCartao);
        bool ValidarNumeroCartao(string numeroCartao);
        IEnumerable<Cartao> ObterCartaoPorHashNumero(string hashNumber);
        Cartao CadastrarCartaoManual(Cartao novo);
        ResumoProcessamento ProcessarArquivo(string caminhoCompletoArquivo);
    }
}
