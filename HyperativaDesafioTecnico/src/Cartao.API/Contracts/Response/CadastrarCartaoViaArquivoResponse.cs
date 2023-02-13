using HyperativaDesafio.Domain.Entities;

namespace HyperativaDesafio.API.Contracts.Response
{
    public class CadastrarCartaoViaArquivoResponse
    {
        public string nomeArquivo { get; set; }
        public int qtdeTotalRegistros { get; set; }
        public int qtdeRegistrosErro { get; set; }
        public int qtdeRegistrosOk { get; set; }
        public string resultadoProcessamento { get; set; }
        public List<CadastrarCartaoViaArquivoResponseDetalhe> detalheProcessamentoArquivo { get; set; }
    }
}
