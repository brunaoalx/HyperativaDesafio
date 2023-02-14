using Microsoft.Data.Sqlite;

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
