using Sql2Cdm.Library.Sql.Text;
using System.Data;
using System.Data.Common;

namespace Sql2Cdm.Library.Sql
{
    public class SqlTextCommandAdapter
    {
        private readonly DbConnection dbConnection;
        private readonly SqlTextScriptReader sqlTextScriptReader;

        public SqlTextCommandAdapter(DbConnection dbConnection, SqlTextScriptReader sqlTextScriptReader)
        {
            this.dbConnection = dbConnection;
            this.sqlTextScriptReader = sqlTextScriptReader;
        }

        public void ExecuteSqlCommand()
        {
            string sqlScript = sqlTextScriptReader.ReadAllText();

            if (dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }

            var command = dbConnection.CreateCommand();
            command.CommandText = sqlScript;
            command.ExecuteNonQuery();
        }
    }
}
