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
            return SHA.ComputeSHA256Hash(valorEntrada + keyToHash);
        }
    }
}
