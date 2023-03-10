using HyperativaDesafio.Application.Interfaces;
using HyperativaDesafio.Domain.Entities;
using HyperativaDesafio.Domain.Interfaces.Services;

namespace HyperativaDesafio.Application
{
    public class CartaoAppService : AppServiceBase<Cartao>, ICartaoAppService
    {

        private readonly ICartaoService _cartaoService;

        public CartaoAppService(ICartaoService cartaoService)
            : base(cartaoService)
        {
            _cartaoService = cartaoService;
        }

        public Cartao CadastrarCartaoManual(Cartao novo)
        {
            return _cartaoService.CadastrarCartaoManual(novo);

        }

        public string GerarHashNumeroCartao(string numeroCartao)
        {
            return _cartaoService.GerarHashNumeroCartao(numeroCartao);
        }

        public string GerarMascaraNumeroCartao(string numeroCartao)
        {
            return _cartaoService.GerarMascaraNumeroCartao(numeroCartao);
        }

        public IEnumerable<Cartao> ObterCartaoPorHashNumero(string hashNumber)
        {
            return _cartaoService.ObterCartaoPorHashNumero(hashNumber);
        }

        public ResumoProcessamento ProcessarArquivo(string caminhoCompletoArquivo)
        {
            return _cartaoService.ProcessarArquivo(caminhoCompletoArquivo);
        }

        public bool ValidarNumeroCartao(string numeroCartao)
        {
            return _cartaoService.ValidarNumeroCartao(numeroCartao);
        }
    }
}
