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
    public class CartaoService : ServiceBase<Cartao>, ICartaoService
    {

        private readonly ICartaoRepository _cartaoRepository;
        

        public CartaoService(ICartaoRepository cartaoRepository)
            :base(cartaoRepository)
        {
            _cartaoRepository = cartaoRepository;
        }

        public string GerarHashNumeroCartao(string numeroCartao)
        {
            /*
             PCI DSS Requirement 3.4
             All PAN’s strong one-way hash functions
             Fonte: https://www.pcidssguide.com/pci-dss-requirements/
             On: 2023-02-11

             */
            return SecurityService.GerarHashSha256(numeroCartao);
        }

        public string GerarMascaraNumeroCartao(string numeroCartao)
        {

            try
            {

                if (ValidarNumeroCartao(numeroCartao) == false) 
                    throw new Exception("Numero inválido para Cartao de Credito."); 

                return SecurityService.MarcararNumeroCartao(numeroCartao);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool ValidarNumeroCartao(string numeroCartao)
        {

            return SecurityService.ValidaNumeroCartao(numeroCartao);

        }

        public Cartao CadastrarCartaoManual (Cartao novoCartao)
        {

            try
            {
                Lote loteManual = _cartaoRepository.ObtemLoteParaCadastroManual();

                novoCartao.lote = loteManual.id;

                var cartaoCadastrado = _cartaoRepository.CadastrarCartao(novoCartao);

                return cartaoCadastrado;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public IEnumerable<Cartao> ObterCartaoPorHashNumero(string hashNumber)
        {
            return _cartaoRepository.ObterCartaoPorHashNumero(hashNumber);
        }

        public void CadastraCartaoViaArquivo(List<Cartao> cartoesNovos)
        {

        }

        public void Add(Cartao cartao)
        {

            CadastrarCartaoManual(cartao);

        }
    }
}
