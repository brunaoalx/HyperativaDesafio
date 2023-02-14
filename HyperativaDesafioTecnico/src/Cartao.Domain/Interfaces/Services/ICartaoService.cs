using HyperativaDesafio.Domain.Entities;

namespace HyperativaDesafio.Domain.Interfaces.Services
{
    public interface ICartaoService : IServiceBase<Cartao>
    {
        string GerarHashNumeroCartao(string numeroCartao);
        string GerarMascaraNumeroCartao(string numeroCartao);
        bool ValidarNumeroCartao(string numeroCartao);
        IEnumerable<Cartao> ObterCartaoPorHashNumero(string hashNumber);
        Cartao CadastrarCartaoManual(Cartao novo);

        ResumoProcessamento ProcessarArquivo(string caminhoCompletoArquivo);

    }
}
