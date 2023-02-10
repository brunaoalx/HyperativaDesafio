using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperativaDesafio.Domain.Entities
{
    internal class Cartao
    {
        public int id { get; set; }
        public string numeroHash { get; set; }
        public string numeroMascara { get; set; }
        public string numeracaoNoLote { get; set; } 
        public DateTime dataCadastro { get; set; }
        public Lote lote { get; set; }
    }
}
