using HyperativaDesafio.Domain.Entities;

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
