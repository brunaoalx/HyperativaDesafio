using Dapper;
using HyperativaDesafio.Domain.Entities;
using HyperativaDesafio.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperativaDesafio.Infra.Data.Repositories
{
    public class CartaoRepository : RepositoryBase<Cartao>, ICartaoRepository
    {

        private string _connectioString;

        public CartaoRepository(string connectioString)
            : base(connectioString)
        {
            _connectioString = connectioString;
        }
        public IEnumerable<Cartao> ObterCartaoPorHashNumero(string hashNumber)
        {

            var cartoes = DbContext.Connection.Query<Cartao>($"select * from cartao where numeroHash = '{hashNumber}'");

            return cartoes;
        }

        public Cartao CadastrarCartao(Cartao novoCartao)
        {

            string queryInsert = 
                $"insert into cartao (" +
                $",numeroHash" +
                $",numeroMascara " +
                $",numeracaoLote)" +
                $",Lote)" +
                $"values(" +
                $" '{novoCartao.numeroHash}'" +
                $",'{novoCartao.numeroMascara}'" +
                $",'{novoCartao.dataCadastro}'" + 
                $",'{novoCartao.numeracaoNoLote}'" +
                $",'{novoCartao.dataCadastro}'" +
                $",{novoCartao.lote})";

            DbContext.Connection.Execute(queryInsert);

            return ObterCartaoPorHashNumero(novoCartao.numeroHash).First();
        }
        

        public Lote ObtemLoteParaCadastroManual() 
        { 
            var loteNovo = new Lote();

            loteNovo.tipoLote = "MANUAL";
            loteNovo.data = DateTime.Now.ToString("yyyy-MM-dd");
            loteNovo.dataProcessamento = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            loteNovo.qtdeRegistros = "1";


            string queryInsert = "insert into lote (tipoLote,data, dataProcessamento, qtdeRegistros)" +
                $"values ('{loteNovo.tipoLote}'" +
                $"values ,'{loteNovo.data}'" +
                $"values ,'{loteNovo.dataProcessamento}'" +
                $"values ,'{loteNovo.qtdeRegistros}')"; 


            LoteRepository loteRepository = new LoteRepository(_connectioString);

            loteRepository.Add(loteNovo, queryInsert);

            return loteRepository.ObtemLotePorParametros(loteNovo.tipoLote, loteNovo.dataProcessamento);
        
        }


    }
}
