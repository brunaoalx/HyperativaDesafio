using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncryption;

namespace HyperativaDesafio.Domain.Services
{
    public static class SecurityService
    {

        private static string keyToHash = "YGDIWKFcerkaD9KwCVUOcnaivlNfoHawU4AnflzlRMoxiqY23XDOulMbWj8RE3N9gvFYURj4f4Npo/x8HICmHDPzfdv/S/0Zj/Jwk35PHzbF17eKPcVOhcyjbLJfm/zmGmmDMI98/BsxVuxE/lHcPmFKR5Ra7LJFqww+8YVIljY=";

        public static string GerarHashSha256(string valorEntrada)
        {
            /*
             PCI DSS Requirement 3.4
             All PAN’s strong one-way hash functions
             Fonte: https://www.pcidssguide.com/pci-dss-requirements/
             On: 2023-02-11

             */
            return SHA.ComputeSHA256Hash(valorEntrada + keyToHash);
        }

        public static bool ValidaNumeroCartao(string numeroCartao)
        {
            /*
             Serasa: Os cartões de crédito normalmente têm entre 13 e 16 dígitos de identificação
             Source:https://www.serasa.com.br/blog/numeros-do-cartao-de-credito-o-que-eu-preciso-saber/
             */

            if (String.IsNullOrEmpty(numeroCartao))
                return false;

            if (int.TryParse(numeroCartao, out _) == false)
                return false;

            if (numeroCartao.Length < 13 || numeroCartao.Length > 16)
                return false;

            return true;
        }

        public static string MarcararNumeroCartao (string numero)
        {
            /*
             PCI DSS Requirement 3.3 The maximum number that can be shown is the first six and the last four digits. 
             Source: https://www.pcidssguide.com/pci-dss-requirements/
             On: 2023-02-11 

                Vamos adotar o padrão de exibir os 4 primeiros e os 4 ultimos digitos, que é a pratica o mais comum.
            */

            string numeracaoPrefixo = numero.Substring(0, 4);
            string numeracaoSufixo = numero.Substring(numero.Length - 4, 4);

            return $"{numeracaoPrefixo}***{numeracaoSufixo}";

        }
    }
}
