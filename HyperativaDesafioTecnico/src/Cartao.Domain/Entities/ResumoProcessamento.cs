using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperativaDesafio.Domain.Entities
{
    public class ResumoProcessamento
    {

        public string nomeArquivo { get; set; }
        public int qtdeTotalRegistros { get; set; }
        public int qtdeRegistrosErro { get; set; }
        public int qtdeRegistrosOk { get; set; }
        public string resultadoProcessamento { get; set; }
        public List<DetalheProcessamentoArquivo> detalheProcessamentoArquivo { get; set; }

        public ResumoProcessamento(string nomeArquivo, int totalRegistros)
        {
            this.nomeArquivo = nomeArquivo;
            this.qtdeTotalRegistros = totalRegistros;
            this.detalheProcessamentoArquivo = new();
        }

    }
}
