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
        public HyperativaDesafioDbContext(string connectionString)
        {
            SetConnection(connectionString);
        }

        private void SetConnection(string connectionString)
        {
            try
            {
                this.Connection = new SqliteConnection(connectionString);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
