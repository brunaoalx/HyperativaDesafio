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

            return CartaoSecurityService.GerarHashSha256(numeroCartao);
        }

        public string GerarMascaraNumeroCartao(string numeroCartao)
        {
            /*
             PCI DSS Requirement 3.3 The maximum number that can be shown is the first six and the last four digits. 
             Source: https://www.pcidssguide.com/pci-dss-requirements/
             On: 2023-02-11 

                Vamos adotar o padrão de exibir os 4 primeiros e os 4 ultimos digitos, que é a pratica o mais comum.

            */

            try
            {

                if (!ValidarNumeroCartao(numeroCartao)) { throw new Exception("Numero inválido para Cartao de Credito."); }

                string numeracaoPrefixo = numeroCartao.Substring(0, 4);
                string numeracaoSufixo = numeroCartao.Substring(numeroCartao.Length - 4,4);

                return $"{numeracaoPrefixo}***{numeracaoSufixo}";

            }
            catch (Exception)
            {

                throw;
            }


            throw new NotImplementedException();
        }

        public bool ValidarNumeroCartao(string numeroCartao)
        {
            /*
             Serasa: Os cartões de crédito normalmente têm entre 13 e 16 dígitos de identificação
             Source:https://www.serasa.com.br/blog/numeros-do-cartao-de-credito-o-que-eu-preciso-saber/
             */

            if (String.IsNullOrEmpty(numeroCartao))
                return false;

            if(int.TryParse(numeroCartao,out _) == false)
                return false;

            if(numeroCartao.Length < 13 || numeroCartao.Length > 16)
                return false;

            return true;

        }

        public Cartao CadastrarCartaoManual (Cartao novoCartao)
        {

            try
            {
                var cartaoPrepado = PreparaDadosAntesDoCadastro(novoCartao);
                
                var cartaoCadastrado = _cartaoRepository.CadastrarCartao(novoCartao);

                return cartaoCadastrado;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private Cartao PreparaDadosAntesDoCadastro(Cartao novoCartao)
        {
            novoCartao.dataCadastro = DateTime.Now;
            Lote loteManual = _cartaoRepository.ObtemLoteParaCadastroManual();

            novoCartao.lote = loteManual.id;
            //Cadastrar dentro do lote correto

            return novoCartao;

        }

        public IEnumerable<Cartao> ObterCartaoPorHashNumero(string hashNumber)
        {
            return _cartaoRepository.ObterCartaoPorHashNumero(hashNumber);
        }

        public void CadastraCartaoViaArquivo(List<Cartao> cartoesNovos)
        {

        }
    }
}
