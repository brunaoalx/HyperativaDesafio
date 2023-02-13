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
    public class LoteRepository : RepositoryBase<Lote>, ILoteRepository
    {

        //public LoteRepository(string connectioString)
        //    :base(connectioString)
        //{

        //}

        public Lote ObtemLotePorParametros(string tipoLote, string dataProcessamento)
        {
            return DbContext.Connection.Query<Lote>($"select * from lote where tipolote = '{tipoLote}' " +
                $" and dataProcessamento = '{dataProcessamento}'").FirstOrDefault() ?? new Lote();
                
        }

        public Lote CriarLoteParaArquivo(string linhaHeaderArquivo)
        {


            var loteNovo = new Lote();

            loteNovo.nome = linhaHeaderArquivo.Substring(0, 29);
            loteNovo.tipoLote = "ARQUIVO";
            loteNovo.data = linhaHeaderArquivo.Substring(29, 8);
            loteNovo.dataProcessamento = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            loteNovo.qtdeRegistros = linhaHeaderArquivo.Substring(45, 6);


            string queryInsert = "insert into lote (nome,tipoLote,data, dataProcessamento, qtdeRegistros)" +
                "values (" +
                "@nome" +
                ",@tipoLote" +
                ",@data" +
                ",@dataProcessamento" +
                ",@qtdeRegistros)";


            Add(loteNovo, queryInsert);

            return ObtemLotePorParametros(loteNovo.tipoLote, loteNovo.dataProcessamento);


        }


    }
}
