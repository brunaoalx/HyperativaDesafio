using HyperativaDesafio.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperativaDesafio.Domain.Interfaces.Repositories
{
    public interface ICartaoRepository : IRepositoryBase<Cartao>
    {

        IEnumerable<Cartao> ObterCartaoPorHashNumero(string hashNumber);

        Cartao CadastrarCartao(Cartao novo);

        Lote ObtemLoteParaCadastroManual();

        Lote ObterLoteParaArquivo(string linhaDadosArquivo);


    }
}
