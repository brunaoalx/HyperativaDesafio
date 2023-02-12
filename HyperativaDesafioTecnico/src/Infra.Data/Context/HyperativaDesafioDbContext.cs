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
            SetConnection();
        }

        private void SetConnection()
        {
            try
            {
                this.Connection = new SqliteConnection(@"Data Source=..\HyperativaDesafioTecnico\src\Infra.Data\DataBase\hyperativaDesafio.db\");
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
