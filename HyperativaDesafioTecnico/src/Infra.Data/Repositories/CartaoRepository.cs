using Dapper;
using HyperativaDesafio.Domain.Entities;
using HyperativaDesafio.Domain.Interfaces.Repositories;

namespace HyperativaDesafio.Infra.Data.Repositories
{
    public class CartaoRepository : RepositoryBase<Cartao>, ICartaoRepository
    {

        LoteRepository _loteRepository = new LoteRepository();
        public IEnumerable<Cartao> ObterCartaoPorHashNumero(string hashNumber)
        {

            var cartoes = DbContext.Connection.Query<Cartao>($"select * from cartao where numeroHash = '{hashNumber}'");

            return cartoes;
        }

        public Cartao CadastrarCartao(Cartao novoCartao)
        {

            string queryInsert =
                "insert into cartao (" +
                "numeroHash" +
                ",numeroMascara " +
                ",dataCadastro" +
                ",numeracaoLote" +
                ",Lote)" +
                "values(" +
                " @numeroHash" +
                ",@numeroMascara" +
                ",@dataCadastro" +
                ",@numeracaoNoLote" +
                ",@lote)";

            Add(novoCartao, queryInsert);

            return ObterCartaoPorHashNumero(novoCartao.numeroHash).First();
        }

        public Lote ObtemLoteParaCadastroManual()
        {
            var loteNovo = new Lote();

            loteNovo.tipoLote = "MANUAL";
            loteNovo.data = DateTime.Now.ToString("yyyyMMdd");
            loteNovo.dataProcessamento = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            loteNovo.qtdeRegistros = "1";


            string queryInsert = "insert into lote (tipoLote,data, dataProcessamento, qtdeRegistros)" +
                "values (" +
                "@tipoLote" +
                ",@data" +
                ",@dataProcessamento" +
                ",@qtdeRegistros)";




            _loteRepository.Add(loteNovo, queryInsert);

            return _loteRepository.ObtemLotePorParametros(loteNovo.tipoLote, loteNovo.dataProcessamento);

        }

        public Lote ObterLoteParaArquivo(string linhaDadosArquivo)
        {
            return _loteRepository.CriarLoteParaArquivo(linhaDadosArquivo);
        }


    }
}
