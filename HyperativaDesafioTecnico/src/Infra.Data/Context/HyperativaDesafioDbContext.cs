using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;

namespace HyperativaDesafio.Infra.Data.Context
{
    public class HyperativaDesafioDbContext
    {
        public SqliteConnection Connection { get; private set; }
        public HyperativaDesafioDbContext()
        {
            string dbPath = Environment.CurrentDirectory + @"\Database.sqlite";

            Connection = new SqliteConnection(@"Data Source = C:\Users\brn_a\Documents\_ESTUDOS\ASP_NET_CORE_MVC\HyperativaDesafio\HyperativaDesafioTecnico\src\Infra.Data\DataBase\hyperativaDesafio.db");
            Connection.Open();
        }


    }
}
