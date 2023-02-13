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
            string dbPath = Environment.CurrentDirectory + @"\DataBase\hyperativaDesafio.db";

            Connection = new SqliteConnection("Data Source=" + dbPath);
            Connection.Open();
        }


    }
}
